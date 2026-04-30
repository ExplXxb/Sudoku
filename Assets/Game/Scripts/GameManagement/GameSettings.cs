using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    private DifficultyData _difficulty;
    private bool _continuePreviousGame = false;
    private bool _paused = false;

    private void Awake()
    {
        _paused = false;

        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetDifficulty(DifficultyData difficulty)
    {
        _difficulty = difficulty;
    }

    public DifficultyData GetDifficulty()
    {
        return _difficulty;
    }

    public void SetContinuePreviousGame(bool continueGame)
    {
        _continuePreviousGame = continueGame;
    }

    public bool GetContinuePreviousGame()
    {
        return _continuePreviousGame;
    }

    public void SetPaused(bool paused)
    {
        _paused = paused;
    }

    public bool GetPaused()
    {
        return _paused;
    }
}
