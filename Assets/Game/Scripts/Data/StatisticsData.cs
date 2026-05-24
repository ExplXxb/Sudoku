[System.Serializable]
public class StatisticsData
{
    public int GamesPlayed => GamesWon + GamesLost + GamesAbandoned;
    public int GamesWon;
    public int GamesLost;
    public int GamesAbandoned;

    public float TotalPlayTime;

    public int TotalHintsUsed;
    public int TotalMistakes;

    public DifficultyStatistics Easy;
    public DifficultyStatistics Medium;
    public DifficultyStatistics Hard;
}