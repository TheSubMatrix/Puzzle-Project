using UnityEngine;
public class GridTile<T> where T : IIdentifiable
{
    public GridTile(T value, Grid2D<GridTile<T>> connectedGrid, int x, int y)
    {
        ConnectedGrid = connectedGrid;
        Value = value;
        Position = new(x, y);
    }
    public T Value;
    public Grid2D<GridTile<T>> ConnectedGrid;
    public Vector2Int Position;

    public GridTile(T value, Grid2D<GridTile<T>> connectedGrid, Vector2Int position)
    {
        ConnectedGrid = connectedGrid;
        Value = value;
        Position = position;
    }
}