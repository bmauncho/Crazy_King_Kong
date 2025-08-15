using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;


public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public  bool isPointerDown = false;
    public bool isPersistentGlow;
    public GameObject ButtonGlow;

    public UnityEvent onClick;

    public void OnPointerDown ( PointerEventData eventData )
    {
        isPointerDown = true;
        if(ButtonGlow != null)
        {
            if(isPersistentGlow)
            {
                if(ButtonGlow.activeSelf)
                {
                    ButtonGlow.SetActive(false);
                }
                else
                {
                    ButtonGlow.SetActive(true);
                }
            }
            else
            {
                ButtonGlow.SetActive(true);
            }
               
        }
    }

    public void OnPointerUp ( PointerEventData eventData )
    {
        if (ButtonGlow != null)
        {
            if (!isPersistentGlow)
            {
                ButtonGlow.SetActive(false);
            }
        }
        onClick?.Invoke();
        isPointerDown = false;
    } 
}
