namespace Scripts.Util
{
    public enum SceneID
    {
        None = 0,
        LoadingScene, // 범용적인 로딩 시 사용
        DataLoadingScene, // 개별적인 데이터 로드 시 사용
        PrologScene,
        MainTitleScene, 
        RouletteScene,
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

        ResourceManager,        // Default Manager
        SceneManagerEx,
        AudioManager,
        UIManager,

        DataStore,              // Data Manager

        ChampionManager,        // Add Manager
        MonsterManager,
    }
}
