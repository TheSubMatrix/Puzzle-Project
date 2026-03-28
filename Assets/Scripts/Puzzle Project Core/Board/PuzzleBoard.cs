using System.Collections.Generic;
using MatrixUtils.Attributes;
using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
    [SerializeField] uint m_width;
    [SerializeField] uint m_height;
    [SerializeField] float m_cellSize;
    [SerializeReference, ClassSelector] IBoardProcessor<TileObject> m_spawnHandler;
    [SerializeReference, ClassSelector] IBoardProcessor<TileObject> m_moveHandler;
    Grid2D<GridTile<TileObject>> m_grid;
    void Start()
    {
        m_grid = new(m_width, m_height, m_cellSize, new(-m_width * m_cellSize * 0.5f, -m_height * m_cellSize * 0.5f));
        UpdateTileData(m_spawnHandler.ProcessBoardState(m_grid));
    }
    void UpdateTileData(List<BoardStateData<TileObject>> spawnData)
    {
        foreach (BoardStateData<TileObject> data in spawnData)
        {
            m_grid[data.Position] = new(Instantiate(data.SpawnedObject, m_grid.GetCellCenter(data.Position), Quaternion.identity), m_grid, data.Position);
        }
    }
}