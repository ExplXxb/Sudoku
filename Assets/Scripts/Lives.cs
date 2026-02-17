using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    [SerializeField] private List<GameObject> _errorImages;
    [SerializeField] private List<GameObject> _healthPointsImages;
    [SerializeField] private GameObject _gameOverPopup;

    private int _lives = 0;
    private int _errorNumber = 0;
    public static Lives Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    private void Start()
    {
        _lives = _errorImages.Count;
        _errorNumber = 0;

        if (GameSettings.Instance.GetContinuePreviousGame())
        {
            _errorNumber = Config.ErrorNumber();
            _lives = _errorImages.Count - _errorNumber;

            for (int error = 0; error < _errorNumber; error++)
            {
                _errorImages[error].SetActive(true);
                _healthPointsImages[error].SetActive(false);
            }
        }
    }

    public int GetErrorNumbers()
    {
        return _errorNumber;
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

        _errorNumber = 0;
        _lives = _errorImages.Count;
    }

    private void WrongNumber()
    {
        if (_errorNumber < _errorImages.Count)
        {
            _errorImages[_errorNumber].SetActive(true);
            _healthPointsImages[_errorNumber].SetActive(false);
            _errorNumber++;
            _lives--;
        }

        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if(_lives <= 0)
        {
            GameEvents.OnGameOverMethod();
            _gameOverPopup.SetActive(true);
        }
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
