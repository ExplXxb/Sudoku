// ����, �� ���� ��������� ����� ������ "����� ������� �� �������� �������", ���������� ������������ �������� ������� �� ��� ���
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))] // ��������� �������� ���������� Button �� ����� ��'���
public class GiveAHintButton : MonoBehaviour
{
    private Button button; // ��������� �� ������

    // �����, �� �����������, ���� ������� �������� ������������� ����� ������ �������� ����-����� ������ Update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked); // ��������� ��������� ��䳿 ���������� ������
        button.interactable = true; // ����� �� ������� � �������
    }

    // �����, �� ����������� ��� ��������� �� ������
    private void OnButtonClicked()
    {
        AdsManager.Instance.ShowRewarded(GameEvents.OnGiveAHintMethod); // ��������� ����� ��� ������� �������
    }
}
