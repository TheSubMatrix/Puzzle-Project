using UnityEngine;

public readonly struct BoardStateData<T>
{
    public readonly T SpawnedObject;
    public readonly Vector2Int Position;
    public BoardStateData(T spawnedObject, Vector2Int position)
    {
        SpawnedObject = spawnedObject;
        Position = position;
    }
}