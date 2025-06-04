using System.Collections;

using Scripts.Data;
using Scripts.Tool;

using UnityEngine;

namespace Scripts.Legacy
{
    public class ChampionImageDownloader : MonoBehaviour
    {
        private const string savePortraitPath = "Image/ChampionPortrait/";
        private const string saveLoadingPath = "Image/ChampionLoading/";
        private const string saveSplashPath = "Image/ChampionSplash/";

        public IEnumerator ApplyImagesToSO(ChampionSO so, string championId, string version)
        {
            string portraitUrl = Links.ChampionPortraitURL(version, championId);
            string loadingUrl = Links.ChampionLoadingURL(championId);
            string splashUrl = Links.ChampionSplashURL(championId);

            Sprite iconSprite = null;
            Sprite portraitSprite = null;
            Sprite splashSprite = null;

            // 아이콘
            yield return ImageLoader.LoadSpriteFromURL(portraitUrl, s => iconSprite = s);
            // 초상화
            yield return ImageLoader.LoadSpriteFromURL(loadingUrl, s => portraitSprite = s);
            // 일러스트
            yield return ImageLoader.LoadSpriteFromURL(splashUrl, s => splashSprite = s);

            if (so != null)
            {
                // 이미지 변환
                ImageSaver.SaveTextureToPNG(iconSprite.texture, savePortraitPath, Links.ChampionPortraitName(championId));
                ImageSaver.SaveTextureToPNG(portraitSprite.texture, saveLoadingPath, Links.ChampionLoadingName(championId));
                ImageSaver.SaveTextureToPNG(splashSprite.texture, saveSplashPath, Links.ChampionSplashName(championId));

                // so에 할당
                so.icon = iconSprite;
                so.portrait = portraitSprite;
                so.splashArt = splashSprite;

                Debug.Log($"[ChampionImageDownloader] {championId} 이미지 적용 완료");
            }
        }
    }

}
