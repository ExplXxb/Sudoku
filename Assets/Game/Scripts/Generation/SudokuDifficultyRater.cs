public class SudokuDifficultyRater
{
    private SudokuSolver solver = new SudokuSolver();

    public bool MatchesDifficulty(int[,] puzzle, DifficultyData difficulty)
    {
        SudokuSolver solver = new SudokuSolver();

        if (!difficulty.AllowGuessing && solver.BacktrackCount > 0)
            return false;

        if (solver.BacktrackCount > difficulty.MaxBacktracking)
            return false;

        return true;
    }

    public int RateDifficulty(int[,] puzzle, int boxSize)
    {
        solver.AnalyzeDifficulty(puzzle, boxSize);
        return solver.BacktrackCount;
    }
}