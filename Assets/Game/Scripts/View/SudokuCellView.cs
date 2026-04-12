using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SudokuCellView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text _numberText;
    [SerializeField] private Text[] _noteTexts;
    [SerializeField] private Image _background;

    [Header("Borders")]
    [SerializeField] private RectTransform _topBorder;
    [SerializeField] private RectTransform _bottomBorder;
    [SerializeField] private RectTransform _leftBorder;
    [SerializeField] private RectTransform _rightBorder;

    [Header("Colors")]
    [SerializeField] private Color _defaultBackgroundColor = Color.yellow;

    [SerializeField] private Color _defaultValueForegroundColor = Color.gray;

    [SerializeField] private Color _correctValueForegroundColor = Color.black;

    [SerializeField] private Color _selectedCellForegroundColor = Color.yellow;
    [SerializeField] private Color _selectedCellBackgroundColor = Color.lightGreen;

    [SerializeField] private Color _highlightedCellBackgroundColor = Color.lightSeaGreen;
    /// <summary>
    /// ////////////////////////////////////////////////// ЗРОБИТИ СТУКТУРОЮ + МОЖНА ВИНЕСТИ В СКРІПТЕБЛ ОБЖЕКТ
    /// </summary>
    [SerializeField] private Color _wrongValueForegroundColor = Color.red;

    private SudokuCell _cell;
    private int _index;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvents.SquareSelectedMethod(_index);
    }

    public void Initialize(int index, SudokuCell cell)
    {
        _index = index;
        _cell = cell;

        Refresh();
    }

    public void Refresh()
    {
        SetNumber(_cell.Value);
        SetNotes(_cell.GetNotes());

        UpdateColors(false, false);
    }

    public void SetNumber(int number)
    {
        _numberText.text = number == 0 ? "" : number.ToString();
    }

    public void SetNotes(IEnumerable<bool> notes)
    {
        int i = 0;
        foreach (var show in notes)
        {
            _noteTexts[i].gameObject.SetActive(show);

            if (show)
                _noteTexts[i].text = (i + 1).ToString();

            i++;
        }
    }

    public void SetBorders(float top, float bottom, float left, float right)
    {
        SetBorder(_topBorder, top, true);
        SetBorder(_bottomBorder, bottom, true);
        SetBorder(_leftBorder, left, false);
        SetBorder(_rightBorder, right, false);
    }

    public void SetSelected(bool selected, bool highlighted)
    {
        UpdateColors(selected, highlighted);
    }

    private void UpdateColors(bool isSelected, bool isHighlighted)
    {
        _background.color = _defaultBackgroundColor;

        _numberText.color = _cell.IsCorrect() ? _correctValueForegroundColor : _wrongValueForegroundColor;

        if (_cell.IsDefault)
            _numberText.color = _defaultValueForegroundColor;

        if (isHighlighted)
            _background.color = _highlightedCellBackgroundColor;

        if (isSelected)
            _background.color = _selectedCellBackgroundColor;

    }

    private void SetBorder(RectTransform border, float size, bool horizontal)
    {
        if (horizontal)
        {
            border.sizeDelta = new Vector2(0, size);

            border.offsetMin = new Vector2(-size / 2, border.offsetMin.y);
            border.offsetMax = new Vector2(size / 2, border.offsetMax.y);
        }
        else
        {
            border.sizeDelta = new Vector2(size, 0);

            border.offsetMin = new Vector2(border.offsetMin.x, -size / 2);
            border.offsetMax = new Vector2(border.offsetMax.x, size / 2);
        }
    }
}