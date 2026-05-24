using UnityEngine;
using UnityEngine.UI;

public class ContinueAfterGameOverButton : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPopup;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
        _button.interactable = true;
    }

    private void OnButtonClicked()
    {
        AdsManager.Instance.ShowRewarded(() =>
        {
            GameOverPopup.SetActive(false);
            ResetLives();
            Clock.Instance.StartTimer();
        });
    }

    public void ResetLives()
    {
        StatisticsManager.Instance.RemoveLostGame();
        LivesView.Instance.ResetLives();
    }
}