using Scripts.Managers;
using Scripts.Util;

namespace Scripts.Scene
{
    public class AppInitializeScene : SceneBase
    {
        public override SceneID SceneID => SceneID.AppInitialize;

        private bool _isAction;

        protected override void Initialize()
        {
            return;
        }


        private void Start()
        {
            SceneManagerEx.Instance.OnSceneBeforeChange.Add(() => AppInitializer.Instance.InitializeVersion());
            SceneManagerEx.Instance.OnSceneBeforeChange.Add(() => AppInitializer.Instance.InitializeChampionDatas());
            SceneManagerEx.Instance.OnSceneBeforeChange.Add(() => AppInitializer.Instance.InitializeChampionFormatSetting());

        }


        private void Update()
        {

        }

        public void NextScene()
        {
            SceneManagerEx.Instance.LoadSceneAsync("MainTitleScene");
            _isAction = true;
        }


    }
}
