using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _numberText;
    [SerializeField] private List<GameObject> _numberNotes;

    [SerializeField] private Color _defaultColor = new Color(0.85f, 0.85f, 0.85f);
    [SerializeField] private Color _normalColor = Color.white;

    private int _number = 0;
    private int _correctNumber = 0;
    private int _squareIndex = -1;

    private bool _selected = false;
    private bool _hasDefaultValue = false;
    private bool _hasWrongValue = false;
    private bool _isSolved = false;
    private bool _noteActive;

    public int GetSquareNumber() { return _number; }
    public bool IsCorrectNumberSet() { return _number == _correctNumber; }
    public bool HasWrongValue() { return _hasWrongValue; }
    public void SetHasDefaultValue(bool has_default) { _hasDefaultValue = has_default; }
    public bool GetHasDefaultValue() { return _hasDefaultValue; }
    public bool IsSelected() { return _selected; }
    public void SetSquareIndex(int index) { _squareIndex = index; }

    private void Start()
    {
        _selected = false;
        _noteActive = false;

        if (GameSettings.Instance.GetContinuePreviousGame() == false)
            SetNoteNumberValue(0);
        else
            SetClearEmptyNotes();
    }

    public void SetCorrectNumber(int number)
    {
        _correctNumber = number;
        _hasWrongValue = false;

        if (_number != 0 && _number != _correctNumber)
        {
            _hasWrongValue = true;
            SetSquareColor(Color.red);
        }
    }

    public void SetCorrectNumber()
    {
        _number = _correctNumber;
        SetNoteNumberValue(0);
        DisplayText();
    }

    public List<string> GetSquareNotes()
    {
        List<string> notes = new List<string>();

        foreach (var number in _numberNotes)
        {
            notes.Add(number.GetComponent<Text>().text);
        }

        return notes;
    }

    public void SetGridNotes(List<int> notes)
    {
        foreach (var note in notes)
        {
            SetNotesSingleNumberValue(note, true);
        }
    }

    public void OnNotesActive(bool active)
    {
        if (_isSolved) return;
        _noteActive = active;
    }

    public void DisplayText()
    {
        var text = _numberText.GetComponent<Text>();

        if (_number <= 0)
            text.text = " ";
        else
            text.text = _number.ToString();

        var colors = this.colors;

        if (_hasDefaultValue)
        {
            text.fontStyle = FontStyle.Bold;
            colors.normalColor = _defaultColor;
        }
        else
        {
            text.fontStyle = FontStyle.Normal;
            colors.normalColor = Color.white;
        }

        this.colors = colors;
    }

    public void SetNumber(int number)
    {
        _number = number;
        DisplayText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _selected = true;
        GameEvents.SquareSelectedMethod(_squareIndex);
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }

    public void OnClearNumber()
    {
        if (_selected && !_hasDefaultValue && !_isSolved)
        {
            _number = 0;
            _hasWrongValue = false;
            SetSquareColor(Color.white);
            SetNoteNumberValue(0);
            DisplayText();
        }
    }

    public void SetCorrectValueOnHint()
    {
        SetSquareNumber(_correctNumber);
    }

    public void OnSetNumber(int number)
    {
        if (_selected && _hasDefaultValue == false && _isSolved == false)
        {
            SetSquareNumber(number);
        }
    }

    public void OnSquareSelected(int square_index)
    {
        if (_squareIndex != square_index)
        {
            _selected = false;
        }
    }

    public void SetSquareColor(Color color)
    {
        var colors = this.colors;
        colors.normalColor = color;
        this.colors = colors;
    }

    public void ResetBaseColor()
    {
        if (_hasWrongValue)
        {
            SetSquareColor(Color.red);
            return;
        }

        if (_hasDefaultValue)
            SetSquareColor(_defaultColor);
        else
            SetSquareColor(_normalColor);
    }

    private void SetClearEmptyNotes()
    {
        foreach (var number in _numberNotes)
        {
            if (number.GetComponent<Text>().text == "0")
                number.GetComponent<Text>().text = " ";
        }
    }

    private void SetNoteNumberValue(int value)
    {
        foreach (var number in _numberNotes)
        {
            if (value <= 0)
                number.GetComponent<Text>().text = " ";
            else
                number.GetComponent<Text>().text = value.ToString();
        }
    }

    private void SetNotesSingleNumberValue(int value, bool forceUpdate = false)
    {
        if (_noteActive == false && forceUpdate == false)
            return;

        if (value <= 0)
            _numberNotes[value - 1].GetComponent<Text>().text = " ";
        else
        {
            if (_numberNotes[value - 1].GetComponent<Text>().text == " " || forceUpdate)
                _numberNotes[value - 1].GetComponent<Text>().text = value.ToString();
            else
                _numberNotes[value - 1].GetComponent<Text>().text = " ";
        }
    }

    private void OnEnable()
    {
        GameEvents.OnUpdateSquareNumber += OnSetNumber;
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnNotesActive += OnNotesActive;
        GameEvents.OnClearNumber += OnClearNumber;
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnNotesActive -= OnNotesActive;
        GameEvents.OnClearNumber -= OnClearNumber;
        GameEvents.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        if (_number != 0 && _number != _correctNumber)
        {
            _hasDefaultValue = false;
            SetSquareColor(Color.white);
            _number = 0;
            DisplayText();
        }
        SetSquareColor(Color.white);
    }

    private void SetSquareNumber(int number)
    {
        if (_noteActive == true && _hasWrongValue == false)
        {
            SetNotesSingleNumberValue(number);
        }
        else if (_noteActive == false)
        {
            SetNoteNumberValue(0);
            SetNumber(number);

            if (_number != _correctNumber)
            {
                _hasWrongValue = true;
                var colors = this.colors;
                colors.normalColor = Color.red;
                this.colors = colors;

                GameEvents.OnWrongNumberMethod();
            }
            else
            {
                _hasWrongValue = false;
                _isSolved = true; 
                _noteActive = false; 
                ResetBaseColor();
            }

        }
        GameEvents.CheckBoardCompletedMethod();
    }
}
