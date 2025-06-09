using Scripts.Managers;
using Scripts.Scene;
using Scripts.Util;

using UnityEngine;

public class DataLoadingScene : SceneBase
{
    [SerializeField] private RectTransform m_loadingUIRT;
    [SerializeField] private RectTransform m_loadingBarRT;

    [SerializeField] private float m_loadProgressSpeed = 1.0f;
    private float m_loadProgress = 0.0f;

    public override SceneID SceneID => SceneID.DataLoadingScene;

    protected override void Initialize()
    {
        DataStoreManager.Instance.SetupData();
        SceneManagerEx.Instance.LoadSceneAsync(SceneID.MainTitleScene);
    }

    private void Update()
    {
        if (m_loadingUIRT != null)
            m_loadingUIRT.gameObject.SetActive(true);
        if (m_loadingBarRT != null)
        {
            Debug.Log(SceneManagerEx.Instance.LoadProgress);
            m_loadProgress = Mathf.MoveTowards(m_loadProgress, SceneManagerEx.Instance.LoadProgress, m_loadProgressSpeed * Time.deltaTime);

            m_loadingBarRT.localScale = new(m_loadProgress, 1f, 1f);
        }        
    }
}
