using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Text _timeText;

    public void DisplayTime()
    {
        _timeText.text = Clock.Instance.GetCurrentTimeText().text;
    }
}
