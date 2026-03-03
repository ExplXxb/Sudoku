public class SudokuSolver
{
    public int BacktrackCount { get; private set; }

    private int size;
    private int boxSize;
    private int solutions;

    public void AnalyzeDifficulty(int[,] grid, int boxSize)
    {
        this.size = grid.GetLength(0);
        this.boxSize = boxSize;
        BacktrackCount = 0;
        solutions = 0;

        int[,] copy = (int[,])grid.Clone();

        Solve(copy);
    }

    public bool HasUniqueSolution(int[,] grid, int boxSize)
    {
        this.size = grid.GetLength(0);
        this.boxSize = boxSize;
        solutions = 0;
        BacktrackCount = 0;

        int[,] copy = (int[,])grid.Clone();

        Solve(copy);

        return solutions == 1;
    }

    private void Solve(int[,] grid)
    {
        if (solutions > 1)
            return;

        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                if (grid[r, c] == 0)
                {
                    BacktrackCount++;

                    for (int n = 1; n <= size; n++)
                    {
                        if (IsValid(grid, r, c, n))
                        {
                            grid[r, c] = n;
                            Solve(grid);
                            grid[r, c] = 0;
                        }
                    }
                    return;
                }
            }
        }

        solutions++;
    }

    private bool IsValid(int[,] grid, int row, int col, int num)
    {
        for (int i = 0; i < size; i++)
        {
            if (grid[row, i] == num) return false;
            if (grid[i, col] == num) return false;
        }

        int boxRow = (row / boxSize) * boxSize;
        int boxCol = (col / boxSize) * boxSize;

        for (int r = 0; r < boxSize; r++)
            for (int c = 0; c < boxSize; c++)
                if (grid[boxRow + r, boxCol + c] == num)
                    return false;

        return true;
    }
}