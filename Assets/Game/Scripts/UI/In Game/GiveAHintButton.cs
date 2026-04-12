using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GiveAHintButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
        _button.interactable = true;
    }

    private void OnButtonClicked()
    {
        AdsManager.Instance.ShowRewarded(GameEvents.OnGiveAHintMethod);
    }
}
