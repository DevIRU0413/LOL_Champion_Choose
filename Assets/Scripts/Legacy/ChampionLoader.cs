using System.Collections.Generic;

using Scripts.Data;

using UnityEngine;

namespace Scripts.Legacy
{
    public class ChampionLoader : MonoBehaviour
    {
        [SerializeField] private List<ChampionSO> champions;

        public List<ChampionSO> GetAllChampions() => champions;

        public ChampionSO GetChampionByName(string name)
        {
            return champions.Find(c => c.championName == name);
        }
    }
}
