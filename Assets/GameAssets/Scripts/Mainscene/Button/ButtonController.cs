using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;


public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public  bool isPointerDown = false;
    public bool isPersistentGlow;
    public bool isInteractable = true;
    private bool lastInteractableValue;

    public Sprite baseSprite;
    public Sprite normalsprite;

    public GameObject ButtonNormal;
    public GameObject ButtonGlow;

    public UnityEvent onClick;

    private Image buttonNormalImage;

    private void Awake ()
    {
        if (ButtonNormal != null)
            buttonNormalImage = ButtonNormal.GetComponent<Image>();

        HandleInteractableChanged(isInteractable); // Initialize visuals
    }

    private void Update ()
    {
        if (lastInteractableValue != isInteractable)
        {
            HandleInteractableChanged(isInteractable);
            lastInteractableValue = isInteractable;
        }
    }

    public void OnPointerDown ( PointerEventData eventData )
    {
        if(!isInteractable)
        {
            return; // Ignore pointer down if not interactable
        }
        isPointerDown = true;
        if(ButtonGlow != null)
        {
            if(isPersistentGlow)
            {
                if(ButtonGlow.activeSelf)
                {
                    ButtonGlow.SetActive(false);
                    ButtonNormal.SetActive(true);
                }
                else
                {
                    ButtonGlow.SetActive(true);
                    ButtonNormal.SetActive(false);
                }
            }
            else
            {
                ButtonGlow.SetActive(true);
                ButtonNormal.SetActive(false);
            }
               
        }
    }

    public void OnPointerUp ( PointerEventData eventData )
    {
        if (!isInteractable)
        {
            return; // Ignore pointer up if not interactable
        }
        if (ButtonGlow != null)
        {
            if (!isPersistentGlow)
            {
                ButtonGlow.SetActive(false);
                ButtonNormal.SetActive(true);
            }
        }
        onClick?.Invoke();
        isPointerDown = false;
    }
    private void HandleInteractableChanged ( bool newValue )
    {
        if (buttonNormalImage != null)
        {
            buttonNormalImage.sprite = newValue ? normalsprite : baseSprite;
        }

        if (ButtonGlow != null)
        {
            ButtonGlow.SetActive(false);
        }
    }

    [ContextMenu("Test - 0ff")]
    public void InteractableOff ()
    {
        isInteractable = false;
    }

    [ContextMenu("Test - On")]
    public void InteractableOn ()
    {
        isInteractable = true;
    }
}
