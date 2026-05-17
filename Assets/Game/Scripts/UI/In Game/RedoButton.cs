using UnityEngine;

public class RedoButton : MonoBehaviour
{
    public void OnClick()
    {
        GameEvents.OnRedoMethod();
    }
}