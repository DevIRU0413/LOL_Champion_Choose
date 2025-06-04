using System;

using Scripts.Data;

using UnityEngine;

[Serializable]
public class RoulettePieceData
{
    public ChampionData data;

    [Range(1.0f, 100.0f)]
    public float chane;

    [HideInInspector]
    public float weight;
}
