#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;

using Scripts.Data;

using UnityEditor;

using UnityEngine;

namespace Scripts.Legacy
{
    public class ChampionSOGenerator : MonoBehaviour
    {
        private const string saveSOPath = "Assets/Resources/ScriptableObjects/Champions/";

        [MenuItem("Tools/LOL/Create Champion SOs from JSON")]
        public static void GenerateAllChampionSOs()
        {
            var dataDownloader = new GameObject("DataDownloader");
            // dataDownloader.StartCoroutine(dataDownloader.DownloadChampionList(CreateSOAssets));
            // DestroyImmediate(downloader);
        }

        private static void CreateSOAssets(Dictionary<string, ChampionData> data)
        {
            if (!Directory.Exists(saveSOPath))
                Directory.CreateDirectory(saveSOPath);

            var imageDownloader = new GameObject("ImageDownloader").AddComponent<ChampionImageDownloader>();
            var versionDownloader = new GameObject("VersionDownloader").AddComponent<VersionJsonDownloader>();

            versionDownloader.StartCoroutine(versionDownloader.DownloadVersionJson());

            Coroutine imgCo = null;
            foreach (var entry in data)
            {
                ChampionData cd = entry.Value;

                var so = ScriptableObject.CreateInstance<ChampionSO>();

                so.version = cd.version;

                so.championId = cd.id;
                so.championName = cd.name;
                so.title = cd.title;
                so.blurb = cd.blurb;

                so.stats = cd.stats;

                so.tags = MapTags(cd.tags);

                so.mainRole = RoleType.Mid; // 기본값 지정 (실제 데이터엔 없음)
                so.subRoles = new List<RoleType> { };

                imgCo = imageDownloader.StartCoroutine(imageDownloader.ApplyImagesToSO(so, cd.id, cd.version));

                string fileName = $"{saveSOPath}ChampionSO_{cd.id}.asset";
                AssetDatabase.CreateAsset(so, fileName);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();



            Debug.Log($"챔피언 SO {data.Count}개 생성 완료");
        }

        public static List<TagType> MapTags(string[] rawTags)
        {
            var result = new List<TagType>();

            foreach (var tag in rawTags)
            {
                if (Enum.TryParse<TagType>(tag, ignoreCase: true, out var parsedTag))
                {
                    result.Add(parsedTag);
                }
                else
                {
                    Debug.LogWarning($"[TagMapper] 변환 실패: '{tag}'는 TagType enum에 없습니다.");
                }
            }

            return result;
        }
    }
}
#endif
