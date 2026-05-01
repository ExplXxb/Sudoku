using System;
using UnityEngine;

public class GameEvents
{
    public static event Action OnCheckBoardCompleted;

    public static void CheckBoardCompletedMethod()
    {
        OnCheckBoardCompleted?.Invoke();
    }

    public static event Action<int> OnUpdateSquareNumber;

    public static void UpdateSquareNumberMethod(int number)
    {
        OnUpdateSquareNumber?.Invoke(number);
    }

    public static event Action<int> OnSquareSelected;

    public static void SquareSelectedMethod(int squareIndex)
    {
        OnSquareSelected?.Invoke(squareIndex);
    }

    public static event Action OnWrongNumber;

    public static void OnWrongNumberMethod()
    {
        OnWrongNumber?.Invoke();
    }

    public static event Action OnGameOver;

    public static void OnGameOverMethod()
    {
        OnGameOver?.Invoke();
    }

    public static event Action<bool> OnNotesActive;

    public static void OnNotesActiveMethod(bool active)
    {
        OnNotesActive?.Invoke(active);
    }

    public static event Action OnClearCell;

    public static void OnClearCellMethod()
    {
        OnClearCell?.Invoke();
    }

    public static event Action OnBoardCompleted;

    public static void OnBoardCompletedMethod()
    {
        OnBoardCompleted?.Invoke();
    }

    public static event Action OnGiveAHint;

    public static void OnGiveAHintMethod()
    {
        OnGiveAHint?.Invoke();
    }

    public static event Action OnGiveAHintOpening;

    public static void OnGiveAHintOpeningMethod()
    {
        OnGiveAHintOpening?.Invoke();
    }

    public static event Action OnExitToMenu;

    public static void OnExitToMenuMethod()
    {
        OnExitToMenu?.Invoke();
    }
}