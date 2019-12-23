namespace Content
{
    public class LobbyContent : IContent
    {
        protected override void OnLoad( )
        {
            // UI 등록
            _dialogList.Add(typeof(Dialog.LobbyDialog));

            base.OnLoad( );
        }

        protected override void OnUnload( )
        {

        }

        protected override void OnEnter( )
        {
            Dialog.IDialog.RequestDialogEnter<Dialog.LobbyDialog>( );
        }

        protected override void OnExit( )
        {
            Dialog.IDialog.RequestDialogExit<Dialog.LobbyDialog>( );
        }
    }
}