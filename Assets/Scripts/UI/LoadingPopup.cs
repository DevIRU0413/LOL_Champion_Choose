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
                m_loadingBarRT.localScale = new(SceneManagerEx.Instance.LoadProgress, 1f, 1f);
            }
        }
    }
}
