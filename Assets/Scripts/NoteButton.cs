using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteButton : Selectable, IPointerClickHandler
{
    [SerializeField] private Sprite _onImage;
    [SerializeField] private Sprite _offImage;

    private bool _active;

    private void Start()
    {
        _active = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _active = !_active;

        if (_active)
            GetComponent<Image>().sprite = _onImage;
        else
            GetComponent<Image>().sprite = _offImage;

        GameEvents.OnNotesActiveMethod(_active);
    }
}
