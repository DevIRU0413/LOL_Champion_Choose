using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.Legacy
{
    public static class ImageLoader
    {
        public static IEnumerator LoadSpriteFromURL(string url, System.Action<Sprite> callback)
        {
            using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url);
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[ImageLoader] 이미지 로드 실패: {url} -> {uwr.error}");
                callback?.Invoke(null);
            }
            else
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
                callback?.Invoke(sprite);
            }
        }
    }
}
