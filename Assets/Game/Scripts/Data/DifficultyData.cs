using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "Difficulties/New Difficulty")]
public class DifficultyData : ScriptableObject
{
    [SerializeField] private string _id;

    [SerializeField] private string _name;
    [SerializeField] private int _sudokuBoxSize;

    [SerializeField] private int _minClues;
    [SerializeField] private int _maxClues;

    [SerializeField] private int _maxBacktracking;

    [SerializeField] private bool _allowGuessing;

    public string Id => _id;
    public string Name => _name;
    public int SudokuBoxSize => _sudokuBoxSize;
    public int MinClues => _minClues;
    public int MaxClues => _maxClues;
    public int MaxBacktracking => _maxBacktracking;
    public bool AllowGuessing => _allowGuessing;
}
