namespace Content
{
    public class IngameContent : IContent
    {
        protected override void OnLoad( )
        {
            // UI 등록
            _dialogList.Add(typeof(Dialog.IngameDialog));

            base.OnLoad( );
        }

        protected override void OnUnload( )
        {

        }

        protected override void OnEnter( )
        {
            Dialog.IDialog.RequestDialogEnter<Dialog.IngameDialog>( );
        }

        protected override void OnExit( )
        {
            Dialog.IDialog.RequestDialogExit<Dialog.IngameDialog>( );
        }
    }
}