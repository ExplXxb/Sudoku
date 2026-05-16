using System.Collections.Generic;

public class SudokuCell
{
    public int Value { get; private set; }
    public int CorrectValue { get; private set; }
    public bool IsDefault { get; private set; }

    private bool[] _notes = new bool[9];

    public SudokuCell(int value, int correct, bool isDefault)
    {
        Value = value;
        CorrectValue = correct;
        IsDefault = isDefault;
    }

    public void SetValue(int value)
    {
        if (IsDefault) return;

        Value = value;

        if (value != 0)
            ClearNotes();
    }

    public void ForceSetValue(int value, bool[] notes)
    {
        Value = value;

        ClearNotes();

        if (notes != null)
        {
            for (int i = 0; i < notes.Length; i++)
            {
                if (notes[i])
                    _notes[i] = true;
            }
        }
    }

    public void Clear()
    {
        if (IsDefault) return;

        Value = 0;
        ClearNotes();
    }

    public bool IsCorrect()
    {
        return Value == CorrectValue;
    }

    public void AddNote(int n)
    {
        if (Value == 0) 
            _notes[n - 1] = true;
    }

    public void RemoveNote(int n)
    {
        _notes[n - 1] = false;
    }

    public bool HasNote(int n)
    {
        return _notes[n - 1];
    }

    public bool[] GetNotes()
    {
        return _notes;
    }

    public void ClearNotes()
    {
        for (int i = 0; i < _notes.Length; i++)
            _notes[i] = false;
    }
}