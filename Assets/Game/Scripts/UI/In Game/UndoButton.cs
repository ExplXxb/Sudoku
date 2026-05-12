using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public void OnClick()
    {
        GameEvents.OnUndoMethod();
    }
}