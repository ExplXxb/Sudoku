using UnityEngine;

public class GeneratedSudokuProvider : IPuzzlesProvider
{
    public SudokuPuzzle GetPuzzle(DifficultyData difficultyData)
    {
        SudokuPuzzleGenerator sudokuPuzzleGenerator = new SudokuPuzzleGenerator(difficultyData.SudokuBoxSize);
        return sudokuPuzzleGenerator.Generate(difficultyData);
    }
}
