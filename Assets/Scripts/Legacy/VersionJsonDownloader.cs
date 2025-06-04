using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;

namespace Scripts.Legacy
{
    public class VersionJsonDownloader : MonoBehaviour
    {
        private string savePath => Path.Combine(Application.persistentDataPath, "VersionJSON", "versions.json");

        public IEnumerator DownloadVersionJson()
        {
            string url = "https://ddragon.leagueoflegends.com/api/versions.json";
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                File.WriteAllText(savePath, request.downloadHandler.text);
                Debug.Log($"버전 정보 저장 완료: {savePath}");
            }
            else
            {
                Debug.LogError("버전 정보 다운로드 실패: " + request.error);
            }
        }
    }
}
