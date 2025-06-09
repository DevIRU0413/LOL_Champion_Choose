using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Scripts.Interface;
using Scripts.Util;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Managers
{
    public class SceneManagerEx : SimpleSingleton<SceneManagerEx>, IManager
    {
        #region PrivateVariables
        private SceneID m_currentScene = SceneID.None; // 현재 씬

        private float m_loadProgress = 0.0f;

        private string[] m_sceneNames;
        #endregion

        #region PublicVariables
        public event Action<string> OnSceneLoaded;

        public Dictionary<Func<IEnumerator>, string> OnSceneBeforeChange;

        public int Priority => (int)ManagerPriority.SceneManagerEx;
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        public float LoadProgress => m_loadProgress;
        #endregion

        #region PublicMethod
        public void Initialize()
        {
            OnSceneBeforeChange = new();
            m_sceneNames = Enum.GetNames(typeof(SceneID));
        }

        public void Cleanup() { }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public string GetCurrentSceneName()
        {
            return m_currentScene.ToString();
        }

        public void LoadSceneAsync(SceneID id)
        {
            string next;

            if (ContainSceneID(id, out next))
            {
                LoadSceneAsync(next);
            }
            else
                Debug.LogError($"[{id}] is Have Not Scene");
        }

        public void UnloadSceneAsync(SceneID id)
        {
            string name;
            if (ContainSceneID(id, out name))
                UnloadSceneAsync(name);
            else
                Debug.LogError($"[{id}] is Have Not Scene");
        }


        #endregion

        #region PrivateMethod
        private bool ContainSceneID(SceneID sceneID, out string outSceneName)
        {
            string sceneName = sceneID.ToString();
            bool isContains = m_sceneNames.Contains(sceneName);
            outSceneName = sceneName;

            return isContains;
        }

        private bool ContainSceneName(string sceneName, out string outSceneName)
        {
            outSceneName = string.Empty;
            if (sceneName == null) return false;

            bool isContains = m_sceneNames.Contains(sceneName);
            outSceneName = sceneName;
            return isContains;
        }

        private void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(IE_LoadSceneAsync(sceneName));
        }

        public void UnloadSceneAsync(string sceneName)
        {
            StartCoroutine(IE_UnloadScene(sceneName));
        }

        private IEnumerator IE_LoadScene(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
        }

        private IEnumerator IE_LoadSceneAsync(string sceneName)
        {
            // 로딩 씬 전환
            string load = string.Empty;
            if (ContainSceneID(SceneID.LoadingScene, out load))
            {
                yield return StartCoroutine(IE_LoadScene(load));
            }

            // 전환 후, 필요 데이터 및 사전 처리 진행
            m_loadProgress = 0.0f;
            if (OnSceneBeforeChange != null)
            {
                foreach (var fuc in OnSceneBeforeChange)
                {
                    Debug.Log($"[{sceneName}]: {fuc.Value}");
                    yield return StartCoroutine(fuc.Key());
                    m_loadProgress += 0.7f / OnSceneBeforeChange.Count;
                    yield return null;
                }
            }

            // 지정 씬으로 전환 시작
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            // 씬 전환 시 해당 씬에 필요한 데이터들 및 구성 요소 로딩
            while (operation.progress < 0.9f)
            {
                m_loadProgress = 0.7f + (operation.progress / 0.9f) * 0.2f;
                yield return null;
            }

            // 약간의 딜레이
            yield return new WaitForSeconds(0.5f);
            m_loadProgress = 1.0f;

            // 전환 허용
            operation.allowSceneActivation = true;

            // 전환 완료까지 대기
            yield return new WaitUntil(() => operation.isDone);

            // 마지막 추후 처리
            OnSceneLoaded?.Invoke(sceneName);
        }

        private IEnumerator IE_UnloadScene(string sceneName)
        {
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
        }


        #endregion
    }
}
