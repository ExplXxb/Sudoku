using System.Collections.Generic;
using UnityEngine;

public class SudokuGameState : MonoBehaviour
{
    private const string SaveKey = "sudoku_save";

    [SerializeField] private SudokuGridView _gridView;

    private System.Random _rng = new System.Random();
    private SudokuBoard _board;
    private SudokuCell _selectedCell;
    private int _selectedIndex = -1;
    private bool _notesMode = false;
    private Stack<Move> _history = new Stack<Move>();

    private void Start()
    {
        if (GameSettings.Instance.GetContinuePreviousGame() && SaveLoadData.Exists(SaveKey))
            LoadGame();
        else
            StartGame();

        AdsManager.Instance.ShowBanner();
    }

    private void OnEnable()
    {
        GameEvents.OnSquareSelected += SelectCell;
        GameEvents.OnUpdateSquareNumber += SetNumber;
        GameEvents.OnClearCell += ClearSelected;
        GameEvents.OnGiveAHint += GiveAHint;
        GameEvents.OnNotesActive += SetNotesMode;
        GameEvents.OnGameOver += DeleteSave;
        GameEvents.OnExitToMenu += SaveGame;
        GameEvents.OnUndo += Undo;
    }

    private void OnDisable()
    {
        GameEvents.OnSquareSelected -= SelectCell;
        GameEvents.OnUpdateSquareNumber -= SetNumber;
        GameEvents.OnClearCell -= ClearSelected;
        GameEvents.OnGiveAHint -= GiveAHint;
        GameEvents.OnNotesActive -= SetNotesMode;
        GameEvents.OnGameOver -= DeleteSave;
        GameEvents.OnExitToMenu -= SaveGame;
        GameEvents.OnUndo -= Undo;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void StartGame()
    {
        LivesView.Instance.SetLives(LivesView.MaxLivesCount);

        _history.Clear();

        var difficulty = GameSettings.Instance.GetDifficulty();

        var generator = new SudokuPuzzleGenerator(difficulty.SudokuBoxSize);
        var puzzle = generator.Generate(difficulty);

        _board = new SudokuBoard(puzzle);

        _gridView.CreateGrid(_board.Size);
        _gridView.Draw(_board, _selectedIndex);
    }

    public void SaveGame()
    {
        if (_board == null)
            return;

        if (LivesView.Instance.LivesCount <= 0)
            return;

        SudokuSaveData data = new SudokuSaveData();

        data.size = _board.Size;
        data.time = Clock.Instance.GetTime();
        data.lives = LivesView.Instance.LivesCount;
        data.difficultyName = GameSettings.Instance.GetDifficulty().Name;

        var historyList = new List<Move>();

        foreach (var move in _history)
        {
            historyList.Add(move.Clone());
        }

        data.history = historyList;

        data.cells = new List<CellSaveData>();

        for (int r = 0; r < _board.Size; r++)
        {
            for (int c = 0; c < _board.Size; c++)
            {
                var cell = _board.GetCell(r, c);

                CellSaveData cellData = new CellSaveData();
                cellData.value = cell.Value;
                cellData.correctValue = cell.CorrectValue;
                cellData.isDefault = cell.IsDefault;
                cellData.notes = cell.GetNotes();

                data.cells.Add(cellData);
            }
        }

        SaveLoadData.Save(SaveKey, data);
    }

    public void LoadGame()
    {
        SudokuSaveData data = SaveLoadData.Load<SudokuSaveData>(SaveKey);

        Clock.Instance.SetTime(data.time);
        LivesView.Instance.SetLives(data.lives);

        var historyList = data.history ?? new List<Move>();

        historyList.Reverse();

        _history = new Stack<Move>();

        foreach (var move in historyList)
        {
            _history.Push(move.Clone());
        }

        int size = data.size;

        int[,] solved = new int[size, size];
        int[,] unsolved = new int[size, size];

        int index = 0;

        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                var cellData = data.cells[index];
                index++;

                solved[r, c] = cellData.correctValue;
                unsolved[r, c] = cellData.isDefault ? cellData.value : 0;
            }
        }

        SudokuPuzzle puzzle = new SudokuPuzzle(solved, unsolved);
        _board = new SudokuBoard(puzzle);

        index = 0;

        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                var cellData = data.cells[index];
                index++;

                var cell = _board.GetCell(r, c);

                cell.SetValue(cellData.value);

                for (int i = 0; i < cellData.notes.Length; i++)
                {
                    if (cellData.notes[i])
                        cell.AddNote(i + 1);
                }
            }
        }

        _gridView.CreateGrid(size);
        _gridView.Draw(_board, -1);
    }

    public void DeleteSave()
    {
        SaveLoadData.Delete(SaveKey);
        GameSettings.Instance.SetContinuePreviousGame(false);
    }

    private void Undo()
    {
        if (_history.Count == 0)
            return;

        var move = _history.Pop();

        var cell = _board.GetCell(move.Row, move.Col);

        cell.SetValue(move.PreviousValue);

        cell.ClearNotes();

        for (int i = 0; i < move.PreviousNotes.Length; i++)
        {
            if (move.PreviousNotes[i])
                cell.AddNote(i + 1);
        }

        _gridView.Draw(_board, _selectedIndex);

        SaveGame();

        CheckBoardCompleted();
    }

    private void SelectCell(int index)
    {
        if (_board == null)
            return;

        if (_selectedIndex != -1)
            _gridView.SetSelected(_selectedIndex, false);

        _selectedIndex = index;
        _gridView.SetSelected(_selectedIndex, true);
    }

    private void ClearSelected()
    {
        if (_selectedIndex < 0)
            return;

        int size = _board.Size;

        int row = _selectedIndex / size;
        int column = _selectedIndex % size;

        var cell = _board.GetCell(row, column);

        if (cell.IsDefault || cell.IsCorrect())
            return;

        var move = new Move
        {
            Row = row,
            Col = column,
            PreviousValue = cell.Value,
            PreviousNotes = (bool[])cell.GetNotes().Clone()
        };

        cell.Clear();

        move.NewValue = cell.Value;
        move.NewNotes = (bool[])cell.GetNotes().Clone();

        _history.Push(move);

        SaveGame();

        _gridView.Draw(_board, _selectedIndex);
    }

    private void SetNumber(int number)
    {
        if (_selectedIndex < 0)
            return;

        int size = _board.Size;

        int row = _selectedIndex / size;
        int column = _selectedIndex % size;

        var cell = _board.GetCell(row, column);

        if (cell.IsDefault || cell.IsCorrect())
            return;

        var move = new Move
        {
            Row = row,
            Col = column,
            PreviousValue = cell.Value,
            PreviousNotes = (bool[])cell.GetNotes().Clone()
        };

        if (_notesMode)
        {
            if (cell.HasNote(number))
                cell.RemoveNote(number);
            else
                cell.AddNote(number);
        }
        else
        {
            if (cell.Value == number)
                cell.Clear();
            else
            {
                cell.SetValue(number);

                if (number != 0 && cell.IsCorrect() == false)
                {
                    GameEvents.OnWrongNumberMethod();
                }
            }
        }

        move.NewValue = cell.Value;
        move.NewNotes = (bool[])cell.GetNotes().Clone();

        if (move.PreviousValue == move.NewValue && NotesEqual(move.PreviousNotes, move.NewNotes))
        {
            return;
        }

        _history.Push(move);

        _gridView.Draw(_board, _selectedIndex);

        SaveGame();

        CheckBoardCompleted();
    }

    private bool IsBoardCompleted()
    {
        int size = _board.Size;

        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                var cell = _board.GetCell(r, c);

                if (!cell.IsCorrect())
                    return false;
            }
        }

        return true;
    }

    private bool NotesEqual(bool[] a, bool[] b)
    {
        if (a == null && b == null)
            return true;

        if (a == null || b == null)
            return false;

        if (a.Length != b.Length)
            return false;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
                return false;
        }

        return true;
    }

    private void GiveAHint()
    {
        if (_selectedIndex < 0)
        {
            GiveRandomHint();
            return;
        }

        int size = _board.Size;

        int row = _selectedIndex / size;
        int column = _selectedIndex % size;

        var cell = _board.GetCell(row, column);

        if (cell.IsDefault || cell.IsCorrect())
        {
            GiveRandomHint();
            return;
        }

        var move = new Move
        {
            Row = row,
            Col = column,
            PreviousValue = cell.Value,
            PreviousNotes = (bool[])cell.GetNotes().Clone()
        };

        cell.SetValue(cell.CorrectValue);

        move.NewValue = cell.Value;
        move.NewNotes = (bool[])cell.GetNotes().Clone();

        _history.Push(move);

        SaveGame();

        _gridView.Draw(_board, _selectedIndex);

        CheckBoardCompleted();
    }

    private void GiveRandomHint()
    {
        int size = _board.Size;

        var availableCells = new List<(int row, int col, SudokuCell cell)>();

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var currentCell = _board.GetCell(i, j);

                if (!currentCell.IsDefault && !currentCell.IsCorrect())
                    availableCells.Add((i, j, currentCell));
            }
        }

        if (availableCells.Count == 0)
        {
            CheckBoardCompleted();
            return;
        }

        var picked = availableCells[_rng.Next(availableCells.Count)];

        var move = new Move
        {
            Row = picked.row,
            Col = picked.col,
            PreviousValue = picked.cell.Value,
            PreviousNotes = (bool[])picked.cell.GetNotes().Clone()
        };

        picked.cell.SetValue(picked.cell.CorrectValue);

        move.NewValue = picked.cell.Value;
        move.NewNotes = (bool[])picked.cell.GetNotes().Clone();

        _history.Push(move);

        SaveGame();

        _gridView.Draw(_board, -1);

        CheckBoardCompleted();
    }

    private void SetNotesMode(bool active)
    {
        _notesMode = active;
    }

    public void DebugSolveButton()
    {
        SudokuCell lastUnsolved = null;

        for (int i = 0; i < _board.Size; i++)
        {
            for (int j = 0; j < _board.Size; j++)
            {
                var cell = _board.GetCell(i, j);

                if (!cell.IsDefault && cell.Value != cell.CorrectValue)
                {
                    lastUnsolved = cell;
                    break;
                }
            }
            if (lastUnsolved != null)
                break;
        }

        for (int i = 0; i < _board.Size; i++)
        {
            for (int j = 0; j < _board.Size; j++)
            {
                var cell = _board.GetCell(i, j);

                if (cell == lastUnsolved)
                    continue;

                cell.SetValue(cell.CorrectValue);
            }
        }

        _gridView.Draw(_board, _selectedIndex);
    }

    private void CheckBoardCompleted()
    {
        if (IsBoardCompleted())
        {
            DeleteSave();
            GameEvents.OnBoardCompletedMethod();
        }
    }
}