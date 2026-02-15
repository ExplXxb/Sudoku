// ����, ������� �� ���� ������� ����� ������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public List<GameObject> error_images; // ������ ��'���� (���������) �������
    public List<GameObject> health_points_images;
    public GameObject game_over_popup; // ��'��� (����) � ������������ ��� ���������� ���

    int lives_ = 0; // ������� ������� �����
    int error_number_ = 0; // ʳ������ �������
    public static Lives Instance; // ��������� ��������� ����� Lives ��� �������� �������

    // �����, �� ����������� �� ��� ������������ ���������� ����� GameSettings (Instance)
    private void Awake()
    {
        if(Instance)
            Destroy(Instance);

        Instance = this;
    }

    // �����, �� �����������, ���� ������� �������� ������������� ����� ������ �������� ����-����� ������ Update
    void Start()
    {
        lives_ = error_images.Count;
        error_number_ = 0;

        if (GameSettings.Instance.GetContinuePreviousGame())
        { // ���� ��������� ��� � ������ ���, �� ������� ������� ������� � �����
            error_number_ = Config.ErrorNumber();
            lives_ = error_images.Count - error_number_;

            for (int error = 0; error < error_number_; error++)
            {
                error_images[error].SetActive(true);
                health_points_images[error].SetActive(false);
            }
        }
    }

    // ������� ������� �������
    public int GetErrorNumbers()
    {
        return error_number_;
    }

    // �������� ��䳿 ����� ������������� �����
    private void WrongNumber()
    {
        if (error_number_ < error_images.Count)
        {
            error_images[error_number_].SetActive(true);
            health_points_images[error_number_].SetActive(false);
            error_number_++;
            lives_--;
        }

        CheckForGameOver(); // ���������� �� �������
    }

    // ��������, �� ���������� ����� � ������
    private void CheckForGameOver()
    {
        if(lives_ <= 0)
        {
            GameEvents.OnGameOverMethod();
            game_over_popup.SetActive(true);
        }
    }

    // �������� ��������� �����
    private void OnEnable()
    {
        GameEvents.OnWrongNumber += WrongNumber;
    }

    // �������� ����������� �����
    private void OnDisable()
    {
        GameEvents.OnWrongNumber -= WrongNumber;
    }

    // ����� ������� ������� �� ����������� �����
    public void ResetLives()
    {
        foreach (var error in error_images)
        {
            error.SetActive(false);
        }

        foreach (var hp in health_points_images)
        {
            hp.SetActive(true);
        }

        error_number_ = 0;
        lives_ = error_images.Count;
    }
}
