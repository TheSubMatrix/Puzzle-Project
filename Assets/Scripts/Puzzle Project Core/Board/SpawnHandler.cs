using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnHandler : IBoardProcessor<TileObject>
{
    [field:SerializeField] public SerializableHashSet<TileObject> SpawnOptions { get; private set;} = new();
    [field:SerializeField] public List<BoardKernelSO> Patterns { get; private set; } = new();

    [field:SerializeField] public List<BoardKernelSO> FallbackPatterns { get; private set; } = new();
    
    Dictionary<Guid, TileObject> m_spawnOptionsByID;
    Dictionary<Guid, TileObject> SpawnOptionsByID => m_spawnOptionsByID ??= SpawnOptions.ToDictionary(x => x.ID);
    
    CellStateKernelProcessor m_processor;
    
    public List<BoardStateData<TileObject>> ProcessBoardState(Grid2D<GridTile<TileObject>> grid)
    {
        m_spawnOptionsByID = SpawnOptions.ToDictionary(x => x.ID);
        m_processor = new(m_spawnOptionsByID);
        List<BoardStateData<TileObject>> spawnData = new();
        Dictionary<Vector2Int, Guid> pending = new();
        
        for(int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                TileObject tile = GetNextTile(grid, pending, i, j);
                Vector2Int pos = new(i, j);
                spawnData.Add(new(tile, pos));
                pending[pos] = tile.ID;
            }
        }
        return spawnData;
    }

    TileObject GetNextTile(Grid2D<GridTile<TileObject>> grid, Dictionary<Vector2Int, Guid> pending, int x, int y)
    {
        List<BoardStateData<TileObject>> candidates = GetCandidates(grid, pending, x, y);
        
        if (candidates.Count == 0)
            candidates = GetFallbackCandidates(grid, pending, x, y);
            
        return candidates.Count == 0 
            ? SpawnOptions.ElementAt(Random.Range(0, SpawnOptions.Count)) 
            : candidates[Random.Range(0, candidates.Count)].SpawnedObject;
    }

    List<BoardStateData<TileObject>> GetCandidates(Grid2D<GridTile<TileObject>> grid, Dictionary<Vector2Int, Guid> pending, int x, int y)
    {
        List<BoardStateData<TileObject>> candidates = new();
        
        foreach (BoardKernelSO pattern in Patterns)
            if (m_processor.TryProcessKernel(pattern, grid, pending, x, y, out List<BoardStateData<TileObject>> results))
                candidates.AddRange(results);
        return candidates;
    }

    List<BoardStateData<TileObject>> GetFallbackCandidates(Grid2D<GridTile<TileObject>> grid, Dictionary<Vector2Int, Guid> pending, int x, int y)
    {
        List<BoardStateData<TileObject>> candidates = new();
        
        foreach (BoardKernelSO pattern in FallbackPatterns)
            if (m_processor.TryProcessKernel(pattern, grid, pending, x, y, out List<BoardStateData<TileObject>> results))
                candidates.AddRange(results);
        
        return candidates;
    }
}