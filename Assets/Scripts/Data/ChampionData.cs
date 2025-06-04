using System;

using UnityEngine;

namespace Scripts.Data
{
    [Serializable]
    public class ChampionData
    {
        public string version;

        public string id;
        public string name;
        public string title;
        public string blurb;

        public ChampionInfo info;
        public string[] tags;

        public ChampionStats stats;

        public Sprite portraitSprite;
        public Sprite loadingSprite;
        public Sprite splashSprite;
    }
}
