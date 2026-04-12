using UnityEngine;
using UnityEngine.UI;

public class GameWonMenu : MonoBehaviour
{
    [SerializeField] private GameObject _winPopup;
    [SerializeField] private Text _time;

    private void Start()
    {
        Hide();
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += Show;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= Show;
    }

    private void Show()
    {
        _winPopup.SetActive(true);
        SetTime();
    }

    private void Hide()
    {
        _winPopup.SetActive(false);
    }

    private void SetTime()
    {
        _time.text = Clock.Instance.GetCurrentTimeText().text;
    }
}
