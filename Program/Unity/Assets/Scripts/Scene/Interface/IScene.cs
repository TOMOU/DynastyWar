using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scene
{
    public class IScene : MonoBehaviour
    {
        public Constant.GameScene sceneName;
        protected List<Type> _contentList = new List<Type>( );
        protected List<Content.IContent> _loadedContentList = new List<Content.IContent>( );
        protected Logic.ILogic _loadedLogic;
        protected string _loadingMapStr = string.Empty;
        protected Constant.OnComplete _onComplete = null;

        public void Load(Constant.OnComplete onComplete)
        {
            _onComplete = onComplete;

            // 이름 변경
            gameObject.name = GetType( ).Name;

            // Content List를 세팅한다 (상속받는 자식 클래스에서)
            OnLoad( );

            // 세팅이 완료되었으니 Content 로딩을 시작한다.
            StartCoroutine(Process_Loading( ));

            // GameManager에 맞는 부모 연결을 위해 메세지 전달
            Message.Send<Global.TransformAttachMsg>(new Global.TransformAttachMsg(Constant.BehaviorType.Scene, this.transform));
        }

        public IEnumerator Unload( )
        {
            OnUnload( );

            if (_loadedLogic != null)
            {
                _loadedLogic.ReleaseLogic( );
                Destroy(_loadedLogic.gameObject);
            }

            foreach (Content.IContent c in _loadedContentList)
            {
                yield return StartCoroutine(c.Unload( ));
            }

            _loadedContentList.Clear( );

            while (_loadedContentList.Count > 0)
                yield return null;

            Destroy(this.gameObject);
        }

        protected virtual void OnLoad( ) { Message.Send<Global.StopAllSoundMsg>(new Global.StopAllSoundMsg( )); }

        protected virtual void OnUnload( ) { }

        protected virtual void OnLoadComplete( ) { }

        protected IEnumerator Process_Loading( )
        {
            yield return StartCoroutine(Process_ContentLoading( ));
        }

        protected IEnumerator Process_ContentLoading( )
        {
            // _contentList 갯수만큼 반복한다.
            foreach (Type t in _contentList)
            {
                GameObject content = new GameObject( );
                content.SetActive(true);

                // 컨텐츠 스크립트 초기화 및 완료 시 동작할 리스트
                var script = content.AddComponent(t) as Content.IContent;
                if (script != null)
                {
                    yield return StartCoroutine(script.Load(( ) =>
                    {
                        _loadedContentList.Add(script);
                        Message.Send<Content.EnterContentMsg>(t.ToString( ).Split('.') [1], new Content.EnterContentMsg( ));
                    }));
                }
                else
                {
                    Debug.LogErrorFormat("Content 생성 실패. 스크립트를 확인해 주세요.\nIContent = {0}", t.ToString( ));
                }

                yield return null;
            }

            yield return StartCoroutine(Process_MapLoading( ));

            OnLoadComplete( );

            if (_onComplete != null)
                _onComplete( );
        }

        protected IEnumerator Process_MapLoading( )
        {
            if (string.IsNullOrEmpty(_loadingMapStr))
                yield break;

            var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_loadingMapStr);
            while (async.isDone == false)
                yield return null;
        }
    }
}