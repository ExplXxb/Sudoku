using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
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
        _button.interactable = canUndo;
    }

    public void OnClick()
    {
        GameEvents.OnUndoMethod();
    }
}