using UnityEngine;

public interface IPuzzlesProvider
{
   public SudokuPuzzle GetPuzzle(DifficultyData difficultyData); 
}
