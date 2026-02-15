// ����, �� ����������� ������� ���� � �� ������. ³� ������� �� ����������� ����� � ����� ������� ����,
// ����������� �� ��������� ���������, ������� ���� ���� ����� �� ����� ����.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static GameEvents;
using Unity.VisualScripting;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject number_text; // ��'��� (�������� ����) ��� ����������� ����� � ������� �� �����
    public List<GameObject> number_notes; // ������ ��'���� (��������� ����) ��� ����������� ������� � ������� (������)
    private bool note_active_; // �� ���������� ����� ������� � ������� (������)
    private int number_ = 0; // �����, ��� �������� � ������� (������)
    private int correct_number_ = 0; // ��������� ����� ��� ���� ������� (������)

    private bool selected_ = false; // �� ������ �� ������� (������)
    private int square_index_ = -1; // ������ �������(������) � ����
    private bool has_default_value_ = false; // �� � � �������(������) �������� �� �������������
    private bool has_wrong_value_ = false; // �� �� �������(������) ����������� ��������
    private bool is_solved_ = false;

    public int GetSquareNumber() { return number_; }// �������� ����� � �������
    public bool IsCorrectNumberSet() { return number_ == correct_number_; } // ���������, �� ����������� ��������� ����� � ������� (������)
    public bool HasWrongValue() { return has_wrong_value_; } // ���������, �� � � �������(������) ����������� ��������
    public void SetHasDefaultValue(bool has_default) { has_default_value_ = has_default; } // ���������� �������� �������� �� ������������� � ������� (������)
    public bool GetHasDefaultValue() { return has_default_value_; } // �������� �������� �������� �� ������������� � ������� (������)
    public bool IsSelected() { return selected_; } // ���������, �� �������(������) ������
    public void SetSquareIndex(int index) { square_index_ = index; } // ���������� ������ �������(������) � ����

    // ���������� ��������� ����� ��� ������
    public void SetCorrectNumber(int number)
    {
        correct_number_ = number;
        has_wrong_value_ = false;

        if (number_ != 0 && number_ != correct_number_)
        {
            has_wrong_value_ = true;
            SetSquareColor(Color.red);
        }
    }

    // ���������� ����� � ������� �� ��������� �����
    public void SetCorrectNumber() // ������������� ����� ������� SetCorrectNumber(), �� ����������� ����
    {
        number_ = correct_number_;
        SetNoteNumberValue(0);
        DisplayText();
    }

    // �����, �� �����������, ���� ������� �������� ������������� ����� ������ �������� ����-����� ������ Update
    void Start()
    {
        selected_ = false;
        note_active_ = false;

        // ���������� � ����������� ������ ���
        if (GameSettings.Instance.GetContinuePreviousGame() == false)
            SetNoteNumberValue(0); // ͳ - ��������� ���� ���� � ����������� ���������� � �������� (�������)
        else
            SetClearEmptyNotes(); // ��� - ��������� ���� ����� ��������� ���������� � �������� (�������)
    }

    // �������� ������ �����, �� ������������� ������� ������� � ��
    public List<string> GetSquareNotes()
    {
        List<string> notes = new List<string>();

        foreach (var number in number_notes)
        {
            notes.Add(number.GetComponent<Text>().text);
        }

        return notes;
    }

    // ���������� �������� �� ������������� ��� ������� � �����
    private void SetClearEmptyNotes()
    {
        foreach (var number in number_notes)
        {
            if (number.GetComponent<Text>().text == "0")
                number.GetComponent<Text>().text = " ";
        }
    }

    // ���������� �������� ��� ������� � ������� (������)
    private void SetNoteNumberValue(int value)
    {
        foreach (var number in number_notes)
        {
            if (value <= 0)  // �������� �� ����� ����� ��� ��� ���� �� �������
                number.GetComponent<Text>().text = " ";
            else
                number.GetComponent<Text>().text = value.ToString();
        }
    }

    // ���������� ���� �������� ��� ������� � ������� (������)
    private void SetNotesSingleNumberValue(int value, bool force_update = false)
    {
        if (note_active_ == false && force_update == false)
            return;

        if (value <= 0) // �������� �� ����� ����� ��� ��� ���� �� �������
            number_notes[value - 1].GetComponent<Text>().text = " ";
        else
        {
            if (number_notes[value - 1].GetComponent<Text>().text == " " || force_update)
                number_notes[value - 1].GetComponent<Text>().text = value.ToString();
            else
                number_notes[value - 1].GetComponent<Text>().text = " ";
        }
    }

    // ���������� ������� ��� ������� (������)
    public void SetGridNotes(List<int> notes)
    {
        foreach (var note in notes)
        {
            SetNotesSingleNumberValue(note, true);
        }
    }

    // ���������� ��� �������� ����� ������� � ������� (������)
    public void OnNotesActive(bool active)
    {
        if (is_solved_) return;
        note_active_ = active;
    }

    // ³���������� ����� � ������� (������)
    public void DisplayText()
    {
        var text = number_text.GetComponent<Text>();

        if (number_ <= 0)
            text.text = " ";
        else
            text.text = number_.ToString();

        var colors = this.colors;

        if (has_default_value_)
        {
            text.fontStyle = FontStyle.Bold;
            colors.normalColor = defaultColor;
        }
        else
        {
            text.fontStyle = FontStyle.Normal;
            colors.normalColor = Color.white;
        }

        this.colors = colors;
    }


    // ���������� �������� ����� � ������� (������)
    public void SetNumber(int number)
    {
        number_ = number;
        DisplayText();
    }

    // ������� ���� �����
    public void OnPointerClick(PointerEventData eventData)
    {
        selected_ = true;
        GameEvents.SquareSelectedMethod(square_index_);
    }

    // ������� ���� �������
    public void OnSubmit(BaseEventData eventData)
    {

    }

    // ������� ��䳿 �������� ������� (������)
    private void OnEnable()
    {
        GameEvents.OnUpdateSquareNumber += OnSetNumber;
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnNotesActive += OnNotesActive;
        GameEvents.OnClearNumber += OnClearNumber;
        GameEvents.OnGameOver += OnGameOver;
    }

    // ������� ��䳿 ��������� ������� (������)
    private void OnDisable()
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnNotesActive -= OnNotesActive;
        GameEvents.OnClearNumber -= OnClearNumber;
        GameEvents.OnGameOver -= OnGameOver;
    }

    // ��������� ���
    private void OnGameOver()
    {
        if (number_ != 0 && number_ != correct_number_)
        {
            has_default_value_ = false;
            SetSquareColor(Color.white);
            number_ = 0;
            DisplayText();
        }
        SetSquareColor(Color.white);
    }

    // �������� �������� ����� � ����� (������)
    public void OnClearNumber()
    {
        if (selected_ && !has_default_value_ && !is_solved_)
        {
            number_ = 0;
            has_wrong_value_ = false;
            SetSquareColor(Color.white);
            SetNoteNumberValue(0);
            DisplayText();
        }
    }

    // ���������� ��������� �������� � �������(������) ��� ���������� �������
    public void SetCorrectValueOnHint()
    {
        SetSquareNumber(correct_number_);
    }

    // ���������� ����� � �������(������) ��� ��䳿 ��������� �����
    public void OnSetNumber(int number)
    {
        if (selected_ && has_default_value_ == false && is_solved_ == false)
        {
            SetSquareNumber(number);
        }
    }

    // ���������� ����� � ������� (������)
    private void SetSquareNumber(int number)
    {
        if (note_active_ == true && has_wrong_value_ == false)
        {
            SetNotesSingleNumberValue(number);
        }
        else if (note_active_ == false)
        {
            SetNoteNumberValue(0);
            SetNumber(number);

            if (number_ != correct_number_)
            {
                has_wrong_value_ = true;
                var colors = this.colors;
                colors.normalColor = Color.red;
                this.colors = colors;

                GameEvents.OnWrongNumberMethod();
            }
            else
            {
                has_wrong_value_ = false;
                is_solved_ = true; 
                note_active_ = false; 
                ResetBaseColor();
            }

        }
        GameEvents.CheckBoardCompletedMethod();
    }

    // ������� ��䳿 ������� ������� (������)
    public void OnSquareSelected(int square_index)
    {
        if (square_index_ != square_index)
        {
            selected_ = false;
        }
    }

    // ���������� ���� ������� (������)
    public void SetSquareColor(Color color)
    {
        var colors = this.colors;
        colors.normalColor = color;
        this.colors = colors;
    }

    [SerializeField] private Color defaultColor = new Color(0.85f, 0.85f, 0.85f);
    [SerializeField] private Color normalColor = Color.white;

    public void ResetBaseColor()
    {
        if (has_wrong_value_)
        {
            SetSquareColor(Color.red);
            return;
        }

        if (has_default_value_)
            SetSquareColor(defaultColor);
        else
            SetSquareColor(normalColor);
    }

}
