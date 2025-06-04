using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{

    [SerializeField] private Roulette roulette;
    [SerializeField] Button btnSpin;

    private void Awake()
    {
        btnSpin.onClick.AddListener(() =>
        {
            btnSpin.interactable =false;
            roulette.Spin(EndOfSpin);

        });
    }

    private void EndOfSpin(RoulettePieceData seletedData)
    {
        btnSpin.interactable = true;
        Debug.Log($"{seletedData.data.name}");
    }
}
