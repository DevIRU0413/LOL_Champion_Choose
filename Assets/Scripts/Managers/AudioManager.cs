using System.Collections.Generic;
using System.Linq;

using Scripts.Interface;
using Scripts.Util;

using UnityEngine;

namespace Scripts.Managers
{
    public class AudioManager : SimpleSingleton<AudioManager>, IManager
    {
        #region Constants
        private const string BGM_GAMEOBJECT_NAME = "BGM_Audio";
        private const string SFX_GAMEOBJECT_NAME = "SFX_Audio";

        private const float SOUND_MIN_VOLUME = 0.0f;
        private const float SOUND_MAX_VOLUME = 1.0f;

        private const int SFX_POOL_SIZE = 5;
        #endregion

        #region PrivateVariables
        [SerializeField] private AudioClip m_defaultBGM;
        private GameObject m_audioManager;
        private AudioSource m_bgmSource;
        private List<AudioSource> m_sfxSources = new();

        private float m_sfxVolume = 1.0f;
        #endregion

        #region PublicVariables
        public int Priority => (int)ManagerPriority.AudioManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        #endregion

        #region PublicMethod
        public void Initialize()
        {
            m_audioManager = gameObject;

            CreateBgm();
            CreateSfxPool();
        }

        public void Cleanup() { }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void PlayBgm(AudioClip clip)
        {
            if (clip == null)
                return;

            m_bgmSource.clip = clip;
            m_bgmSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            var source = m_sfxSources.FirstOrDefault(s => !s.isPlaying);
            if (source != null && clip != null)
            {
                source.clip = clip;
                source.Play();
            }
        }

        public void StopBgm()
        {
            m_bgmSource.Stop();
        }

        public void StopSfx()
        {
            foreach (var sfx in m_sfxSources)
                sfx.Stop();
        }

        public void SetBgmVolume(float volume)
        {
            volume = Mathf.Clamp(volume, SOUND_MIN_VOLUME, SOUND_MAX_VOLUME);
            m_bgmSource.volume = volume;
        }

        public void SetSfxVolume(float volume)
        {
            volume = Mathf.Clamp(volume, SOUND_MIN_VOLUME, SOUND_MAX_VOLUME);
            m_sfxVolume = volume;
            foreach (var sfx in m_sfxSources)
                sfx.volume = m_sfxVolume;
        }

        public float GetBgmVolume()
        {
            return m_bgmSource.volume;
        }
        public float GetSfxVolume()
        {
            return m_sfxVolume;
        }

        public void MuteBgm()
        {
            m_bgmSource.volume = SOUND_MIN_VOLUME;
        }

        public void MuteSfx()
        {
            foreach (var sfx in m_sfxSources)
                sfx.volume = SOUND_MIN_VOLUME;
            m_sfxVolume = SOUND_MIN_VOLUME;
        }
        #endregion

        #region PrivateMethod
        private void CreateBgm()
        {
            if (m_bgmSource != null) return;

            GameObject bgm = UnityUtilEx.FindOrCreateGameObject(BGM_GAMEOBJECT_NAME);
            bgm.transform.SetParent(m_audioManager.transform);
            DontDestroyOnLoad(bgm);

            m_bgmSource = bgm.AddComponent<AudioSource>();
            m_bgmSource.loop = true;
            m_bgmSource.volume = 0.5f;

            PlayBgm(m_defaultBGM);
        }

        private void CreateSfxPool()
        {
            if(m_sfxSources.Count > 0) return;

            for (int i = 0; i < SFX_POOL_SIZE; i++)
            {
                GameObject sfx = UnityUtilEx.FindOrCreateGameObject($"{SFX_GAMEOBJECT_NAME}_{i}");
                sfx.transform.SetParent(m_audioManager.transform);
                DontDestroyOnLoad(sfx);

                AudioSource cmp = sfx.AddComponent<AudioSource>();
                cmp.loop = false;
                cmp.volume = 0.5f;

                m_sfxSources.Add(cmp);
            }

            m_sfxVolume = 0.5f;
        }
        #endregion
    }
}
