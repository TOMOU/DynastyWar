namespace Scene
{
    public class TitleScene : IScene
    {
        protected override void OnLoad( )
        {
            // 컨텐츠 등록
            _contentList.Add(typeof(Content.TitleContent));

            // Global 컨텐츠 (처음 실행 시에만 로드)
            if (Model.First<GameModel>( ).loadCompleteGlobalContent == false)
                _contentList.Add(typeof(Content.GlobalContent));

            // 로드 씬 이름 지정
            _loadingMapStr = "TitleScene";

            base.OnLoad( );
        }

        protected override void OnLoadComplete( )
        {
            UnityEngine.GameObject obj = new UnityEngine.GameObject( );
            _loadedLogic = obj.AddComponent<Logic.TitleLogic>( );
            _loadedLogic.Setup( );

            Message.Send<Global.PlaySoundMsg>(new Global.PlaySoundMsg(Constant.SoundName.Tetris));
        }
    }
}