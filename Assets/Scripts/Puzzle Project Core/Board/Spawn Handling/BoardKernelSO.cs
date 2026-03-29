using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[CreateAssetMenu(fileName = "Board Kernel", menuName = "Scriptable Objects/Board Kernel")]
public class BoardKernelSO : BoardKernelSOBase<CellState>
{
    [SerializeField] SerializableGrid<CellState> m_board;
    public override SerializableGrid<CellState> Board => m_board;
    
    Vector2Int? m_anchor;
    public override Vector2Int Anchor
    {
        get
        {
            if (m_anchor.HasValue) return m_anchor.Value;
            
            return CacheAnchor() && m_anchor is not null ? m_anchor.Value : throw new InvalidOperationException("No anchor found");
        }
    }

    void OnValidate() => m_anchor = null;
    
    bool CacheAnchor()
    {
        for (int r = 0; r < m_board.Rows; r++)
        for (int c = 0; c < m_board.Columns; c++)
            if (m_board.Value[r, c] == CellState.Result)
            {
                m_anchor = new(c, r);
                return true;
            }
        return false;
    }
}