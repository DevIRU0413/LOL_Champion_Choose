using System;
using System.Collections;
using System.Collections.Generic;

using Scripts.Interface;
using Scripts.Util;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Managers
{
    public class SceneManagerEx : SimpleSingleton<SceneManagerEx>, IManager
    {
        #region PrivateVariables
        private float m_loadProgress = 0.0f;
        #endregion
        #region PublicVariables
        public event Action<string> OnSceneLoaded;

        public List<Func<IEnumerator>> OnSceneBeforeChange;
        public List<Func<IEnumerator>> OnSceneAfterChange;

        public int Priority => (int)ManagerPriority.SceneManagerEx;
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        public float LoadProgress => m_loadProgress;
        #endregion

        #region PublicMethod
        public void Initialize()
        {
            OnSceneBeforeChange = new();
            OnSceneAfterChange = new();
        }

        public void Cleanup() { }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(IE_LoadScene(sceneName));
        }

        public void UnloadSceneAsync(string sceneName)
        {
            StartCoroutine(IE_UnloadScene(sceneName));
        }
        #endregion

        #region PrivateMethod
        private IEnumerator IE_LoadScene(string sceneName)
        {
            m_loadProgress = 0.0f;

            if (OnSceneBeforeChange != null)
            {
                foreach (Func<IEnumerator> coroutineFunc in OnSceneBeforeChange)
                {
                    yield return StartCoroutine(coroutineFunc());
                    m_loadProgress += 0.7f / OnSceneBeforeChange.Count;
                    yield return null;
                }
            }

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                m_loadProgress = 0.7f + (operation.progress / 0.9f) * 0.3f;
                yield return null;
            }

            // 로딩 끝난 척 약간의 연출 시간 주고 씬 전환
            m_loadProgress = 1.0f;
            yield return new WaitForSeconds(0.5f);
            operation.allowSceneActivation = true;

            yield return new WaitUntil(() => operation.isDone);

            OnSceneLoaded?.Invoke(sceneName);

            if (OnSceneAfterChange != null)
            {
                foreach (Func<IEnumerator> coroutineFunc in OnSceneAfterChange)
                {
                    yield return StartCoroutine(coroutineFunc());
                }
            }
        }


        private IEnumerator IE_UnloadScene(string sceneName)
        {
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
        }
        #endregion
    }
}
