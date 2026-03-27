using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnHandler : IBoardProcessor<TileObject>
{
    [field:SerializeField] public List<TileObject> SpawnOptions { get; private set;} = new();

    public List<BoardStateData<TileObject>> ProcessBoardState(Grid2D<GridTile<TileObject>> grid)
    {
        List<BoardStateData<TileObject>> spawnData = new();
        for(int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                spawnData.Add(new(SpawnOptions[Random.Range(0, SpawnOptions.Count)], new(i,j)));
            }
        }
        return spawnData;
    }
}