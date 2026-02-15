// ����, �� ���� ��������� ����� ���� ���������� ���, ����������� �������� ��� ���
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Text textClock; // �������� ���� ��� ����������� ��������� ���� ��� �� �����

    // �����, �� �����������, ���� ������� �������� ������������� ����� ������ �������� ����-����� ������ Update
    void Start()
    {
        textClock.text = Clock.Instance.GetCurrentTimeText().text; // �������� ��� � ��������� ��������� 
    }                                                                                                  // ��� � �������� ���� � �������� ����
}
