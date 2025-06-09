using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;


namespace Scripts.Tool
{
    public class Loader
    {
        public static IEnumerator DownloadJSON(string jsonURL, Action<string> onCallBack)
        {
            using UnityWebRequest request = UnityWebRequest.Get(jsonURL);
            yield return request.SendWebRequest(); // 보낸 응답이 올 때까지 대기

            // 응답 실패
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"JSON 다운로드 실패: {request.error}");
                onCallBack?.Invoke(null);
                yield break;
            }

            // 응답 성공
            onCallBack?.Invoke(request.downloadHandler.text);
        }

        public static IEnumerator LoadSprite(string url, System.Action<Sprite> callback)
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
