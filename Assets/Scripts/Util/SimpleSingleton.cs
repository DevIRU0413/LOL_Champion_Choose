using UnityEngine;

namespace Scripts.Util
{
    public abstract class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        [SerializeField] private bool m_isDontDestroyOnLoad = true; // true면 유지, false면 씬 전환 시 파괴

        protected bool IsDontDestroyOnLoad => m_isDontDestroyOnLoad;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<T>();
                    if (m_instance == null)
                        Debug.LogError($"[SimpleSingleton] {typeof(T)} 인스턴스가 이 씬에 없습니다.");
                }
                return m_instance;
            }
        }

        public static T ForceInstance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<T>();
                    if (m_instance == null)
                    {
                        Debug.LogWarning($"[SimpleSingleton] {typeof(T)} 인스턴스가 이 씬에 없습니다.\n" +
                            $"강제로 생성됩니다. > @{typeof(T)}");
                        GameObject go = new GameObject($"@{typeof(T)}");
                        m_instance = go.AddComponent<T>();

                        DontDestroyOnLoad(go);
                    }
                }
                return m_instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
                if (m_isDontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
            else if (m_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_instance == this)
            {
                m_instance = null;
            }
        }
    }
}
