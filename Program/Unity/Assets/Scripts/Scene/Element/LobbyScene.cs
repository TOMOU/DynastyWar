namespace Scene
{
    public class LobbyScene : IScene
    {
        protected override void OnLoad( )
        {
            // 컨텐츠 등록
            _contentList.Add(typeof(Content.LobbyContent));

            // 로드 씬 이름 지정
            _loadingMapStr = "LobbyScene";

            base.OnLoad( );
        }

        protected override void OnLoadComplete( )
        {
            UnityEngine.GameObject obj = new UnityEngine.GameObject( );
            _loadedLogic = obj.AddComponent<Logic.LobbyLogic>( );
            _loadedLogic.Setup( );
        }
    }
}