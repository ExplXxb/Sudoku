using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "Difficulties/New Difficulty")]
public class DifficultyData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _sudokuBoxSize;

    [SerializeField] private int _minClues;
    [SerializeField] private int _maxClues;

    [SerializeField] private int _maxBacktracking;

    [SerializeField] private bool _allowGuessing;

    public string Name 
    { 
        get 
        { 
            return _name; 
        } 
        private set 
        { 
            _name = value; 
        } 
    }

    public int SudokuBoxSize
    { 
        get 
        { 
            return _sudokuBoxSize; 
        } 
        private set 
        {
            _sudokuBoxSize = value; 
        } 
    }

    public int MinClues
    { 
        get 
        { 
            return _minClues; 
        } 
        private set 
        {
            _minClues = value; 
        } 
    }

    public int MaxClues
    { 
        get 
        { 
            return _maxClues; 
        } 
        private set 
        { 
            _maxClues = value; 
        } 
    }

    public int MaxBacktracking
    { 
        get 
        { 
            return _maxBacktracking; 
        } 
        private set 
        {
            _maxBacktracking = value; 
        } 
    }

    public bool AllowGuessing
    { 
        get 
        { 
            return _allowGuessing; 
        } 
        private set 
        {
            _allowGuessing = value; 
        } 
    }
}
