using System.Collections.Generic;
using System.Linq;

using Scripts.Data;
using Scripts.Util;

// DB같은 느낌으로 사용해야지
public class DataStore : SimpleSingleton<DataStore>
{
    private ChampionListRoot m_champions;
    private UserSettingData m_userSettings;

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
