using UnityEngine.UI;
using UnityEngine;

public class StatisticsView : MonoBehaviour
{
    [SerializeField] private Text _gamesPlayedText;
    [SerializeField] private Text _gamesWonText;
    [SerializeField] private Text _gamesLostText;
    [SerializeField] private Text _gamesAbandonedText;

    [SerializeField] private Text _playTimeText;

    [SerializeField] private Text _hintsText;
    [SerializeField] private Text _mistakesText;

    [SerializeField] private Text _easyBestTimeText;
    [SerializeField] private Text _mediumBestTimeText;
    [SerializeField] private Text _hardBestTimeText;

    [SerializeField] private Button _debugResetButton;

    private void OnEnable()
    {
        _debugResetButton.onClick.AddListener(OnDebugReset);
        Refresh();
    }

    private void OnDisable()
    {
        _debugResetButton.onClick.RemoveListener(OnDebugReset);
    }

    private void OnDebugReset()
    {
        StatisticsManager.Instance.ResetStatistics();
        Refresh();
    }

    private void Refresh()
    {
        var data = StatisticsManager.Instance.Data;

        _gamesPlayedText.text = "Усього: " + data.GamesPlayed.ToString();
        _gamesWonText.text = "Перемог: " + data.GamesWon.ToString();
        _gamesLostText.text = "Програно: " + data.GamesLost.ToString();
        _gamesAbandonedText.text = "Покинуто: " + data.GamesAbandoned.ToString();

        _playTimeText.text = "Час за грою: " + FormatTime(data.TotalPlayTime);

        _hintsText.text = "Використано підказок: " + data.TotalHintsUsed.ToString();
        _mistakesText.text = "Кількість помилок: " + data.TotalMistakes.ToString();

        _easyBestTimeText.text = "Легко: " + FormatBestTime(data.Easy.BestTime);
        _mediumBestTimeText.text = "Помірно: " + FormatBestTime(data.Medium.BestTime);
        _hardBestTimeText.text = "Складно: " + FormatBestTime(data.Hard.BestTime);
    }

    private string FormatBestTime(float time)
    {
        if (time < 0)
            return "-";

        return FormatTime(time);
    }

    private string FormatTime(float time)
    {
        int totalSeconds = Mathf.FloorToInt(time);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return $"{minutes:00}:{seconds:00}";
    }
}