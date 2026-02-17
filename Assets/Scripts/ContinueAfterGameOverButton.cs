using UnityEngine;
using UnityEngine.UI;

public class ContinueAfterGameOverButton : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPopup;
    [SerializeField] private MenuButtons MenuButtons;

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
            MenuButtons.ContinueAfterGameOver();
            Clock.Instance.StartClock();
        });
    }
}

