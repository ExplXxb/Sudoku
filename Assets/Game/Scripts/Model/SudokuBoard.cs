public class SudokuBoard
{
    private SudokuCell[,] _cells;

    public int Size { get; private set; }

    public SudokuBoard(SudokuPuzzle puzzle)
    {
        Size = puzzle.Size;
        _cells = new SudokuCell[Size, Size];

        for (int r = 0; r < Size; r++)
        {
            for (int c = 0; c < Size; c++)
            {
                int unsolved = puzzle.Unsolved[r, c];
                int solved = puzzle.Solved[r, c];

                bool isDefault = unsolved != 0;

                _cells[r, c] = new SudokuCell(
                    unsolved,
                    solved,
                    isDefault
                );
            }
        }
    }

    public SudokuCell GetCell(int row, int col)
    {
        return _cells[row, col];
    }
}