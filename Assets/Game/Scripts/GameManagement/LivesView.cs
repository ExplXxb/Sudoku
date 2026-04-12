using System.Collections.Generic;
using UnityEngine;

public class LivesView : MonoBehaviour
{
    public static LivesView Instance { get; private set; }

    [SerializeField] private List<GameObject> _errorImages;
    [SerializeField] private List<GameObject> _healthPointsImages;

    public const int MaxLivesCount = 3;
    public int LivesCount { get; private set; }
    public int ErrorCount => MaxLivesCount - LivesCount;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetLives(int lives)
    {
        LivesCount = Mathf.Clamp(lives, 0, MaxLivesCount);

        for (int i = 0; i < MaxLivesCount; i++)
        {
            bool isAlive = i < LivesCount;

            _healthPointsImages[i].SetActive(isAlive);
            _errorImages[i].SetActive(!isAlive);
        }
    }

    public void ResetLives()
    {
        foreach (var error in _errorImages)
        {
            error.SetActive(false);
        }

        foreach (var hp in _healthPointsImages)
        {
            hp.SetActive(true);
        }

        LivesCount = MaxLivesCount;
    }

    private void WrongNumber()
    {
        if (LivesCount <= 0)
            throw new System.InvalidOperationException();

        SetLives(LivesCount - 1);

        if (LivesCount <= 0)
            GameEvents.OnGameOverMethod();
    }

    private void OnEnable()
    {
        GameEvents.OnWrongNumber += WrongNumber;
    }

    private void OnDisable()
    {
        GameEvents.OnWrongNumber -= WrongNumber;
    }
}
