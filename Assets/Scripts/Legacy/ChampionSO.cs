using System.Collections.Generic;

using Scripts.Data;

using UnityEngine;

namespace Scripts.Legacy
{
    [CreateAssetMenu(fileName = "ChampionSO", menuName = "LOL/ChampionSO", order = 0)]
    public class ChampionSO : ScriptableObject
    {
        public string version;

        [Header("기본 정보")]
        public string championId;
        public string championName;
        public string title;
        [TextArea] public string blurb;

        [Header("정보")]
        public ChampionInfo info;

        [Header("스탯")]
        public ChampionStats stats;

        [Header("이미지")]
        public Sprite icon;
        public Sprite portrait;
        public Sprite splashArt;

        [Header("역할 및 태그")]
        public RoleType mainRole;
        public List<RoleType> subRoles;
        public List<TagType> tags;
    }
}
