using System;
using System.Collections.Generic;
using UnityEngine;

public class CellStateKernelProcessor : IKernelProcessor<BoardKernelSO, CellState, TileObject>
{
    readonly Dictionary<Guid, TileObject> m_spawnOptionsByID;
    
    public CellStateKernelProcessor(Dictionary<Guid, TileObject> spawnOptionsByID)
    {
        m_spawnOptionsByID = spawnOptionsByID;
    }
    
    public bool TryProcessKernel(BoardKernelSO kernel, Grid2D<GridTile<TileObject>> grid, Dictionary<Vector2Int, Guid> pending, int x, int y, out List<BoardStateData<TileObject>> results)
    {
        results = null;
        Guid? matchID = null;
        Vector2Int anchor = kernel.Anchor;

        for (int kr = 0; kr < kernel.Board.Rows; kr++)
        {
            for (int kc = 0; kc < kernel.Board.Columns; kc++)
            {
                CellState state = kernel.Board.Value[kr, kc];
                if (state is CellState.Result) continue;

                int wx = x + kc - anchor.x;
                int wy = y + kr - anchor.y;

                Guid? cellID = ReadPosition(grid, pending, wx, wy);

                switch (state)
                {
                    case CellState.Empty:
                        if (cellID != null) return false;
                        break;
                    case CellState.Match when matchID == null:
                        matchID = cellID;
                        break;
                    case CellState.Match when cellID == null:
                    case CellState.Match when matchID != cellID:
                    case CellState.Mismatch when cellID == null:
                    case CellState.Mismatch when matchID != null && cellID == matchID:
                        return false;
                    case CellState.Match:
                    case CellState.Mismatch:
                        break;
                    case CellState.Result:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        if (matchID == null) return false;
        results = new() { new(m_spawnOptionsByID[matchID.Value], new(x, y)) };
        return true;
    }

    static Guid? ReadPosition(Grid2D<GridTile<TileObject>> grid, Dictionary<Vector2Int, Guid> pending, int x, int y)
    {
        Vector2Int pos = new(x, y);
        return pending.TryGetValue(pos, out Guid id) ? id : grid[pos]?.Value?.ID;
    }
}