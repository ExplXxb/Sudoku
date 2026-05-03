using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pausePopup;
    [SerializeField] private Text _time;

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        _pausePopup.SetActive(true);
        SetTime();
    }

    public void Hide()
    {
        _pausePopup.SetActive(false);
    }

    private void SetTime()
    {
        _time.text = Clock.Instance.GetCurrentTimeText().text;
    }
}
