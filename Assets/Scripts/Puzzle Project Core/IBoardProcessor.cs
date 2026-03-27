using System.Collections.Generic;

public interface IBoardProcessor<T> where T : IIdentifiable
{
    public List<BoardStateData<T>> ProcessBoardState(Grid2D<GridTile<T>> grid);
}