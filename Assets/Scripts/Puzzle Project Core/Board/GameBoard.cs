using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    public Grid2D<GridTile<TileObject>> Grid { get; }
    readonly Queue<TileObject>[] m_fallQueues;
    
    public BoardState(uint width, uint height, float cellSize, Vector2 origin)
    {
        Grid = new(width, height, cellSize, origin);
        m_fallQueues = new Queue<TileObject>[width];
        for (int i = 0; i < width; i++)
            m_fallQueues[i] = new();
    }
    
    public Queue<TileObject> GetFallQueue(int column) => m_fallQueues[column];
    public void EnqueueFall(int column, TileObject tile) => m_fallQueues[column].Enqueue(tile);
    public bool TryDequeueFall(int column, out TileObject tile) => m_fallQueues[column].TryDequeue(out tile);
    public bool HasFalling(int column) => m_fallQueues[column].Count > 0;
}