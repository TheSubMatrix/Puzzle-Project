using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Board Kernel", menuName = "Scriptable Objects/Board Kernel")]
public class BoardKernelSO : ScriptableObject
{
    [Serializable]
    enum CellState
    {
        Ignore,
        Match,
        Result
    }
    [SerializeField]SerializableGrid<CellState> m_board;
}
