using System;
using UnityEngine;

public class Grid2D<T>
{
    public Grid2D(uint width, uint height, float cellSize, Vector2 origin)
    {
        Width = width;
        Height = height;
        m_cellSize = cellSize;
        Origin = origin;
        m_grid = new T[width, height];
    }
    public readonly uint Width;
    public readonly uint Height;
    public event Action<int, int, T> OnCellChanged = delegate { };
    readonly float m_cellSize;
    public Vector2 Origin { get; private set; }
    readonly T[,] m_grid;
    public Vector2 GetCellPosition(Vector2Int cell) => Origin + new Vector2(cell.x, cell.y) * m_cellSize;
    public Vector2 GetCellCenter(Vector2Int cell) => new(cell.x * m_cellSize + (m_cellSize * 0.5f), cell.y * m_cellSize + (m_cellSize * 0.5f));
    public Vector2Int GetCellAtPosition(Vector2 worldPosition)
    {
        Vector2 gridPosition = (worldPosition - Origin) / m_cellSize;
        return new(Mathf.FloorToInt(gridPosition.x), Mathf.FloorToInt(gridPosition.y));
    }
    bool IsValid(Vector2Int cell) => cell is { x: >= 0, y: >= 0 } && cell.x < Width && cell.y < Height;
    bool IsValid(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
    public T this[Vector2Int position]
    {
        get => this[position.x, position.y];
        set => this[position.x, position.y] = value;
    }
    public T this[int x, int y]
    {
        get => IsValid(x,y) ? m_grid[x, y] : default;
        set 
        {
            if(!IsValid(x,y))return;
            m_grid[x, y] = value;
            OnCellChanged(x, y, value);
        }
    }
}
