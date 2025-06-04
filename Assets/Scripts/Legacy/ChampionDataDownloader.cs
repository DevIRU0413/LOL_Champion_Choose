using System;
using System.Collections;

using Newtonsoft.Json;

using Scripts.Data;

using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.Legacy
{
    public class ChampionDataDownloader
    {
        public static IEnumerator DownloadChampionList<T>(string version, Action<T> onComplete)
        {
            using UnityWebRequest request = UnityWebRequest.Get(Links.ChampionListURL(version));
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"JSON 다운로드 실패: {request.error}");
                yield break;
            }

            var json = request.downloadHandler.text;
            var root = JsonConvert.DeserializeObject<ChampionListRoot>(json);
            // onComplete?.Invoke(root.data);
        }
    }
}
