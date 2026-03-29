using UnityEngine;

public abstract class BoardKernelSOBase<T> : ScriptableObject
{
    public abstract SerializableGrid<T> Board{ get; }
    public abstract Vector2Int Anchor { get; }
}