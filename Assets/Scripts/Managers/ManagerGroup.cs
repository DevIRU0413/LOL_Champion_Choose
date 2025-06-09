using System.Collections.Generic;

using Scripts.Interface;
using Scripts.Util;

using UnityEngine;

namespace Scripts.Managers
{
    public class ManagerGroup : MonoBehaviour
    {
        #region Singleton
        private static ManagerGroup m_instance;
        public static ManagerGroup Instance
        {
            get
            {
                if (m_instance == null)
                {
                    string groupName = $"@{typeof(ManagerGroup).Name}";
                    GameObject go = GameObject.Find(groupName);
                    if (go == null)
                    {
                        go = new GameObject(groupName);
                        DontDestroyOnLoad(go);
                    }

                    m_instance = go.GetOrAddComponent<ManagerGroup>();
                }

                return m_instance;
            }
        }
        #endregion

        #region PrivateVariables
        private List<IManager> m_unregisteredManagers  = new(); // 미등록
        private List<IManager> m_registeredManagers = new(); // 등록됨

        private bool m_isManagersInitialized = false; // 초기화 중간 확인 및 매니저들 사용 여부 확인용
        #endregion

        #region PublicMethod

        public bool IsUseAble()
        {
            return m_isManagersInitialized;
        }

        public void RegisterManager(IManager manager)
        {
            if (manager == null || m_registeredManagers.Contains(manager) || m_unregisteredManagers.Contains(manager))
                return;

            foreach (var m in m_registeredManagers)
            {
                if (m.Equals(manager))
                    return;
            }

            foreach (var m in m_unregisteredManagers)
            {
                if (m.Equals(manager))
                    return;
            }

            m_unregisteredManagers.Add(manager);
        }

        public void RegisterManager(GameObject managerObject)
        {
            RegisterManager(managerObject?.GetComponent<IManager>());
        }

        public void RegisterManager(params IManager[] managers)
        {
            foreach (IManager m in managers) RegisterManager(m);
        }

        public void RegisterManager(params GameObject[] managerObjects)
        {
            foreach (GameObject go in managerObjects) RegisterManager(go);
        }

        public void InitializeManagers()
        {
            m_isManagersInitialized = false;
            SortManagersByPriorityAscending(m_unregisteredManagers);

            foreach (var manager in m_unregisteredManagers)
            {
                manager.Initialize();
                GameObject goM = manager.GetGameObject();
                if (goM == null)
                {
                    Debug.LogError($"[Dnot Init] {goM.name} !!!");
                    continue;
                }

                Debug.Log($"[Init] {goM.name}");
                m_registeredManagers.Add(manager);
                goM.transform.parent = transform;
            }

            m_unregisteredManagers.Clear();
            m_isManagersInitialized = true;
        }

        /// <summary>
        /// 매니저들 내부 데이터 종료 처리 밎 정리
        /// </summary>
        public void CleanupManagers()
        {
            for (int i = 0; i < m_registeredManagers.Count; i++)
            {
                IManager manager = m_registeredManagers[i];
                GameObject go = manager.GetGameObject();

                if (go == null)
                {
                    m_registeredManagers.Remove(manager);
                    continue;
                }

                manager.Cleanup();
                Debug.Log($"[Cleanup] {go.name}");
            }
        }

        /// <summary>
        /// 지속적인 생존이 필요하지 않은 매니저 정리
        /// </summary>
        /// <param name="forceClear">강제 정리 여부</param>
        public void ClearManagers(bool forceClear = false)
        {
            for (int i = 0; i < m_registeredManagers.Count; i++)
            {
                IManager manager = m_registeredManagers[i];
                if (!manager.IsDontDestroy || forceClear)
                {
                    GameObject go = manager.GetGameObject();

                    if (go == null)
                    {
                        m_registeredManagers.Remove(manager);
                        continue;
                    }

                    manager.Cleanup();
                    string name = go.name;
                    Destroy(go);
                    Debug.Log($"[Clear] {name}");
                }
            }
        }

        public void ClearAllManagers()
        {
            ClearManagers(true);
        }
        #endregion

        #region PrivateMethod

        private void SortManagersByPriorityAscending(List<IManager> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (list[j].Priority > list[j + 1].Priority)
                    {
                        IManager temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        private void SortManagersByPriorityDescending(List<IManager> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (list[j].Priority < list[j + 1].Priority)
                    {
                        IManager temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        #endregion
    }
}
