using System.Collections.Generic;
using System.Linq;

using Scripts.Data;
using Scripts.Interface;
using Scripts.Util;

using UnityEngine;

namespace Scripts.Managers
{
    // DB같은 느낌으로 사용해야지
    public class DataStoreManager : SimpleSingleton<DataStoreManager>, IManager
    {
        private bool m_isDataReady = false;

        private AppInitializer m_appInitializer;

        private ChampionListRoot m_champions;
        private UserSettingData m_userSettings;

        public int Priority => (int)ManagerPriority.DataStore;
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        public void Initialize()
        {
            if (m_appInitializer == null)
            {
                m_appInitializer = this.gameObject.GetOrAddComponent<AppInitializer>();
                m_appInitializer.Initialize();
            }

            if (m_champions == null || m_userSettings == null)
            {
                if (SceneManagerEx.Instance.GetCurrentSceneName() != SceneID.DataLoadingScene.ToString())
                    SceneManagerEx.Instance.LoadSceneAsync(SceneID.DataLoadingScene);
            }
        }

        public void Cleanup()
        {
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public void SetupData()
        {
            SceneManagerEx.Instance.OnSceneBeforeChange.Add(m_appInitializer.InitializeVersion, "버전 확인");
            SceneManagerEx.Instance.OnSceneBeforeChange.Add(m_appInitializer.InitializeChampionDatas, "챔피언 데이터들 로드");
            SceneManagerEx.Instance.OnSceneBeforeChange.Add(m_appInitializer.InitializeChampionFormatSetting, "챔피언 이디지 데이터 로드");
        }

        #region Champion Data
        public void SetChampionData(ChampionListRoot championDatas, bool isForceInputData = false)
        {
            // 챔피언 데이터가 있으면서, 강제 넣기가 아닐때만 리틴
            if (m_champions != null && !isForceInputData)
                return;

            m_champions = championDatas;
        }

        public ChampionListRoot GetOrignChampionAllData()
        {
            return m_champions;
        }

        public List<ChampionData> GetListChampionAllData()
        {
            var chamList = m_champions.data.Values.ToList();
            return chamList;
        }

        #endregion

        #region User Data
        public void SetUserData(UserSettingData userData)
        {
            m_userSettings = userData;
        }

        public UserSettingData GetUserData() { return m_userSettings; }


        #endregion


    }
}
