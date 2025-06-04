namespace Scripts.Util
{
    public enum SceneID
    {
        AppInitialize,  // AppInitialize 0
        Title,          // Title 1
        Roulette,
    }

    // 게임 상태용
    public enum PlayState
    {
        None,     // 기본 상태 (선택사항)
        Playing,  // 게임이 진행 중
        Paused,   // 일시 정지
        Stopped   // 강제 멈춤
    }

    // 게임 상태용
    public enum ManagerPriority
    {
        None,                   // Error

        ResourceManager, // Manager
        SceneManagerEx,
        AudioManager,
        UIManager,

        AppInitializer,

        ChampionManager,
        MonsterManager,
    }
}
