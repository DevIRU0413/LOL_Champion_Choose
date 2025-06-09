using System;
using System.Linq;

using Scripts.Managers;
using Scripts.Tool;
using Scripts.Util;

using UnityEngine;

namespace Scripts.Scene
{
    public abstract class SceneBase : MonoBehaviour
    {
        // 현재 씬의 고유 ID (상속 클래스에서 지정)
        public abstract SceneID SceneID { get; }

        [SerializeField] private GameObject[] m_uiPrefabs;

        protected void Awake()
        {
            LoadManagers();
            LoadUI();
            Initialize();
        }

        protected abstract void Initialize();

        private void LoadUI()
        {
            foreach (GameObject go in m_uiPrefabs)
                UIManager.Instance.SetupUI(go);

            UIManager.Instance.SetupAllCanvas();
        }
        
        // 매니저 등록 및 초기화
        private void LoadManagers()
        {
            Debug.Log($"Scene {SceneID} Load Managers.");

            // 현재 씬에 존재하는 Manager 태그 오브젝트 수집
            GameObject[] SubscribeManagers = GameObject.FindGameObjectsWithTag("Manager");

            // 기존에 불필요한 매니저 제거
            ManagerGroup.Instance.ClearManagers();

            // 남아 있는 매니저 클린업 (종료 처리 등)
            ManagerGroup.Instance.CleanupManagers();

            // 새로 찾은 매니저 오브젝트들을 등록
            ManagerGroup.Instance.RegisterManager(SubscribeManagers);

            // 등록된 매니저들을 초기화
            ManagerGroup.Instance.InitializeManagers();
        }
    }
}
