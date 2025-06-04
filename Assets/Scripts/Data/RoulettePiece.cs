
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class RoulettePiece : MonoBehaviour
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private TextMeshProUGUI textDescription;

    public void Setup(RoulettePieceData pieceData)
    {
        imageIcon.sprite = pieceData.data.portraitSprite;
        textDescription.text = pieceData.data.name;
    }
}
