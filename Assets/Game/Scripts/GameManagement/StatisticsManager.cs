using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    private const string SaveKey = "statistics";

    public static StatisticsManager Instance { get; private set; }

    private StatisticsData _data;

    public StatisticsData Data => _data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Load()
    {
        if (SaveLoadData.Exists(SaveKey))
        {
            _data = SaveLoadData.Load<StatisticsData>(SaveKey);
        }
        else
        {
            _data = new StatisticsData();

            _data.Easy = new DifficultyStatistics();
            _data.Medium = new DifficultyStatistics();
            _data.Hard = new DifficultyStatistics();

            Save();
        }
    }

    public void Save()
    {
        SaveLoadData.Save(SaveKey, _data);
    }

    public void ResetStatistics()
    {
        _data = new StatisticsData();
        _data.Easy = new DifficultyStatistics();
        _data.Medium = new DifficultyStatistics();
        _data.Hard = new DifficultyStatistics();
        Save();
    }

    public void AddLostGame()
    {
        _data.GamesLost++;
        Save();
    }

    public void RemoveLostGame()
    {
        _data.GamesLost--;
        Save();
    }

    public void AddAbandonedGame()
    {
        _data.GamesAbandoned++;

        Save();
    }

    public void AddWonGame(float time, string difficultyName)
    {
        _data.GamesWon++;

        var difficultyStats = GetDifficultyStats(difficultyName);

        difficultyStats.Wins++;

        if (difficultyStats.BestTime < 0 || time < difficultyStats.BestTime)
        {
            difficultyStats.BestTime = time;
        }

        Save();
    }

    public void AddPlayTime(float time)
    {
        _data.TotalPlayTime += time;
        Save();
    }

    public void AddHintUsed()
    {
        _data.TotalHintsUsed++;
        Save();
    }

    public void AddMistake()
    {
        _data.TotalMistakes++;
        Save();
    }

    private DifficultyStatistics GetDifficultyStats(string difficultyName)
    {
        switch (difficultyName)
        {
            case "Easy":
                return _data.Easy;

            case "Medium":
                return _data.Medium;

            case "Hard":
                return _data.Hard;

            default:
                return _data.Easy;
        }
    }
}