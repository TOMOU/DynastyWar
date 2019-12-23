using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Content
{
    public class IContent : MonoBehaviour
    {
        protected const string _PATH = "UI/{0}";
        protected List<Type> _dialogList = new List<Type>( );
        [SerializeField] protected List<Dialog.IDialog> _loadedDialogList = new List<Dialog.IDialog>( );
        protected bool _isLoadingComplete = false;
        private Constant.OnComplete _onComplete = null;
        protected bool _isGlobalContent = false;

        public IEnumerator Load(Constant.OnComplete onComplete)
        {
            _onComplete = onComplete;

            // 이름 변경
            gameObject.name = GetType( ).Name;

            // 기본 메세지 등록
            Message.AddListener<EnterContentMsg>(GetType( ).Name, OnEnterContentMsg);
            Message.AddListener<ExitContentMsg>(GetType( ).Name, OnExitContentMsg);

            // UI List를 세팅한다. (상속받는 자식 클래스에서)
            OnLoad( );

            // 세팅이 완료되었으니 UI 로딩을 시작한다.
            yield return StartCoroutine(Process_Loading( ));

            // GameManager에 맞는 부모 연결을 위해 메세지 전달
            Message.Send<Global.TransformAttachMsg>(new Global.TransformAttachMsg(Constant.BehaviorType.Content, this.transform));
        }

        public IEnumerator Unload( )
        {
            if (_isGlobalContent)
                yield break;

            // 기본 메세지 등록해제
            Message.RemoveListener<EnterContentMsg>(GetType( ).Name, OnEnterContentMsg);
            Message.RemoveListener<ExitContentMsg>(GetType( ).Name, OnExitContentMsg);

            OnUnload( );

            foreach (Dialog.IDialog d in _loadedDialogList)
            {
                yield return StartCoroutine(d.Unload( ));
            }

            _loadedDialogList.Clear( );

            while (_loadedDialogList.Count > 0)
                yield return null;

            Destroy(this.gameObject);
        }

        private IEnumerator Process_Loading( )
        {
            // UI 로딩을 점진적으로 실행 및 완료될 때까지 대기
            yield return StartCoroutine(Process_UILoading( ));
        }

        private IEnumerator Process_UILoading( )
        {
            // _uiList 갯수만큼 반복한다.
            foreach (Type t in _dialogList)
            {
                GameObject ui = null;
                string separatedName = t.ToString( ).Split('.') [1];
                var async = Resources.LoadAsync(string.Format(_PATH, separatedName), typeof(GameObject));
                yield return async;

                if (async.asset != null)
                {
                    ui = Instantiate(async.asset) as GameObject;
                    ui.SetActive(true);
                    ui.name = async.asset.name;

                    var script = ui.AddComponent(t) as Dialog.IDialog;
                    if (script != null)
                    {
                        yield return StartCoroutine(script.Load(( ) =>
                        {
                            _loadedDialogList.Add(script);
                        }));
                    }
                    else
                    {
                        Debug.LogErrorFormat("UI 생성 실패. 스크립트의 이름과 프리팹 이름이 같은지, 혹은 프리팹이 존재하는지 확인해주세요.\nuiName = {0}", separatedName);
                    }
                }
                else
                {
                    Debug.LogErrorFormat("UI 생성 실패. 스크립트의 이름과 프리팹 이름이 같은지, 혹은 프리팹이 존재하는지 확인해주세요.\nuiName = {0}", separatedName);
                }

                yield return null;
            }

            if (_onComplete != null)
                _onComplete( );
        }

        protected virtual void OnLoad( ) { }
        protected virtual void OnUnload( ) { }
        protected virtual void OnEnter( ) { /* BLANK */ }
        protected virtual void OnExit( ) { /* BLANK */ }

        public static void RequestContentEnter<T>( ) where T : IContent
        {
            Message.Send<EnterContentMsg>(typeof(T).Name, new EnterContentMsg( ));
        }

        public static void RequestContentExit<T>( ) where T : IContent
        {
            Message.Send<ExitContentMsg>(typeof(T).Name, new ExitContentMsg( ));
        }

        private void OnEnterContentMsg(EnterContentMsg msg)
        {
            OnEnter( );
        }

        private void OnExitContentMsg(ExitContentMsg msg)
        {
            OnExit( );
        }
    }
}