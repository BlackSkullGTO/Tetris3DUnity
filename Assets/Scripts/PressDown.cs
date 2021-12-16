using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public bool buttonPressed = false;

    public void OnPointerDown(PointerEventData e)
    {
        buttonPressed = true;
    }
    public void OnPointerUp(PointerEventData e)
    {
        buttonPressed = false;
    }

}
