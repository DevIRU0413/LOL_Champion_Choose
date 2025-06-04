using System.Collections.Generic;

using Scripts.Interface;
using Scripts.Util;

using UnityEngine;

namespace Scripts.Managers
{
    public class UIManager : SimpleSingleton<UIManager>, IManager
    {
        private Canvas m_sceneCanvas;

        // 패널들을 이름으로 관리
        private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

        public int Priority => (int)ManagerPriority.UIManager;

        public bool IsDontDestroy => IsDontDestroyOnLoad;


        public void Initialize()
        {
            GetFindCanvas();
        }

        public void Cleanup()
        {
            return;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }


        /// <summary>
        /// UI 패널을 등록
        /// </summary>
        public void RegisterPanel(string name, GameObject panel)
        {
            if (!panels.ContainsKey(name))
            {
                panels.Add(name, panel);
                panel.SetActive(false); // 기본은 비활성화
            }
        }

        /// <summary>
        /// UI 패널 열기
        /// </summary>
        public void OpenPanel(string name)
        {
            if (panels.TryGetValue(name, out var panel))
            {
                panel.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"[UIManager] 패널 '{name}' 이(가) 등록되어 있지 않습니다.");
            }
        }

        /// <summary>
        /// UI 패널 닫기
        /// </summary>
        public void ClosePanel(string name)
        {
            if (panels.TryGetValue(name, out var panel))
            {
                panel.SetActive(false);
            }
        }

        /// <summary>
        /// UI 패널 토글
        /// </summary>
        public void TogglePanel(string name)
        {
            if (panels.TryGetValue(name, out var panel))
            {
                panel.SetActive(!panel.activeSelf);
            }
        }

        private Canvas GetFindCanvas()
        {
            var go = GameObject.FindGameObjectWithTag("Canvas");
            var cmp = go?.GetComponent<Canvas>();
            if (cmp != null)
                m_sceneCanvas = cmp;

            return m_sceneCanvas;
        }

    }
}
