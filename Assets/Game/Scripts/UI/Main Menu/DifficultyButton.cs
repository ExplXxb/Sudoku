using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyButton : MonoBehaviour
{
    [SerializeField] private DifficultyData _difficulty;
    [SerializeField] private string _gameScene = "GameScene";

    public void SelectDifficulty()
    {
        GameSettings.Instance.SetDifficulty(_difficulty);
        SceneManager.LoadScene(_gameScene);
    }
}