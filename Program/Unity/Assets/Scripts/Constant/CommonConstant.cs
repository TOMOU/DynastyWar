namespace Constant
{
    public enum GameScene
    {
        None,
        Title,
        Lobby,
        Ingame,
    }

    public enum BehaviorType
    {
        Manager,
        Logic,
        Scene,
        Content,
        Dialog,
    }

    public enum UISibling
    {
        // ==============================
        // TitleScene
        // ==============================
        TitleDialog,
        // ==============================
        // LobbyScene
        // ==============================
        LobbyDialog,
        // ==============================
        // IngameScene
        // ==============================
        IngameDialog,
        // ==============================
        // Global
        // ==============================

        Max
    }

    public delegate void OnComplete( );
}