using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class Roulette : MonoBehaviour
{
    [SerializeField] private Transform piecePrefab;
    [SerializeField] private Transform linePrefab;

    [SerializeField] private Transform pieceParent;
    [SerializeField] private Transform lineParent;

    [SerializeField] private RoulettePieceData[] roulettePieceData;

    [SerializeField] private int spinDuration;
    [SerializeField] private Transform spinningRoulette;
    [SerializeField] private AnimationCurve spinningCurve;

    private float pieceAngle;
    private float halfPieceAngle;
    private float halfPieceAngleWithPaddings;

    private float accumulatedWeight;

    private bool isSpinning = false;
    private int selectedIndex = 0;

    private void Awake()
    {
        pieceAngle = 360 / roulettePieceData.Length;
        halfPieceAngle = pieceAngle * 0.5f;
        halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle * 0.25f);

        SpawnPiecesAndLines();
        CalculateWeightAndIndices();
        // GetRandomIndex();
    }

    private void SpawnPiecesAndLines()
    {
        for (int i = 0; i < roulettePieceData.Length; ++i)
        {
            Transform piece = Instantiate(piecePrefab, pieceParent.position, Quaternion.identity, pieceParent);
            piece.GetComponent<RoulettePiece>().Setup(roulettePieceData[i]);
            piece.RotateAround(pieceParent.position, Vector3.back, (pieceAngle * i));

            Transform line = Instantiate(linePrefab, lineParent.position, Quaternion.identity, lineParent);
            line.RotateAround(lineParent.position, Vector3.back, (pieceAngle * i) + halfPieceAngle);
        }
    }

    private void CalculateWeightAndIndices()
    {
        for (int i = 0; i < roulettePieceData.Length; ++i)
        {
            if (roulettePieceData[i].chane <= 0)
            {
                roulettePieceData[i].chane = 1;
            }

            accumulatedWeight += roulettePieceData[i].chane;
            roulettePieceData[i].weight = accumulatedWeight;
        }
    }

    private int GetRandomIndex()
    {
        float weight = Random.Range(0.0f, accumulatedWeight);
        Debug.Log($"weight: {weight}");
        for(int i = 0; i < roulettePieceData.Length; i++)
        {
            if (roulettePieceData[i].weight > weight)
            {
                return i;
            }
        }

        return 0;
    }

    public void Spin(UnityAction<RoulettePieceData> action = null)
    {
        if (isSpinning) return;

        selectedIndex = GetRandomIndex();

        float angle = pieceAngle * selectedIndex;

        float leftOffset = (angle - halfPieceAngleWithPaddings) % 360;
        float rightOffset = (angle + halfPieceAngleWithPaddings) % 360;
        float randomAngle = Random.Range(leftOffset, rightOffset);

        int rotateSpeed = 2;
        float targetAngle = (randomAngle + 360 * spinDuration * rotateSpeed);

        isSpinning = true;
        StartCoroutine(OnSpin(targetAngle, action));
    }

    private IEnumerator OnSpin(float end, UnityAction<RoulettePieceData> action)
    {
        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / spinDuration;

            float z = Mathf.Lerp(0, end, spinningCurve.Evaluate(percent));
            spinningRoulette.rotation = Quaternion.Euler(0,0,z);
            yield return null;
        }

        isSpinning = false;

        if (action != null) action.Invoke(roulettePieceData[selectedIndex]);
    }
}
