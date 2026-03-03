using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text _number;
    [SerializeField] private int _value; 

    private void Awake()
    {
        _number.text = _value.ToString(); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvents.UpdateSquareNumberMethod(_value);
    }
}
