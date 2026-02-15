// ����, �� ���� ��������� ����� ���� �����, ����������� �������� ��� ���
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Text time_text; // �������� ���� ��� ����������� ��������� ���� ��� �� �����

    // ³��������� ���
    public void DisplayTime()
    {
        time_text.text = Clock.Instance.GetCurrentTimeText().text; // ������������ ����� ���� ���� �� �������� ���
    }
}
