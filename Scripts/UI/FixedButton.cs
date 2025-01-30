using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool Pressed { get; private set; }

    public delegate void ButtonPressed();
    public event ButtonPressed OnButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        OnButtonPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}
