using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Settings")]
    public bool isToggle;
    public bool isPersistentGlow;
    public bool isInteractable = true;

    public bool toggledOn = false;

    [Header("Sprites")]
    public Sprite baseSprite;
    public Sprite normalsprite;
    public Sprite toggledSprite;
    public Sprite toggleGlowSprite;

    [Header("Button Objects")]
    public GameObject ButtonNormal;
    public GameObject ButtonGlow;

    [Header("Events")]
    public UnityEvent onClick;

    private Image buttonNormalImage;
    private Image buttonGlowImage;

    private Sprite normalGlow;
    private bool isPointerDown = false;
    private bool lastInteractableValue;

    private void Awake ()
    {
        if (ButtonNormal != null)
            buttonNormalImage = ButtonNormal.GetComponent<Image>();
        else
            Debug.LogWarning("ButtonNormal GameObject not assigned.");

        if (ButtonGlow != null)
            buttonGlowImage = ButtonGlow.GetComponent<Image>();
        else
            Debug.LogWarning("ButtonGlow GameObject not assigned.");

        if (buttonGlowImage != null)
            normalGlow = buttonGlowImage.sprite;

        HandleInteractableChanged(isInteractable);
        SetToggleState(toggledOn);
    }

    private void Start ()
    {
        SetToggleState(toggledOn);
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
        if (!isInteractable) return;
        isPointerDown = true;

        if (isToggle)
            HandleTogglePointerDown();
        else
            HandleNormalPointerDown();
    }

    public void OnPointerUp ( PointerEventData eventData )
    {
        if (!isInteractable) return;

        if (isToggle)
        {
            toggledOn = !toggledOn;
            HandleToggleOnPointerUp();
        }
        else if (!isPersistentGlow)
        {
            ButtonGlow?.SetActive(false);
            ButtonNormal?.SetActive(true);
        }

        onClick?.Invoke();
        isPointerDown = false;
    }

    private void HandleTogglePointerDown ()
    {
        if (isPersistentGlow) return;

        if (buttonGlowImage != null)
            buttonGlowImage.sprite = toggledOn ? normalGlow : toggleGlowSprite;

        ButtonGlow?.SetActive(true);
        ButtonNormal?.SetActive(false);

        // Optional: animate glow (requires DOTween)
         ButtonGlow.transform.DOScale(Vector3.one * 1.1f, 0.15f).SetEase(Ease.OutBack);
    }

    private void HandleNormalPointerDown ()
    {
        if (ButtonGlow == null || ButtonNormal == null) return;

        if (isPersistentGlow)
        {
            bool newState = !ButtonGlow.activeSelf;
            ButtonGlow.SetActive(newState);
            ButtonNormal.SetActive(!newState);
        }
        else
        {
            ButtonGlow.SetActive(true);
            ButtonNormal.SetActive(false);
        }

        // Optional: animate glow
         ButtonGlow.transform.DOScale(Vector3.one * 1.1f, 0.15f).SetEase(Ease.OutBack);
    }

    private void HandleInteractableChanged ( bool newValue )
    {
        if (buttonNormalImage != null)
        {
            if (!isToggle)
            {
                buttonNormalImage.sprite = newValue ? normalsprite : baseSprite;
            }
        }
        ButtonGlow?.SetActive(false);
        ButtonNormal?.SetActive(true);
    }

    public void HandleToggleOnPointerUp ()
    {
        if (buttonNormalImage != null)
        {
            if (toggledOn)
            {
                buttonNormalImage.sprite = normalsprite;
            }
            else
            {
                buttonNormalImage.sprite = toggledSprite;
            }
        }

        ButtonGlow?.SetActive(false);
        ButtonNormal?.SetActive(true);
    }

    private void UpdateToggleVisuals ()
    {
  
        if (buttonNormalImage != null)
        {
            if (toggledOn)
            {
                //Debug.Log("UpdateToggleVisuals -- on");
                buttonNormalImage.sprite = normalsprite;
            }
            else
            {
                //Debug.Log("UpdateToggleVisuals -- off");
                buttonNormalImage.sprite = toggledSprite;
            }
        }

        ButtonGlow?.SetActive(false);
        ButtonNormal?.SetActive(true);
    }

    public bool IsToggledOn => toggledOn;

    public void SetToggleState ( bool value )
    {
        toggledOn = value;
        UpdateToggleVisuals();
    }

    [ContextMenu("Test - Interactable Off")]
    public void InteractableOff () => isInteractable = false;

    [ContextMenu("Test - Interactable On")]
    public void InteractableOn () => isInteractable = true;
}
