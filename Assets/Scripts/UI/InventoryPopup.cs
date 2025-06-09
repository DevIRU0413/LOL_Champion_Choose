using System.Collections.Generic;

using Ricimi;

using Scripts.Data;
using Scripts.Managers;

using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{

    public class InventoryPopup : Popup
    {
        [SerializeField] private string m_backgroundImageTagName; // 배경 옵젝 이름
        [SerializeField] private GameObject m_inventoryContent; // 인벤 콘텐트
        [SerializeField] private GameObject m_inventoryItem; // 프리펩으로 사용할 옵젝

        private Image m_backgroundImage; // 배경으로 사용되는 옵젝
        private RectTransform m_inventoryContentRT; // 인벤 콘텐트 RT
        private List<GameObject> m_inventoryItemGos;
        private List<ChampionData> m_datas;

        private void Awake()
        {
            m_datas = DataStoreManager.Instance.GetListChampionAllData();
            m_inventoryContentRT = m_inventoryContent.GetComponent<RectTransform>();

            var bgGo = GameObject.FindGameObjectWithTag(m_backgroundImageTagName);
            m_backgroundImage = bgGo.GetComponent<Image>();
        }

        private void Start()
        {
            m_inventoryItemGos = new List<GameObject>();

            for (int i = 0; i < m_datas.Count; i++)
            {
                // Item Prefab으로 인스턴스 생성
                var dataGo = GameObject.Instantiate(m_inventoryItem);

                // 생성된 옵젝에게, ItemData 넣어줌
                var itemCmp =dataGo.AddComponent<BGItemData>();
                itemCmp.Setting(m_backgroundImage, m_inventoryContentRT, m_datas[i]);

                // 생성된 자식 리스트에 넣어줌
                m_inventoryItemGos.Add(dataGo);
            }
        }
    }
}
