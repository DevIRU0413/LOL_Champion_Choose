using Scripts.Data;
using Scripts.Interface;
using Scripts.Util;

using UnityEngine;

namespace Scripts.Managers
{
    public class ChampionManager : SimpleSingleton<ChampionManager>, IManager
    {
        public int Priority => (int)ManagerPriority.ChampionManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        public ChampionListRoot list;

        public void Cleanup()
        {
            return;
        }
        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Initialize()
        {
        }
    }
}
