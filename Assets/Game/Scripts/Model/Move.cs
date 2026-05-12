[System.Serializable]
public class Move
{
    public int Row;
    public int Col;

    public int PreviousValue;
    public int NewValue;

    public bool[] PreviousNotes;
    public bool[] NewNotes;

    public Move Clone()
    {
        return new Move
        {
            Row = Row,
            Col = Col,
            PreviousValue = PreviousValue,
            NewValue = NewValue,
            PreviousNotes = (bool[])PreviousNotes?.Clone(),
            NewNotes = (bool[])NewNotes?.Clone()
        };
    }
}