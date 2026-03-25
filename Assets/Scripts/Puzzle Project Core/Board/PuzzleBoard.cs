using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
    [SerializeField] uint m_width;
    [SerializeField] uint m_height;
    [SerializeField] Vector2 m_tileSize;
    [SerializeField] List<TileObject> m_tilePrefabs;
    Grid2D<GridTile<TileObject>> m_grid;
    void Start()
    {
        m_grid = new(width: m_width, height: m_height, cellSize: 1f, origin: Vector2.zero);
        for (int i = 0; i < m_grid.Width; i++)
        {
            for (int j = 0; j < m_grid.Height; j++)
            {
                TileObject newTile = Instantiate(m_tilePrefabs[Random.Range(0, m_tilePrefabs.Count)], m_grid.GetCellPosition(new(i, j)), Quaternion.identity);
                m_grid[i, j] = new(newTile, m_grid, i, j);
            }
        }
    }
}