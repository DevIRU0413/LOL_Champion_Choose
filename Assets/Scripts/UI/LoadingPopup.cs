using Ricimi;

using Scripts.Managers;

using UnityEngine;

namespace Scripts.UI
{
    public class LoadingPopup : Popup
    {
        [Header("Loading UI")]
        [SerializeField] private GameObject m_loadingUI;
        [SerializeField] private GameObject m_loadingBar;

        private RectTransform m_loadingUIRT;
        private RectTransform m_loadingBarRT;

        private float loadProgress = 0.0f;
        private float loadProgressSpeed = 1.0f;

        private void Start()
        {
            m_loadingUIRT = m_loadingUI.GetComponent<RectTransform>();
            m_loadingBarRT = m_loadingBar.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (m_loadingUIRT != null)
                m_loadingUIRT.gameObject.SetActive(true);
            if (m_loadingBarRT != null)
            {
                Debug.Log(SceneManagerEx.Instance.LoadProgress);
                loadProgress = Mathf.MoveTowards(loadProgress, SceneManagerEx.Instance.LoadProgress, loadProgressSpeed * Time.deltaTime);

                m_loadingBarRT.localScale = new(loadProgress, 1f, 1f);
            }
        }
    }
}
