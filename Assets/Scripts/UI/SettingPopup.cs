using Ricimi;

using Scripts.Managers;

using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class SettingPopup : Popup
    {
        [Header("Sound")]
        [SerializeField] private Slider m_bgmSlider;
        [SerializeField] private Slider m_sfxSlider;
        [SerializeField] private Slider m_muteSlider;

        private bool m_isChanging = false; // 슬라이더 상호작용 방지 플래그

        public void Start()
        {
            m_bgmSlider.value = AudioManager.Instance.GetBgmVolume();
            m_sfxSlider.value = AudioManager.Instance.GetSfxVolume();
            m_muteSlider.value = (IsMuted()) ? 1.0f : 0.0f;

            m_bgmSlider?.onValueChanged.AddListener(OnBGM);
            m_sfxSlider?.onValueChanged.AddListener(OnSFX);
            m_muteSlider?.onValueChanged.AddListener(OnMute);
        }

        private bool IsMuted()
        {
            return AudioManager.Instance.GetBgmVolume() <= 0.01f &&
                   AudioManager.Instance.GetSfxVolume() <= 0.01f;
        }

        public void OnBGM(float value)
        {
            if (m_isChanging) return;

            m_isChanging = true;

            AudioManager.Instance.SetBgmVolume(value);

            if (value > 0 && m_muteSlider.value == 1f)
                m_muteSlider.value = 0f;

            m_isChanging = false;
        }

        public void OnSFX(float value)
        {
            if (m_isChanging) return;

            m_isChanging = true;

            AudioManager.Instance.SetSfxVolume(value);

            if (value > 0 && m_muteSlider.value == 1f)
                m_muteSlider.value = 0f;

            m_isChanging = false;
        }

        public void OnMute(float value)
        {
            if (m_isChanging) return;

            m_isChanging = true;

            if (value == 1)
            {
                m_bgmSlider.value = 0;
                m_sfxSlider.value = 0;
            }
            else
            {
                if (m_bgmSlider.value <= 0.01f && m_sfxSlider.value <= 0.01f)
                {
                    m_bgmSlider.value = 0.5f;
                    m_sfxSlider.value = 0.5f;
                }
            }

            AudioManager.Instance.SetSfxVolume(m_bgmSlider.value);
            AudioManager.Instance.SetBgmVolume(m_sfxSlider.value);

            m_isChanging = false;
        }
    }

}
