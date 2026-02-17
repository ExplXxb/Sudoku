using System;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private Text _timeText;
    [SerializeField] private Text _difficultyText;

    private void Start()
    {
        if(Config.GameDataFileExist() == false)
        {
            gameObject.GetComponent<Button>().interactable = false;
            _timeText.text = " ";
            _difficultyText.text = " ";
        }
        else
        {
            float deltaTime = Config.ReadGameTime();
            deltaTime += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(deltaTime);

            string hours = LeadingZero(span.Hours);
            string minutes = LeadingZero(span.Minutes);
            string seconds = LeadingZero(span.Seconds);

            _timeText.text = hours + ":" + minutes + ":" + seconds;


            if (_difficultyText.text != null)
                _difficultyText.text = Config.ReadBoardDifficulty();
        }
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    public void SetGameData()
    {
        GameSettings.Instance.SetGameMode(Config.ReadBoardDifficulty());
    }
}
