using System.Collections.Generic;
using System.Linq;

using Scripts.Interface;
using Scripts.Util;

using UnityEngine;

namespace Scripts.Managers
{
    public class UIManager : SimpleSingleton<UIManager>, IManager
    {
        private const int FRONT_HUD_UI_ORDERLAYER_VALUE = 100;
        private const int BACK_HUD_UI_ORDERLAYER_VALUE = 0;

        private const int NORMAL_UI_MIN_ORDERLAYER_VALUE = 1;
        private const int NORMAL_UI_MAX_ORDERLAYER_VALUE = 99;

        // MainCamera
        private Camera m_mainCam;

        // Prefab
        [SerializeField] private GameObject m_frontHUD;
        [SerializeField] private GameObject m_backHUD;

        // HUD(front 100, back 0)
        private Canvas m_canvasFrontHUD;
        private Canvas m_canvasBackHUD;

        // Scene(1 ~ 99)
        private Dictionary<int, Canvas> m_sceneCanvasDictionary = new Dictionary<int, Canvas>();
        private int m_sceneCanvasTopOrder = 0;

        // Scene(101 ~ 200)
        private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

        public int Priority => (int)ManagerPriority.UIManager;

        public bool IsDontDestroy => IsDontDestroyOnLoad;


        public void Initialize()
        {
            SetupUI(m_frontHUD, FRONT_HUD_UI_ORDERLAYER_VALUE);
            SetupUI(m_backHUD, BACK_HUD_UI_ORDERLAYER_VALUE);
        }

        public void Cleanup()
        {
            CleanUI();
            return;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void SetupUI(GameObject uiPrefab, int sortingOrder = -1)
        {
            if (uiPrefab.GetComponent<Canvas>() == null) return;
            if (sortingOrder == FRONT_HUD_UI_ORDERLAYER_VALUE && m_canvasFrontHUD != null) return;
            if (sortingOrder == BACK_HUD_UI_ORDERLAYER_VALUE && m_canvasBackHUD != null) return;

            var go =  GameObject.Instantiate(uiPrefab, this.gameObject.transform);
            var canvas = go?.GetComponent<Canvas>();

            canvas.worldCamera = Camera.main;

            if (canvas.sortingOrder == FRONT_HUD_UI_ORDERLAYER_VALUE)
            {
                m_canvasFrontHUD = canvas;
                sortingOrder = FRONT_HUD_UI_ORDERLAYER_VALUE;
            }
            else if (canvas.sortingOrder == BACK_HUD_UI_ORDERLAYER_VALUE)
            {
                m_canvasBackHUD = canvas;
                sortingOrder = BACK_HUD_UI_ORDERLAYER_VALUE;
            }
            else if (m_sceneCanvasTopOrder < NORMAL_UI_MAX_ORDERLAYER_VALUE)
            {
                m_sceneCanvasTopOrder++;
                m_sceneCanvasDictionary.Add(m_sceneCanvasTopOrder, canvas);
                sortingOrder = m_sceneCanvasTopOrder;
            }
            else
            {
            }

            canvas.sortingOrder = sortingOrder;
        }

        private void CleanUI()
        {
            while (m_sceneCanvasTopOrder > 0)
            {
                var go = m_sceneCanvasDictionary[m_sceneCanvasTopOrder].gameObject;
                Destroy(go);
                m_sceneCanvasTopOrder--;
            }

            m_sceneCanvasDictionary.Clear();
            m_sceneCanvasTopOrder = 0;
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

        public void SetupAllCanvas()
        {
            SetupCanvasCamera(m_sceneCanvasDictionary.Values.ToArray());
            SetupCanvasCamera(m_canvasFrontHUD, m_canvasBackHUD);
        }
        private void SetupCanvasCamera(params Canvas[] canvasArr)
        {
            if (canvasArr == null || canvasArr.Length == 0) return;

            var camGo = GameObject.FindGameObjectWithTag("MainCamera");
            m_mainCam = camGo.GetComponent<Camera>();

            if (m_mainCam == null) return;

            foreach(var canvas in canvasArr)
            {
                if (canvas == null) continue;

                canvas.worldCamera = m_mainCam;
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

    }
}
