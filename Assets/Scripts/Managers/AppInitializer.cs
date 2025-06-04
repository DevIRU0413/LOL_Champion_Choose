using System.Collections;
using System.IO;

using Newtonsoft.Json;

using Scripts.Data;
using Scripts.Interface;
using Scripts.Tool;
using Scripts.Util;

using UnityEngine;


namespace Scripts.Managers
{
    /// <summary>
    /// 앱 초기화를 담당하는 매니저 클래스
    /// </summary>
    public class AppInitializer : SimpleSingleton<AppInitializer>, IManager
    {
        private string defaultVersion = "0.0.0";
        private string releasedVersion = "0.0.0";

        private string folderName = "LOL_Released_Version";
        private string fileName = "ReleasedVersion.json";

        private string folderPath;
        private string filePath;

        private float progress = 0;

        private ChampionListRoot m_championList;

        /// <summary>
        /// 매니저의 우선순위
        /// </summary>
        public int Priority => (int)ManagerPriority.AppInitializer;

        /// <summary>
        /// 씬 전환 시 파괴되지 않을지 여부
        /// </summary>
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        /// <summary>
        /// 매니저 초기화
        /// </summary>
        public void Initialize()
        {
            folderPath = Path.Combine(Application.persistentDataPath, folderName);
            filePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        }

        /// <summary>
        /// 매니저 정리
        /// </summary>
        public void Cleanup()
        {
            return;
        }

        /// <summary>
        /// 게임오브젝트 반환
        /// </summary>
        public GameObject GetGameObject()
        {
            return this == null ? null : gameObject; ;
        }

        public IEnumerator InitializeVersion()
        {
            // 1. 로컬 버전 불러오기
            string localVersion = GetLocalVersion();

            // 2. 최신 버전 확인
            yield return StartCoroutine(Loader.DownloadJSON(Links.VersionURL, GetStringByFromJson));
            if (string.IsNullOrEmpty(releasedVersion))
            {
                Debug.LogError("최신 버전 정보를 가져오지 못했습니다.");
                yield break;
            }

            Debug.Log($"Local: {localVersion}, Latest: {releasedVersion}");

            // 3. 최신 버전 갱신
            if (localVersion != releasedVersion)
                SaveLocalVersion(releasedVersion);
        }

        public IEnumerator InitializeChampionDatas()
        {
            // 4. 챔피언 데이터 포멧에 맞게, 챔피언 리스트 생성
            yield return StartCoroutine(Loader.DownloadJSON(Links.ChampionListURL(releasedVersion), GetChampionList));
            if (string.IsNullOrEmpty(releasedVersion))
            {
                Debug.LogError("챔피언 데이터 포멧 리스트 생성 실패");
                yield break;
            }
        }

        public IEnumerator InitializeChampionFormatSetting()
        {
            // 5. 챔피인 리스트의 챔피언 데이터에 이미지 할당
            foreach (var c in m_championList.data)
            {
                var chamInfo = c.Value;

                string portraitFilePath = Links.SavePortraitPath + Links.ChampionPortraitName(chamInfo.id);
                string loadingFilePath = Links.SaveLoadingPath + Links.ChampionLoadingName(chamInfo.id);
                string splashFilePath = Links.SaveSplashPath + Links.ChampionSplashName(chamInfo.id);

                bool portraitPahtCheck = File.Exists(portraitFilePath);
                bool LoadingPathCheck = File.Exists(loadingFilePath);
                bool splashPathCheck = File.Exists(splashFilePath);

                // 초상화
                if (portraitPahtCheck)
                    GetImage(portraitFilePath, ref chamInfo.portraitSprite);
                else
                {
                    string portraitUrl = Links.ChampionPortraitURL(chamInfo.version, chamInfo.id);
                    yield return StartCoroutine(Loader.LoadSprite(portraitUrl, s => chamInfo.portraitSprite = s));

                    ImageSaver.SaveTextureToPNG(chamInfo.portraitSprite.texture, Links.SavePortraitPath, Links.ChampionPortraitName(chamInfo.id));
                }

                // 로딩 이미지
                if (LoadingPathCheck)
                    GetImage(loadingFilePath, ref chamInfo.loadingSprite);
                else
                {
                    string loadingUrl = Links.ChampionLoadingURL(chamInfo.id);
                    yield return StartCoroutine(Loader.LoadSprite(loadingUrl, s => chamInfo.loadingSprite = s));

                    ImageSaver.SaveTextureToPNG(chamInfo.loadingSprite.texture, Links.SaveLoadingPath, Links.ChampionLoadingName(chamInfo.id));
                }

                // 일러스트
                if (splashPathCheck)
                    GetImage(splashFilePath, ref chamInfo.splashSprite);
                else
                {
                    string splashUrl = Links.ChampionSplashURL(chamInfo.id);
                    yield return StartCoroutine(Loader.LoadSprite(splashUrl, s => chamInfo.splashSprite = s));

                    ImageSaver.SaveTextureToPNG(chamInfo.splashSprite.texture, Links.SaveSplashPath, Links.ChampionSplashName(chamInfo.id));
                }
            }

            DataStore.ForceInstance.SetChampionData(m_championList);
        }

        /// <summary>
        /// 로컬 버전 정보를 가져옵니다
        /// </summary>
        /// <returns>로컬 버전 문자열</returns>
        private string GetLocalVersion()
        {
            if (File.Exists(filePath))
                return File.ReadAllText(filePath);
            return defaultVersion;
        }

        /// <summary>
        /// 로컬 버전 정보를 저장합니다
        /// </summary>
        /// <param name="version">저장할 버전 문자열</param>
        private void SaveLocalVersion(string version)
        {
            Directory.CreateDirectory(folderPath);
            File.WriteAllText(filePath, version);
        }

        /// <summary>
        /// JSON에서 버전 정보를 파싱합니다
        /// </summary>
        /// <param name="json">파싱할 JSON 문자열</param>
        private void GetStringByFromJson(string json)
        {
            var versions = JsonHelper.FromJson<string>(json);
            if (versions != null && versions.Length > 0)
                releasedVersion = versions[0]; // 최신 버전
        }

        /// <summary>
        /// JSON에서 챔피언 리스트를 파싱합니다
        /// </summary>
        /// <param name="json">파싱할 JSON 문자열</param>
        private void GetChampionList(string json)
        {
            m_championList = JsonConvert.DeserializeObject<ChampionListRoot>(json);
        }

        /// <summary>
        /// 이미지 파일을 스프라이트로 변환합니다
        /// </summary>
        /// <param name="path">이미지 파일 경로</param>
        /// <param name="s">변환된 스프라이트를 저장할 참조</param>
        private void GetImage(string path, ref Sprite s)
        {
            byte[] imageData = File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            s = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
        }
    }
}
