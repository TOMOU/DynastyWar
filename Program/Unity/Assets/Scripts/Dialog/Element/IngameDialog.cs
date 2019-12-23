using UnityEngine.UI;

namespace Dialog
{
    public class IngameDialog : IDialog
    {
        private Button _btNext;
        private Text _txtDescription;

        protected override void OnLoad( )
        {
            _btNext = dialogView.transform.Find("btNext").GetComponent<Button>( );
            _txtDescription = dialogView.transform.Find("txtDescription").GetComponent<Text>( );

            _btNext.onClick.AddListener(OnClickNext);
            _txtDescription.text = string.Format("{0}", GetType( ).Name);
        }

        protected override void OnUnload( )
        {
            _btNext.onClick.RemoveAllListeners( );
        }

        protected override void OnEnter( )
        {

        }

        protected override void OnExit( )
        {

        }

        private void OnClickNext( )
        {
            Message.Send<Global.ChangeSceneMsg>(new Global.ChangeSceneMsg(Constant.GameScene.Title));
        }
    }
}