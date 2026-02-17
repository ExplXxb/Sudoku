using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    [SerializeField] private GameObject _winPopup;
    [SerializeField] private Text _clockText;

    private void Start()
    {
        _winPopup.SetActive(false);
    }

    private void OnBoardCompleted()
    {
        _winPopup.SetActive(true);
        _clockText.text = Clock.Instance.GetCurrentTimeText().text;
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += OnBoardCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= OnBoardCompleted;
    }
}
