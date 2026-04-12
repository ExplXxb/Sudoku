using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock : MonoBehaviour
{
    private int _hours = 0;
    private int _minutes = 0;
    private int _seconds = 0;

    private Text _textClock;
    private float _deltaTime;
    private bool _stopClock = false;

    public static Clock Instance { get; private set; }

    private void Awake()
    {
        if(Instance)
            Destroy(Instance);

        Instance = this;

        _textClock = GetComponent<Text>();
    }

    public float GetTime()
    {
        return _deltaTime;
    }

    public void SetTime(float time)
    {
        _deltaTime = time;
    }

    private void Start()
    {
        _stopClock = false;
    }

    private void Update()
    {
        if(GameSettings.Instance.GetPaused() == false && _stopClock == false)
        {
            _deltaTime += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(_deltaTime);

            string hours = LeadingZero(span.Hours);
            string minutes = LeadingZero(span.Minutes);
            string seconds = LeadingZero(span.Seconds);

            _textClock.text = hours + ":" + minutes + ":" + seconds;
        }
    }

    public void StopTimer()
    {
        _stopClock = true;
    }

    public static string GetCurrentTime()
    {
        return Instance._deltaTime.ToString();
    }

    public Text GetCurrentTimeText()
    {
        return _textClock;
    }

    public void StartTimer()
    {
        _stopClock = false;
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += StopTimer;
        GameEvents.OnBoardCompleted += StopTimer;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= StopTimer;
        GameEvents.OnBoardCompleted -= StopTimer;
    }
}
