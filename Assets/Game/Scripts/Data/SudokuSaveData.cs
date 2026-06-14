using System.Collections.Generic;

[System.Serializable]
public class SudokuSaveData
{
    public int size;
    public float time;
    public string difficultyId;
    public int errors;
    public int lives;
    public List<CellSaveData> cells;
    public List<Move> history;
    public List<Move> redoHistory;
}