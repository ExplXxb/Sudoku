using UnityEngine;
using UnityEngine.UI;

public class InputModeButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _cellFirstSprite;
    [SerializeField] private Sprite _numberFirstSprite;

    private void OnEnable()
    {
        GameEvents.OnInputModeChanged += UpdateVisual;
    }

    private void OnDisable()
    {
        GameEvents.OnInputModeChanged -= UpdateVisual;
    }

    public void ToggleMode()
    {
        GameEvents.OnToggleInputModeMethod();
    }

    private void UpdateVisual(bool numberFirst)
    {
        _icon.sprite = numberFirst ? _numberFirstSprite : _cellFirstSprite;
    }
}