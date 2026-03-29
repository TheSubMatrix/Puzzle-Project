using System;
using System.Collections.Generic;
using UnityEngine;

public interface IKernelProcessor<in TKernel, TKernelData, TTileData> 
    where TKernel : BoardKernelSOBase<TKernelData> 
    where TTileData : IIdentifiable
{
    bool TryProcessKernel(
        TKernel kernel,
        Grid2D<GridTile<TTileData>> grid,
        Dictionary<Vector2Int, Guid> pending,
        int x, int y,
        out List<BoardStateData<TTileData>> results);
}