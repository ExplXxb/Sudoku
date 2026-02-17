using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Text _textClock;

    void Start()
    {
        _textClock.text = Clock.Instance.GetCurrentTimeText().text;
    }                                                                                                  
}
