// ����, �� ������ �� ���������� �� ��������� ���� 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;

public class Clock : MonoBehaviour
{
    private int hours_ = 0; // ������
    private int minutes_ = 0; // �������
    private int seconds_ = 0; // �������

    private Text textClock; // �������� ���� ��� ����������� ��������� �� �����
    private float delta_time; // ������ �������� ��� � ��������
    private bool stop_clock_ = false; // �� ����� �������� ��������

    public static Clock Instance; // ��������� �����

    // �����, �� ����������� �� ��� ������������ ���������� ����� GameSettings (Instance)
    private void Awake()
    {
        if(Instance)
            Destroy(Instance);

        Instance = this;

        textClock = GetComponent<Text>();

        if (GameSettings.Instance.GetContinuePreviousGame())
            delta_time = Config.ReadGameTime();
        else
            delta_time = 0;
    }

    // �����, �� �����������, ���� ������� �������� ������������� ����� ������ �������� ����-����� ������ Update
    void Start()
    {
        stop_clock_ = false;
    }

    // �����, �� ����������� ������� �����, ���� �������� MonoBehaviour.
    void Update()
    {
        if(GameSettings.Instance.GetPaused() == false && stop_clock_ == false)
        {
            delta_time += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(delta_time);

            string hours = LeadingZero(span.Hours);
            string minutes = LeadingZero(span.Minutes);
            string seconds = LeadingZero(span.Seconds);

            textClock.text = hours + ":" + minutes + ":" + seconds;
        }
    }

    // ������ ������� ����, ���� ����� ����� 10
    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    // ����������� ��� ��������� ���, �� ���, ������ ������� ��������
    public void OnGameOver()
    {
        stop_clock_ = true; 
    }

    // ϳ��������� �� ���� ���������� ���
    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
    }

    // ³��������� �� ��䳿 ���������� ���
    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
    }

    // �������� �������� ��� � ������ �����
    public static string GetCurrentTime()
    {
        return Instance.delta_time.ToString();
    }

    // �������� �������� ����������� ��������� ����
    public Text GetCurrentTimeText()
    {
        return textClock;
    }

    // ��������� ��������
    public void StartClock()
    {
        stop_clock_ = false;
    }
}
