public class SudokuPuzzle
{
    public int[,] Solved { get; private set; }
    public int[,] Unsolved { get; private set; }

    public int Size => Solved.GetLength(0);

    public SudokuPuzzle(int[,] solved, int[,] unsolved)
    {
        Solved = solved;
        Unsolved = unsolved;
    }
}
