using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RubberButton : Selectable, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvents.OnClearCellMethod();
    }
}
