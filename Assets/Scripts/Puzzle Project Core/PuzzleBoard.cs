using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
    [SerializeField] SolutionSO m_solution;
    [SerializeField] uint m_shuffleMoveTotal;
    Grid2D<GridTile<PuzzlePiece>> m_grid;
    Vector2Int m_emptyCell;
    IEnumerator Start()
    {
        m_grid = new((uint)m_solution.Grid.Columns, (uint)m_solution.Grid.Rows, 1f, new(transform.position.x - m_solution.Grid.Columns * 0.5f,transform.position.y - m_solution.Grid.Rows * 0.5f));
        for (int x = 0; x < m_grid.Width; x++)
        {
            for (int y = 0; y < m_grid.Height; y++)
            {
                int row = (m_solution.Grid.Rows - 1) - y;
                if (m_solution.Grid[row, x] == null)
                {
                    m_emptyCell = new(x, y);
                    m_grid[x, y] = null;
                    continue;
                }
                m_grid[x, y] = new(Instantiate(m_solution.Grid[row, x]), m_grid, x, y);
                m_grid[x, y].Value.transform.position = m_grid.GetCellCenter(new(x, y));
            }
        }
        Vector2Int? lastNeighborPos = null;
        for (int i = 0; i < m_shuffleMoveTotal; i++)
        {
            List<GridTile<PuzzlePiece>> neighbors = GetCellNeighbors(m_emptyCell.x, m_emptyCell.y);
            if (lastNeighborPos.HasValue)
                neighbors.RemoveAll(n => n.Position == lastNeighborPos.Value);
            if (neighbors.Count <= 0) break;
            GridTile<PuzzlePiece> selectedNeighbor = neighbors[Random.Range(0, neighbors.Count)];
            Vector2Int neighborOldPos = selectedNeighbor.Position;
            m_grid[m_emptyCell] = selectedNeighbor;
            m_grid[neighborOldPos] = null;
            selectedNeighbor.Position = m_emptyCell;
            yield return MoveTo(selectedNeighbor, m_emptyCell, 0.1f);
            lastNeighborPos = m_emptyCell;
            m_emptyCell = neighborOldPos;
        }
        yield return null;
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

    IEnumerator MoveTo(GridTile<PuzzlePiece> pieceToMove, Vector2Int newPosition, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            pieceToMove.Value.transform.position = Vector3.Lerp(pieceToMove.Value.transform.position, m_grid.GetCellCenter(newPosition), elapsed / time);
            yield return null;
        }
    }
}
