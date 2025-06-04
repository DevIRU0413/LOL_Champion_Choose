using System.Collections;

using Scripts.Tool;

using UnityEngine;

using UnityEngine.Networking;

namespace Scripts.Legacy
{
    public static class VersionChecker
    {
        public static IEnumerator GetLatestVersion(System.Action<string> onComplete)
        {
            string url = "https://ddragon.leagueoflegends.com/api/versions.json";
            using UnityWebRequest request = UnityWebRequest.Get(url);

            if (request.result != UnityWebRequest.Result.Success)
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var json = request.downloadHandler.text;
                    var versions = JsonHelper.FromJson<string>(json);
                    onComplete?.Invoke(versions[0]); // 최신 버전
                }
                else
                {
                    Debug.LogError("버전 체크 실패: " + request.error);
                    onComplete?.Invoke(null);
                }
            }
        }


    }
}
