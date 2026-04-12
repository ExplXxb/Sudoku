using UnityEngine;
using UnityEngine.Audio;

public class SudokuPuzzleGenerator
{        
    private SolvedGridGenerator solvedGridGenerator;
    private UnsolvedGridGenerator unsolvedGridGenerator = new UnsolvedGridGenerator();

    public SudokuPuzzleGenerator(int boxSize)
    {
        solvedGridGenerator = new SolvedGridGenerator(boxSize);
    }

    public SudokuPuzzle Generate(DifficultyData difficulty)
    {
        var solved = solvedGridGenerator.Generate();
        var unsolved = unsolvedGridGenerator.DeconstructFromTheSolved(solved, difficulty);

        return new SudokuPuzzle(solved, unsolved);
    }
}
