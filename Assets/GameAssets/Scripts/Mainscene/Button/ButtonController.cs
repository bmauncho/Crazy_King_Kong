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
    public UnityEvent onClick; // Only triggers this button, instance-specific

    private Image buttonNormalImage;
    private Image buttonGlowImage;

    private Sprite normalGlow;
    private bool isPointerDown = false;
    [SerializeField] private bool lastInteractableValue;

    private float lastUpTime = 0f;
    private float lastDownTime = 0f;

    private void Awake ()
    {
        //  Ensure we only cache this instance's images
        if (ButtonNormal != null)
            buttonNormalImage = ButtonNormal.GetComponent<Image>();
        else
            Debug.LogWarning($"ButtonNormal not assigned on {name}"); //  added warning

        if (ButtonGlow != null)
            buttonGlowImage = ButtonGlow.GetComponent<Image>();
        else
            Debug.LogWarning($"ButtonGlow not assigned on {name}"); //  added warning

        if (buttonGlowImage != null)
            normalGlow = buttonGlowImage.sprite;

        HandleInteractableChanged(isInteractable);
        SetToggleState(toggledOn);

        lastInteractableValue = !isInteractable;
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
        if (Time.unscaledTime - lastDownTime < 0.1f) return;
        lastDownTime = Time.unscaledTime;

        if (!isInteractable) return;

        isPointerDown = true;

        if (isToggle) HandleTogglePointerDown();
        else HandleNormalPointerDown();
    }

    public void OnPointerUp ( PointerEventData eventData )
    {
        if (Time.unscaledTime - lastUpTime < 0.1f) return;
        lastUpTime = Time.unscaledTime;

        if (!isInteractable) return;

        //  Only toggle this button
        if (isToggle)
        {
            toggledOn = !toggledOn;
            UpdateToggleVisuals(); //  Updated visuals only for this button
        }
        else if (!isPersistentGlow)
        {
            ButtonGlow?.SetActive(false); //  Affects only this button
            ButtonNormal?.SetActive(true);
        }

        // Critical fix: only invoke this button's onClick
        onClick?.Invoke();

        isPointerDown = false;
    }

    private void HandleTogglePointerDown ()
    {
        if (isPersistentGlow) return;

        if (buttonGlowImage != null)
            buttonGlowImage.sprite = toggledOn ? normalGlow : toggleGlowSprite;

        // Only affects this button's GameObjects
        ButtonGlow?.SetActive(true);
        ButtonNormal?.SetActive(false);

        ButtonGlow.transform.DOScale(Vector3.one * 1.1f , 0.15f).SetEase(Ease.OutBack);
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

        ButtonGlow.transform.DOScale(Vector3.one * 1.1f , 0.15f).SetEase(Ease.OutBack);
    }

    private void HandleInteractableChanged ( bool newValue )
    {
        if (buttonNormalImage != null && !isToggle)
            buttonNormalImage.sprite = newValue ? normalsprite : baseSprite;

        // Only affects this button
        ButtonGlow?.SetActive(false);
        ButtonNormal?.SetActive(true);
    }

    private void UpdateToggleVisuals ()
    {
        if (buttonNormalImage != null)
        {
            // Only update this instance
            buttonNormalImage.sprite = toggledOn ? normalsprite : toggledSprite;
        }

        //  Only affects this button's visuals
        ButtonGlow?.SetActive(false);
        ButtonNormal?.SetActive(true);
    }

    public bool IsToggledOn => toggledOn;

    public void SetToggleState ( bool value )
    {
        toggledOn = value;
        UpdateToggleVisuals(); // Only updates this button
    }

    [ContextMenu("Test - Interactable Off")]
    public void InteractableOff () => isInteractable = false;

    [ContextMenu("Test - Interactable On")]
    public void InteractableOn () => isInteractable = true;
}
