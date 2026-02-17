using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberButton : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private int _value = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvents.UpdateSquareNumberMethod(_value);
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }
}
