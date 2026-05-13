using System.Collections.Generic;

[System.Serializable]
public class SudokuSaveData
{
    public int size;
    public float time;
    public string difficultyName;
    public int errors;
    public int lives;
    public List<CellSaveData> cells;
    public List<Move> history;
}