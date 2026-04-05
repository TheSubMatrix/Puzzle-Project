using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleBoard : MonoBehaviour
{
    [SerializeField] SolutionSO m_solution;
    [SerializeField] uint m_shuffleMoveTotal;
    [SerializeField] float m_shuffleTime;
    [SerializeField] UnityEvent m_onPuzzleSolved = new();
    [SerializeField] UnityEvent m_onPuzzleStarted = new();
    [SerializeField] List<Sprite> m_puzzleSprites = new();
    Grid2D<GridTile<PuzzlePiece>> m_grid;
    Vector2Int m_emptyCell;
    bool m_isMoving;
    bool UseAlternativeSprite => m_puzzleSprites.Count > 0;
    IEnumerator Start()
    {
        Sprite spriteToUse = m_puzzleSprites[Random.Range(0, m_puzzleSprites.Count)];
        m_grid = new((uint)m_solution.Grid.Columns,
            (uint)m_solution.Grid.Rows,
            1f,
            new(transform.position.x - m_solution.Grid.Columns * 0.5f,
                transform.position.y - m_solution.Grid.Rows * 0.5f));
        for (int x = 0; x < m_grid.Width; x++)
        {
            for (int y = 0; y < m_grid.Height; y++)
            {
                int row = m_solution.Grid.Rows - 1 - y;
                if (m_solution.Grid[row, x] == null)
                {
                    m_emptyCell = new(x, y);
                    m_grid[x, y] = null;
                    continue;
                }
                m_grid[x, y] = new(Instantiate(m_solution.Grid[row, x]), m_grid, x, y);
                m_grid[x, y].Value.transform.position = m_grid.GetCellCenter(new(x, y));
                if (!UseAlternativeSprite) continue;
                m_grid[x,y].Value.GetComponent<SpriteRenderer>().sprite = spriteToUse;
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
            yield return MoveTo(selectedNeighbor, m_emptyCell, m_shuffleTime / m_shuffleMoveTotal);
            lastNeighborPos = m_emptyCell;
            m_emptyCell = neighborOldPos;
        }
        m_onPuzzleStarted.Invoke();
    }
    List<GridTile<PuzzlePiece>> GetCellNeighbors(int x, int y, bool filterEmpty = true)
    {
        List<GridTile<PuzzlePiece>> neighbors = new();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = new(x + dir.x, y + dir.y);
            if (!m_grid.IsValid(neighborPos)) continue;
            GridTile<PuzzlePiece> tile = m_grid[neighborPos.x, neighborPos.y];
            if (tile is null && filterEmpty) continue;
            neighbors.Add(tile);
        }
        return neighbors;
    }

    IEnumerator MoveTo(GridTile<PuzzlePiece> pieceToMove, Vector2Int newPosition, float time)
    {
        m_isMoving = true;
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            pieceToMove.Value.transform.position = Vector3.Lerp(pieceToMove.Value.transform.position, m_grid.GetCellCenter(newPosition), elapsed / time);
            yield return null;
        }
        m_isMoving = false;
    }
    public void MoveTile(Vector2 desiredTile, Vector2 tileToMoveTo)
    {
        if (m_isMoving) return;
        Vector2Int desiredCell = m_grid.GetCellAtPosition(desiredTile);
        Vector2Int cellToMoveTo = m_grid.GetCellAtPosition(tileToMoveTo);
        if (!m_grid.IsValid(desiredCell) || cellToMoveTo != m_emptyCell) return;
        if (!GetCellNeighbors(cellToMoveTo.x, cellToMoveTo.y, false).Contains(m_grid[desiredCell])) return;
        GridTile<PuzzlePiece> pieceToMove = m_grid[desiredCell];
        m_grid[m_emptyCell] = pieceToMove;
        m_grid[desiredCell] = null;
        m_emptyCell = cellToMoveTo;
        StartCoroutine(MoveTo(m_grid[cellToMoveTo], m_emptyCell, 0.25f));
        m_emptyCell = desiredCell;
        if(CheckWin()) OnPuzzleSolved();
    }
    bool CheckWin()
    {
        for (int x = 0; x < m_grid.Width; x++)
        {
            for (int y = 0; y < m_grid.Height; y++)
            {
                int row = m_solution.Grid.Rows - 1 - y;
                PuzzlePiece solutionPiece = m_solution.Grid[row, x];
                GridTile<PuzzlePiece> gridTile = m_grid[x, y];
                if (solutionPiece == null && gridTile == null) continue;
                if (solutionPiece == null || gridTile == null) return false;
                if (solutionPiece.ID != gridTile.Value.ID) return false;
            }
        }
        return true;
    }

    void OnPuzzleSolved()
    {
        m_onPuzzleSolved.Invoke();
    }
}
