using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Scriptable Objects/Solution", fileName = "Solution", order = 0)]
public class SolutionSO : ScriptableObject
{
    [field:SerializeField]public SerializableGrid<PuzzlePiece> Grid{get; private set;}
    [field:SerializeField]public Vector2Int MissingPiecePosition{get; private set;}
    [field:SerializeField]public PuzzlePiece MissingPiece{get; private set;}
}
