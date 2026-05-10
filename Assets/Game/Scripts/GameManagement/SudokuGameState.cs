using System.Collections.Generic;
using UnityEngine;

public class SudokuGameState : MonoBehaviour
{
    private const string SaveKey = "sudoku_save";

    [SerializeField] private SudokuGridView _gridView;

    private SudokuBoard _board;
    private SudokuCell _selectedCell;
    private int _selectedIndex = -1;
    private bool _notesMode = false;

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
        GameEvents.OnBoardCompleted += DeleteSave;
        GameEvents.OnGiveAHint += SaveGame;
        GameEvents.OnExitToMenu += HandleExitToMenu;
    }

    private void OnDisable()
    {
        GameEvents.OnSquareSelected -= SelectCell;
        GameEvents.OnUpdateSquareNumber -= SetNumber;
        GameEvents.OnClearCell -= ClearSelected;
        GameEvents.OnGiveAHint -= GiveAHint;
        GameEvents.OnNotesActive -= SetNotesMode;
        GameEvents.OnGameOver -= DeleteSave;
        GameEvents.OnBoardCompleted -= DeleteSave;
        GameEvents.OnGiveAHint -= SaveGame;
        GameEvents.OnExitToMenu -= HandleExitToMenu;
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

    private void HandleExitToMenu()
    {
        SaveGame();
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

        if (cell.IsDefault)
            return;

        cell.Clear();

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

        if (cell.IsDefault)
            return;

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

        _gridView.Draw(_board, _selectedIndex);

        SaveGame();

        if (IsBoardCompleted())
        {
            GameEvents.OnBoardCompletedMethod();
        }
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

        if (cell.IsDefault || cell.IsCorrect() || _selectedIndex < 0)
        {
            GiveRandomHint();
            return;
        }

        cell.SetValue(cell.CorrectValue);

        _gridView.Draw(_board, _selectedIndex);
    }

    private void GiveRandomHint()
    {
        int size = _board.Size;
        System.Random rng = new System.Random();

        int index;
        SudokuCell cell;

        do
        {
            index = rng.Next(size * size);

            int row = index / size;
            int column = index % size;

            cell = _board.GetCell(row, column);

        } while (cell.IsDefault || cell.IsCorrect());

        cell.SetValue(cell.CorrectValue);

        _gridView.Draw(_board, -1);
    }

    private void SetNotesMode(bool active)
    {
        _notesMode = active;
    }

    public void DebugSolveButton()
    {
        for (int i = 0; i < _board.Size; i++)
            for (int j = 0; j < _board.Size; j ++)
                _board.GetCell(i, j).SetValue(_board.GetCell(i, j).CorrectValue);

        _gridView.Draw(_board, _selectedIndex);
    }
}