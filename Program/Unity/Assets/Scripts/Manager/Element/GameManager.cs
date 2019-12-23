using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Scene;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private Transform _managerGroup;
        private Transform _logicGroup;
        private Transform _sceneGroup;
        private Transform _contentGroup;
        private Transform _dialogGroup;
        [SerializeField] private Constant.GameScene _curScene = Constant.GameScene.None;
        [SerializeField] private Scene.IScene _loadedScene;

        private void Awake( )
        {
            DontDestroyOnLoad(this.gameObject);
            Init( );
        }

        protected override void Init( )
        {
            _managerGroup = transform.Find("Managers");
            _logicGroup = transform.Find("Logics");
            _sceneGroup = transform.Find("Scenes");
            _contentGroup = transform.Find("Contents");
            _dialogGroup = transform.Find("Dialogs/Canvas");

            // 그룹 하위를 전부 비운다.
            foreach (Transform child in _managerGroup)
                Destroy(child.gameObject);
            foreach (Transform child in _logicGroup)
                Destroy(child.gameObject);
            foreach (Transform child in _sceneGroup)
                Destroy(child.gameObject);
            foreach (Transform child in _contentGroup)
                Destroy(child.gameObject);
            foreach (Transform child in _dialogGroup)
                Destroy(child.gameObject);

            // Message 등록
            Message.AddListener<Global.ChangeSceneMsg>(OnChangeScene);
            Message.AddListener<TransformAttachMsg>(OnTransformAttach);

            // Manager Instance 생성
            Init_First( );

            if (_curScene == Constant.GameScene.None)
            {
                Message.Send<Global.ChangeSceneMsg>(new Global.ChangeSceneMsg(Constant.GameScene.Title));
            }
        }

        protected override void Release( )
        {
            // Message 등록해제
            Message.RemoveListener<Global.ChangeSceneMsg>(OnChangeScene);
            Message.RemoveListener<TransformAttachMsg>(OnTransformAttach);
        }

        private void Init_First( )
        {
            // 모델 로드
            GameModel gm = new GameModel( );
            gm.Setup( );

            Manager.SoundManager.Singleton.Setup( );
        }

        private void OnChangeScene(Global.ChangeSceneMsg msg)
        {
            // 메세지와 현재 씬이 같으면 중복 로드할 수 있으므로 예외처리.
            if (_curScene == msg.scene)
                return;

            if (msg.scene == Constant.GameScene.Title)
            {
                _curScene = msg.scene;
                StartCoroutine(LoadRoot<TitleScene>(_curScene));
            }
            else if (msg.scene == Constant.GameScene.Lobby)
            {
                _curScene = msg.scene;
                StartCoroutine(LoadRoot<LobbyScene>(_curScene));
            }
            else if (msg.scene == Constant.GameScene.Ingame)
            {
                _curScene = msg.scene;
                StartCoroutine(LoadRoot<IngameScene>(_curScene));
            }
        }

        private IEnumerator LoadRoot<T>(Constant.GameScene scene) where T : IScene
        {
            if (_loadedScene != null)
            {
                yield return StartCoroutine(_loadedScene.Unload( ));
            }

            GameObject obj = new GameObject( );

            // 씬 스크립트 초기화 및 완료 후 리스트
            var script = obj.AddComponent<T>( );
            if (script != null)
            {
                script.Load(( ) =>
                {
                    _loadedScene = script;
                    SortUISibling( );
                });
            }
            else
            {
                Debug.LogErrorFormat("Scene 생성 실패. 스크립트를 확인해 주세요.\nIScene = {0}", typeof(T).ToString( ));
            }

            yield return null;
        }

        private void OnTransformAttach(TransformAttachMsg msg)
        {
            if (msg.type == Constant.BehaviorType.Manager)
                msg.transform.SetParent(_managerGroup, false);
            else if (msg.type == Constant.BehaviorType.Logic)
                msg.transform.SetParent(_logicGroup, false);
            else if (msg.type == Constant.BehaviorType.Scene)
                msg.transform.SetParent(_sceneGroup, false);
            else if (msg.type == Constant.BehaviorType.Content)
                msg.transform.SetParent(_contentGroup, false);
            else if (msg.type == Constant.BehaviorType.Dialog)
                msg.transform.SetParent(_dialogGroup, false);
        }

        private void SortUISibling( )
        {
            Dictionary<int, Transform> uiDic = new Dictionary<int, Transform>( );
            foreach (Transform child in _dialogGroup)
            {
                int idx = (int) Enum.Parse(typeof(Constant.UISibling), child.name);
                uiDic.Add(idx, child);
            }

            int maxCount = (int) Enum.Parse(typeof(Constant.UISibling), "Max");
            Transform source = null;
            for (int i = 0; i < maxCount; i++)
            {
                if (uiDic.TryGetValue(i, out source) == true)
                {
                    source.SetAsLastSibling( );
                }
            }

            uiDic.Clear( );
            uiDic = null;
        }
    }
}