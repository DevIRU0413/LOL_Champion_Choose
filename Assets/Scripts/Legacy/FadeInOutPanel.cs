using System.Collections;

using Ricimi;

using UnityEngine;

namespace Scripts.Legacy
{
    public class FadeInOutPanel : Popup
    {
        [Header("Fade Settings")]
        [SerializeField] private CanvasGroup m_fadeCanvasGroup;
        [SerializeField] private float m_fadeDuration = 1f;

        private IEnumerator IE_FadeOut()
        {
            if (m_fadeCanvasGroup == null) yield break;

            m_fadeCanvasGroup.blocksRaycasts = true;
            float time = 0f;

            while (time < m_fadeDuration)
            {
                time += Time.deltaTime;
                m_fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, time / m_fadeDuration);
                yield return null;
            }
        }

        private IEnumerator IE_FadeIn()
        {
            if (m_fadeCanvasGroup == null) yield break;

            float time = 0f;

            while (time < m_fadeDuration)
            {
                time += Time.deltaTime;
                m_fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, time / m_fadeDuration);
                yield return null;
            }

            m_fadeCanvasGroup.blocksRaycasts = false;
        }
    }
}
