using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPopup;
    [SerializeField] private Text _time;

    private void Start()
    {
        Hide();
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += Show;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= Show;
    }

    private void Show()
    {
        _gameOverPopup.SetActive(true);
        SetTime();
    }

    private void Hide()
    {
        _gameOverPopup.SetActive(false);
    }

    private void SetTime()
    {
        _time.text = Clock.Instance.GetCurrentTimeText().text;
    }
}
