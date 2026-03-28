using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnHandler : IBoardProcessor<TileObject>
{
    [field:SerializeField] public SerializableHashSet<TileObject> SpawnOptions { get; private set;} = new();
    Dictionary<Guid, TileObject> m_spawnOptionsByID;
    Dictionary<Guid, TileObject> SpawnOptionsByID => m_spawnOptionsByID ??= SpawnOptions.ToDictionary(x => x.ID);
    public List<BoardStateData<TileObject>> ProcessBoardState(Grid2D<GridTile<TileObject>> grid)
    {
        m_spawnOptionsByID = SpawnOptions.ToDictionary(x => x.ID);
        List<BoardStateData<TileObject>> spawnData = new();
        for(int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                spawnData.Add(new(SpawnOptions.ElementAt(Random.Range(0, SpawnOptions.Count)), new(i,j)));
            }
        }
        return spawnData;
    }
}