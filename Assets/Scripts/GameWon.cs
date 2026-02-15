// ����, �� ���� ��������� ����� ���������� ������ ���, ����������� ���� �������� �� �������� ��� ���.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    public GameObject WinPopup; // ³��� ��������
    public Text ClockText; // �������� ���� ��� ����������� ���� ���

    // �����, �� �����������, ���� ������� �������� ������������� ����� ������ �������� ����-����� ������ Update
    void Start()
    {
        WinPopup.SetActive(false);
    }

    // �����, �� ����������� ��� ��������� ���
    private void OnBoardCompleted()
    {
        WinPopup.SetActive(true); // ������ ���� �������� �������� ��� ��������� ���
        ClockText.text = Clock.Instance.GetCurrentTimeText().text; // ��������� ����� ���� �� ����� ��������
    }

    // �����, �� ����������� ��� ��������� ��'����
    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += OnBoardCompleted; // ϳ������ �� ���� ���������� ���
    }

    // �����, �� ����������� ��� ����������� ��'����
    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= OnBoardCompleted; // ³������ �� ��䳿 ���������� ���
    }
}
