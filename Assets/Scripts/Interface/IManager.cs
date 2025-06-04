using UnityEngine;

namespace Scripts.Interface
{
    public interface IManager
    {
        int Priority { get; }       // 초기화 순서 지정
        bool IsDontDestroy { get; } // DontDestroy 체크
        void Initialize();          // 초기화
        void Cleanup();             // 해제/정리
        GameObject GetGameObject();  // 소속 GameObject 반환
    }
}

