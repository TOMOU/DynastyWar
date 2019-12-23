namespace Content
{
    public class TitleContent : IContent
    {
        protected override void OnLoad( )
        {
            // UI 등록
            _dialogList.Add(typeof(Dialog.TitleDialog));

            base.OnLoad( );
        }

        protected override void OnUnload( )
        {

        }

        protected override void OnEnter( )
        {
            Dialog.IDialog.RequestDialogEnter<Dialog.TitleDialog>( );
        }

        protected override void OnExit( )
        {
            Dialog.IDialog.RequestDialogExit<Dialog.TitleDialog>( );
        }
    }
}