using UnityEngine;
using UnityEngine.UI;

public class ClockIconAnimator : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _frames;

    private readonly int[] _frameMap =
    {
        7, 3, 6, 8,
        9, 0, 4, 7,
        0, 1, 2, 3,
        4, 2, 5, 6
    };

    private void OnEnable()
    {
        GameEvents.OnSecondChanged += UpdateClock;
    }

    private void OnDisable()
    {
        GameEvents.OnSecondChanged -= UpdateClock;
    }

    private void UpdateClock(int seconds)
    {
        int frame = seconds % _frameMap.Length;
        int spriteIndex = _frameMap[frame];

        if (_frames == null || spriteIndex >= _frames.Length)
            return;

        _image.sprite = _frames[spriteIndex];
    }
}