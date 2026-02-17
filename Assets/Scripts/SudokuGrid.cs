using System.Collections.Generic;
using UnityEngine;

public class SudokuGrid : MonoBehaviour
{
    [SerializeField] private int _columns = 0;
    [SerializeField] private int _rows = 0;
    [SerializeField] private float _squareOffset = 0.0f;
    [SerializeField] private GameObject _gridSquare;
    [SerializeField] private Vector2 _startPosition = new Vector2(0.0f, 0.0f);
    [SerializeField] private float _squareScale = 1.0f;
    [SerializeField] private float _squareGap = 0.1f;
    [SerializeField] private Color _lineHighlightColor = Color.red;

    private List<GameObject> _gridSquares = new List<GameObject>();
    private int _selectedGridData = -1;

    private void Start()
    {
        if (_gridSquare.GetComponent<SudokuGrid>() == null)
            Debug.LogError("This Game Object needs to have GridSquare script attached!");

        CreateGrid();

        if (GameSettings.Instance.GetContinuePreviousGame())
            SetGridFromFile();
        else
            SetGridNumber(GameSettings.Instance.GetGameMode());
    }

    void SetGridFromFile()
    {
        string difficulty = GameSettings.Instance.GetGameMode();
        _selectedGridData = Config.ReadBoardLevel();
        var data = Config.ReadGridData();

        SetGridSquareData(data);
        SetGridNotes(Config.GetGridNotes());
    }

    private void SetGridNotes(Dictionary<int, List<int>> notes)
    {
        foreach (var note in notes)
        {
            _gridSquares[note.Key].GetComponent<GridSquare>().SetGridNotes(note.Value);
        }
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        int squareIndex = 0;
        for (int row = 0; row < _rows; row++)
        {
            for (int column = 0; column < _columns; column++)
            {
                _gridSquares.Add(Instantiate(_gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetSquareIndex(squareIndex);
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform, false);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(_squareScale, _squareScale, _squareScale);

                squareIndex++;
            }
        }
    }

    private void SetSquaresPosition()
    {
        var squareRect = _gridSquares[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        Vector2 squareGapNumber = new Vector2(0.0f, 0.0f);
        bool rowMoved = false;

        offset.x = squareRect.rect.width * squareRect.transform.localScale.x + _squareOffset;
        offset.y = squareRect.rect.height * squareRect.transform.localScale.y + _squareOffset;

        int columnNumber = 0;
        int rowNumber = 0;

        foreach (GameObject square in _gridSquares)
        {
            if (columnNumber + 1 > _columns)
            {
                rowNumber++;
                columnNumber = 0;
                squareGapNumber.x = 0;
                rowMoved = false;
            }

            var xPositionOffset = offset.x * columnNumber + (squareGapNumber.x * _squareGap);
            var yPositionOffset = offset.y * rowNumber + (squareGapNumber.y * _squareGap);

            if (columnNumber > 0 && columnNumber % 3 == 0)
            {
                squareGapNumber.x++;
                xPositionOffset += _squareGap;
            }
            if (rowNumber > 0 && rowNumber % 3 == 0 && rowMoved == false)
            {
                rowMoved = true;
                squareGapNumber.y++;
                yPositionOffset += _squareGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(_startPosition.x + xPositionOffset, _startPosition.y - yPositionOffset);
            columnNumber++;
        }
    }

    private void SetGridNumber(string level)
    {
        _selectedGridData = Random.Range(0, SudokuData.Instance.SudokuGame[level].Count);
        var data = SudokuData.Instance.SudokuGame[level][_selectedGridData];

        SetGridSquareData(data);
    }

    private void SetGridSquareData(SudokuData.SudokuBoardData data)
    {
        for (int index = 0; index < _gridSquares.Count; index++)
        {
            _gridSquares[index].GetComponent<GridSquare>().SetHasDefaultValue(data.unsolvedData[index] != 0 && data.unsolvedData[index] == data.solvedData[index]);
            _gridSquares[index].GetComponent<GridSquare>().SetNumber(data.unsolvedData[index]);
            _gridSquares[index].GetComponent<GridSquare>().SetCorrectNumber(data.solvedData[index]);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnCheckBoardCompleted += CheckBoardCompleted;
        GameEvents.OnGiveAHint += GiveAHint;
    }

    private void OnDisable()
    {
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnCheckBoardCompleted -= CheckBoardCompleted;
        GameEvents.OnGiveAHint -= GiveAHint;

        var solved_data = SudokuData.Instance.SudokuGame[GameSettings.Instance.GetGameMode()][_selectedGridData].solvedData;
        int[] unsolved_data = new int[81];
        Dictionary<string, List<string>> grid_notes = new Dictionary<string, List<string>>();

        for (int i = 0; i < _gridSquares.Count; i++)
        {
            var comp = _gridSquares[i].GetComponent<GridSquare>();
            unsolved_data[i] = comp.GetSquareNumber();
            string key = "square_note:" + i.ToString();
            grid_notes.Add(key, comp.GetSquareNotes());
        }

        SudokuData.SudokuBoardData current_game_data = new SudokuData.SudokuBoardData(unsolved_data, solved_data);

        if (GameSettings.Instance.GetExitAfterWon() == false)
        {
            Config.SaveBoardData(current_game_data,
                GameSettings.Instance.GetGameMode(),
                _selectedGridData,
                Lives.Instance.GetErrorNumbers(),
                grid_notes);
        }
        else
            Config.DeleteDataFile();

        GameSettings.Instance.SetExitAfterWon(false);
    }

    private void GiveAHint()
    {
        var squareIndexes = new List<int>();

        for (var index = 0; index < _gridSquares.Count; index++)
        {
            var comp = _gridSquares[index].GetComponent<GridSquare>();
            if (comp.GetSquareNumber() == 0 && comp.GetHasDefaultValue() == false)
            {
                squareIndexes.Add(index);
            }
        }

        if (squareIndexes.Count == 0)
            return;

        var randomIndex = UnityEngine.Random.Range(0, squareIndexes.Count);
        var squareIndex = squareIndexes[randomIndex];
        _gridSquares[squareIndex].GetComponent<GridSquare>().SetCorrectValueOnHint();
    }

    private void SetSquaresColor(int[] data, Color color)
    {
        foreach (var index in data)
        {
            var comp = _gridSquares[index].GetComponent<GridSquare>();
            if (comp.HasWrongValue() == false && comp.IsSelected() == false)
            {
                comp.SetSquareColor(color);
            }
        }
    }

    public void OnSquareSelected(int squareIndex)
    {
        foreach (var gridSquare in _gridSquares)
            gridSquare.GetComponent<GridSquare>().ResetBaseColor();

        var selected = _gridSquares[squareIndex].GetComponent<GridSquare>();

        if (selected.GetHasDefaultValue())
            return;

        var horizontalLine = LineIndicator.Instance.GetHorizontalLine(squareIndex);
        var verticalLine = LineIndicator.Instance.GetVerticalLine(squareIndex);
        var square = LineIndicator.Instance.GetSquare(squareIndex);

        SetSquaresColor(horizontalLine, _lineHighlightColor);
        SetSquaresColor(verticalLine, _lineHighlightColor);
        SetSquaresColor(square, _lineHighlightColor);
    }


    private void CheckBoardCompleted()
    {
        foreach (var square in _gridSquares)
        {
            var comp = square.GetComponent<GridSquare>();
            if (comp.IsCorrectNumberSet() == false)
            {
                return;
            }
        }

        GameEvents.OnBoardCompletedMethod();
    }

    public void SolveSudoku()
    {
        foreach (var square in _gridSquares)
        {
            var comp = square.GetComponent<GridSquare>();
            comp.SetCorrectNumber();
        }

        CheckBoardCompleted();
    }
}
