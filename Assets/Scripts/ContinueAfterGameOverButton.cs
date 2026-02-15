using UnityEngine;
using UnityEngine.UI;

public class ContinueAfterGameOverButton : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private MenuButtons menuButtons;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked); // ��������� ��������� ��䳿 ���������� ������
        button.interactable = true; // ����� �� ������� � �������
    }

    private void OnButtonClicked()
    {
        AdsManager.Instance.ShowRewarded(() =>
        {
            gameOverPopup.SetActive(false);
            menuButtons.ContinueAfterGameOver();
            Clock.Instance.StartClock();
        });
    }
}

