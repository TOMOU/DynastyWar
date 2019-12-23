using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class IDialog : MonoBehaviour
    {
        public GameObject dialogView;
        protected Constant.OnComplete _onComplete = null;

        public IEnumerator Load(Constant.OnComplete onComplete)
        {
            _onComplete = onComplete;

            // 이름 변경
            gameObject.name = GetType( ).Name;

            // dialogView 세팅
            if (dialogView == null)
                dialogView = transform.Find("View").gameObject;

            dialogView.SetActive(false);

            // 기본 메세지 등록
            Message.AddListener<EnterDialogMsg>(GetType( ).Name, OnEnterDialogMsg);
            Message.AddListener<ExitDialogMsg>(GetType( ).Name, OnExitDialogMsg);

            // 각 UI에 필요한 컴포넌트 캐싱
            OnLoad( );

            if (_onComplete != null)
                _onComplete( );

            // GameManager에 맞는 부모 연결을 위해 메세지 전달
            Message.Send<Global.TransformAttachMsg>(new Global.TransformAttachMsg(Constant.BehaviorType.Dialog, this.transform));

            yield return null;
        }

        public IEnumerator Unload( )
        {
            // 기본 메세지 등록해제
            Message.RemoveListener<EnterDialogMsg>(GetType( ).Name, OnEnterDialogMsg);
            Message.RemoveListener<ExitDialogMsg>(GetType( ).Name, OnExitDialogMsg);

            OnUnload( );

            yield return null;

            Destroy(this.gameObject);
        }

        protected virtual void OnLoad( ) { }
        protected virtual void OnUnload( ) { }
        protected virtual void OnEnter( ) { }
        protected virtual void OnExit( ) { }

        // Call : Active Dialog (이 함수를 호출 시 UI 활성화)
        public static void RequestDialogEnter<T>( ) where T : IDialog
        {
            Message.Send<EnterDialogMsg>(typeof(T).Name, new EnterDialogMsg( ));
        }

        // Call : Inactive Dialog (이 함수를 호출 시 UI 비활성화)
        public static void RequestDialogExit<T>( ) where T : IDialog
        {
            Message.Send<ExitDialogMsg>(typeof(T).Name, new ExitDialogMsg( ));
        }

        private void OnEnterDialogMsg(EnterDialogMsg msg)
        {
            if (dialogView != null)
                dialogView.SetActive(true);

            OnEnter( );
        }

        private void OnExitDialogMsg(ExitDialogMsg msg)
        {
            if (dialogView != null)
                dialogView.SetActive(false);

            OnExit( );
        }
    }
}