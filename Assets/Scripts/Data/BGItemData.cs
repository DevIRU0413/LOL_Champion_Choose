using Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

public class BGItemData : MonoBehaviour
{
    private readonly string INVENTORY_ITEM_IMAGE_NAME = "Icon";

    public ChampionData data ;
    private Image m_targetImage;

    public void Setting(Image targetImage, Transform parentTr, ChampionData data)
    {
        m_targetImage = targetImage;
        this.gameObject.transform.parent = parentTr;
        this.data = data;

        // 현재 자신 기본 크기로 설정
        gameObject.transform.localScale = Vector3.one;

        // 자식들 가지고 와서 이이지 적용
        var dataGoChilds = gameObject.GetComponentsInChildren<Transform>();
        foreach (var childGo in dataGoChilds)
        {
            if (childGo.name == INVENTORY_ITEM_IMAGE_NAME)
            {
                Image image = childGo.GetComponent<Image>();

                image.sprite = data.portraitSprite;
                break;
            }
        }

        // 이벤트 연결
        var btnCmp = gameObject.GetComponent<Button>();
        btnCmp.onClick.AddListener(ChangeBackGround);
    }

    public void ChangeBackGround()
    {
        m_targetImage.sprite = data.splashSprite;
    }
}
