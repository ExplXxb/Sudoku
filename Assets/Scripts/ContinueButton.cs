// ����, �� ���� ��������� ����� ������ "���������� ���" (���� �������� ��������� ����� ��� ��� � ��������� ������ ���)
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    public Text timeText; // �������� ���� ��� ����������� ���� �� �����
    public Text difficultyText; // �������� ���� ��� ����������� ���� �� �����

    // ������ ������� ����, ���� ����� ����� 10
    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    // �����, �� �����������, ���� ������� �������� ������������� ����� ������ �������� ����-����� ������ Update
    void Start()
    {
        if(Config.GameDataFileExist() == false)
        {
            gameObject.GetComponent<Button>().interactable = false;
            timeText.text = " ";
            difficultyText.text = " ";
        }
        else
        {
            float delta_time = Config.ReadGameTime();
            delta_time += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(delta_time);

            string hours = LeadingZero(span.Hours);
            string minutes = LeadingZero(span.Minutes);
            string seconds = LeadingZero(span.Seconds);

            timeText.text = hours + ":" + minutes + ":" + seconds;


            if (difficultyText.text != null)
                difficultyText.text = Config.ReadBoardDifficulty();
        }
    }

    // ���������� ��� ��� ��� ����������� ���
    public void SetGameData()
    {
        GameSettings.Instance.SetGameMode(Config.ReadBoardDifficulty());
    }
}
