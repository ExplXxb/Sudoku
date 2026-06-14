using System;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    private const string SaveKey = "sudoku_save";

    [SerializeField] private Text _buttonText;
    [SerializeField] private Text _previousGameTime;
    [SerializeField] private Text _previousGameDifficulty;

    [SerializeField] private Color _enabledTextColor = Color.black;
    [SerializeField] private Color _disabledTextColor = Color.gray;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            Button button = GetComponent<Button>();

            button.interactable = false;

            _buttonText.color = _disabledTextColor;

            _previousGameTime.text = "";
            _previousGameDifficulty.text = "";
            return;
        }

        _buttonText.color = _enabledTextColor;

        SudokuSaveData data = SaveLoadData.Load<SudokuSaveData>(SaveKey);

        SetTime(data.time);
        SetDifficulty(GameSettings.Instance.GetDifficultyById(data.difficultyId).Name);
    }

    private void SetDifficulty(string difficulty)
    {
        _previousGameDifficulty.text = difficulty;
    }

    private void SetTime(float deltaTime)
    {
        TimeSpan span = TimeSpan.FromSeconds(deltaTime);

        string hours = LeadingZero(span.Hours);
        string minutes = LeadingZero(span.Minutes);
        string seconds = LeadingZero(span.Seconds);

        _previousGameTime.text = hours + ":" + minutes + ":" + seconds;
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
