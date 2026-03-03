using System;
using System.Collections.Generic;

public class UnsolvedGridGenerator
{
    private SudokuSolver _solver = new SudokuSolver();
    private SudokuDifficultyRater _rater = new SudokuDifficultyRater();

    public int[,] DeconstructFromTheSolved(int[,] solvedGrid, DifficultyData difficulty)
    {
        int size = solvedGrid.GetLength(0);

        int[,] puzzle = (int[,])solvedGrid.Clone();

        int cluesTarget = UnityEngine.Random.Range(
            difficulty.MinClues,
            difficulty.MaxClues + 1
        );

        int cellsToRemove = size * size - cluesTarget;

        List<(int r, int c)> cells = new List<(int r, int c)>();

        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                cells.Add((r, c));

        Shuffle(cells);

        int removed = 0;

        foreach (var cell in cells)
        {
            if (removed >= cellsToRemove)
                break;

            int r = cell.r;
            int c = cell.c;

            int backup = puzzle[r, c];
            puzzle[r, c] = 0;

            bool unique = _solver.HasUniqueSolution(puzzle, difficulty.SudokuBoxSize);
            bool matchesDifficulty = _rater.MatchesDifficulty(puzzle, difficulty);

            if (!unique || !matchesDifficulty)
            {
                puzzle[r, c] = backup;
            }
            else
            {
                removed++;
            }
        }

        return puzzle;
    }

    private void Shuffle(List<(int r, int c)> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}