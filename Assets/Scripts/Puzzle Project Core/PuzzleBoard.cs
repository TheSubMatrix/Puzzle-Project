using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
    [SerializeField] SolutionSO m_solution;
    [SerializeField] uint m_shuffleMoveTotal;
    Grid2D<GridTile<PuzzlePiece>> m_grid;
    Vector2Int m_emptyCell;
    void Start()
    {
        m_grid = new((uint)m_solution.Grid.Rows, (uint)m_solution.Grid.Columns, 1f, Vector2.zero);
        for (int w = 0; w < m_grid.Width; w++)
        {
            for (int h = 0; h < m_grid.Height; h++)
            {
                if (m_solution.Grid[w, h] == null)
                {
                    m_emptyCell = new(w, h);
                    m_grid[w, h] = null;
                    continue;
                }
                m_grid[w, h] = new(Instantiate(m_solution.Grid[w, h]), m_grid, w, h);
            }
        }
        for (int i = 0; i < m_shuffleMoveTotal; i++)
        {
            List<GridTile<PuzzlePiece>> neighbors = GetCellNeighbors(m_emptyCell.x, m_emptyCell.y);
            if (neighbors.Count <= 0) break; 
            GridTile<PuzzlePiece> selectedNeighbor = neighbors[Random.Range(0, neighbors.Count)];
            Vector2Int neighborOldPos = selectedNeighbor.Position;
            m_grid[m_emptyCell] = selectedNeighbor;
            m_grid[neighborOldPos] = null;
            selectedNeighbor.Position = m_emptyCell;
            selectedNeighbor.Value.transform.position = m_grid.GetCellCenter(m_emptyCell);
            m_emptyCell = neighborOldPos;
        }
    }
    List<GridTile<PuzzlePiece>> GetCellNeighbors(int x, int y)
    {
        List<GridTile<PuzzlePiece>> neighbors = new();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = new(x + dir.x, y + dir.y);
            if (neighborPos.x < 0 || neighborPos.x >= m_grid.Width ||
                neighborPos.y < 0 || neighborPos.y >= m_grid.Height) continue;
            GridTile<PuzzlePiece> tile = m_grid[neighborPos.x, neighborPos.y];
            if (tile is null) continue;
            neighbors.Add(tile);
        }
        return neighbors;
    }
}
