namespace Scene
{
    public class IngameScene : IScene
    {
        protected override void OnLoad( )
        {
            // 컨텐츠 등록
            _contentList.Add(typeof(Content.IngameContent));

            // 로드 씬 이름 지정
            _loadingMapStr = "IngameScene";

            base.OnLoad( );
        }

        protected override void OnLoadComplete( )
        {
            UnityEngine.GameObject obj = new UnityEngine.GameObject( );
            _loadedLogic = obj.AddComponent<Logic.IngameLogic>( );
            _loadedLogic.Setup( );
        }
    }
}