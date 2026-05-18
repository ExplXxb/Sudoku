using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text _number;
    [SerializeField] private int _value;
    [SerializeField] private Color _normalColor = Color.black;
    [SerializeField] private Color _inactiveColor = Color.gray;
    [SerializeField] private int _normalFontSize = 160;
    [SerializeField] private int _inactiveFontSize = 140;

    private bool _isNumberFirstMode = false;

    private void Awake()
    {
        _number.text = _value.ToString();
    }

    private void OnEnable()
    {
        GameEvents.OnInputModeChanged += OnInputModeChanged;
        GameEvents.OnNumberSelected += OnNumberSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnInputModeChanged -= OnInputModeChanged;
        GameEvents.OnNumberSelected -= OnNumberSelected;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvents.UpdateSquareNumberMethod(_value);
    }

    private void OnInputModeChanged(bool numberFirst)
    {
        _isNumberFirstMode = numberFirst;
        _number.color = numberFirst ? _inactiveColor : _normalColor;
        _number.fontStyle = FontStyle.Normal;
        _number.fontSize = numberFirst ? _inactiveFontSize : _normalFontSize;
    }

    private void OnNumberSelected(int number)
    {
        if (!_isNumberFirstMode)
            return;

        bool selected = number == _value;
        _number.color = selected ? _normalColor : _inactiveColor;
        _number.fontStyle = selected ? FontStyle.Bold : FontStyle.Normal;
        _number.fontSize = selected ? _normalFontSize : _inactiveFontSize;
    }
}