using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SolvedGridGenerator
{
    private readonly int BoxSize;
    private readonly int GridSize;

    private int[,] _grid;
    private System.Random _random;

    public SolvedGridGenerator(int boxSize, int? seed = null)
    {
        BoxSize = boxSize;
        GridSize = boxSize * boxSize;

        _random = seed.HasValue 
            ? new System.Random(seed.Value) 
            : new System.Random();
    }

    public int[,] Generate()
    {
        _grid = GenerateBasePattern();

        ShuffleNumbers();
        ShuffleRowBands();
        ShuffleColumnStacks();
        ShuffleRowsInsideBands();
        ShuffleColumnsInsideStacks();

        return _grid;
    }

    private int[,] GenerateBasePattern()
    {
        int[,] baseGrid = new int[GridSize, GridSize];

        for (int row = 0; row < GridSize; row++)
            for (int column = 0; column < GridSize; column++)
                baseGrid[row, column] = (row * BoxSize + row / BoxSize + column) % GridSize + 1;

        return baseGrid;
    }

    private void ShuffleNumbers()
    {
        var map = new List<int>();

        for (int i = 1; i <= GridSize; i++)
            map.Add(i);

        ShuffleList(map);

        for (int row = 0; row < GridSize; row++)
            for (int column = 0; column < GridSize; column++)
                _grid[row, column] = map[_grid[row, column] - 1];
    }

    private void ShuffleRowBands()
    {
        for (int i = 0; i < BoxSize; i++)
        {
            int band1 = _random.Next(BoxSize);
            int band2 = _random.Next(BoxSize);
            SwapRowBand(band1, band2);
        }
    }

    private void ShuffleColumnStacks()
    {
        for (int i = 0; i < BoxSize; i++)
        {
            int column1 = _random.Next(BoxSize);
            int column2 = _random.Next(BoxSize);
            SwapColumnStack(column1, column2);
        }
    }

    private void ShuffleRowsInsideBands()
    {
        for (int band = 0; band < BoxSize; band++)
        {
            for (int i = 0; i <= BoxSize; i++)
            {
                int row1 = band * BoxSize + _random.Next(BoxSize);
                int row2 = band * BoxSize + _random.Next(BoxSize);
                SwapRows(row1, row2);
            }
        }
    }

    private void ShuffleColumnsInsideStacks()
    {
        for (int stack = 0; stack < BoxSize; stack++)
        {
            for (int i = 0; i <= BoxSize; i++)
            {
                int column1 = stack * BoxSize + _random.Next(BoxSize);
                int column2 = stack * BoxSize + _random.Next(BoxSize);
                SwapColumns(column1, column2);
            }
        }
    }

    private void SwapRows(int row1, int row2)
    {
        for (int column = 0; column < GridSize; column++)
            (_grid[row1, column], _grid[row2, column]) = (_grid[row2, column], _grid[row1, column]);
    }

    private void SwapColumns(int column1, int column2)
    {
        for (int row = 0; row < GridSize; row++)
            (_grid[row, column1], _grid[row, column2]) = (_grid[row, column2], _grid[row, column1]);
    }

    private void SwapRowBand(int band1, int band2)
    {
        for (int i = 0; i < BoxSize; i++)
            SwapRows(band1 * BoxSize + i, band2 * BoxSize + i);
    }

    private void SwapColumnStack(int stack1, int stack2)
    {
        for (int i = 0; i < BoxSize; i++)
            SwapColumns(stack1 * BoxSize + i, stack2 * BoxSize + i);
    }

    private void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = _random.Next(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}