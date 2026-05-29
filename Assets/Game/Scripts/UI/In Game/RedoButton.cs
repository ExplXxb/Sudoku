using UnityEngine;
using UnityEngine.UI;

public class RedoButton : MonoBehaviour
{
    private Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();

        GameEvents.OnHistoryStateChanged += UpdateState;
    }

    private void OnDisable()
    {
        GameEvents.OnHistoryStateChanged -= UpdateState;
    }

    private void UpdateState(bool canUndo, bool canRedo)
    {
        _button.interactable = canRedo;
    }

    public void OnClick()
    {
        GameEvents.OnRedoMethod();
    }
}