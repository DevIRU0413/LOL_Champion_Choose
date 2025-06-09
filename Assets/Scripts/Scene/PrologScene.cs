using Scripts.Managers;
using Scripts.Util;

using UnityEngine;

namespace Scripts.Scene
{
    public class PrologScene : SceneBase
    {
        public override SceneID SceneID => SceneID.PrologScene;

        protected override void Initialize()
        {
        }

        public void NextScene()
        {
            SceneManagerEx.Instance.LoadSceneAsync(SceneID);
        }
    }
}
