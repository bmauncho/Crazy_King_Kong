using UnityEngine;
using UnityEngine.UI;

public class GameRules : MonoBehaviour
{
    public bool isActive = false;
    public ScrollRect scrollRect;
    public RectTransform slidingArea; // The full area the handle moves within
    public RectTransform handle;      // The custom handle (center-anchored)

    void Start ()
    {
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
        moveHandle(); // Initial sync
    }

    public void openGameRules ()
    {
        gameObject.SetActive(true);
        CommandCenter.Instance.gamePlayManager_.settingUi.gameObject.SetActive(false);
        moveHandle();
    }

    public void closeGameRules ()
    {
        gameObject.SetActive(false);
        CommandCenter.Instance.gamePlayManager_.settingUi.gameObject.SetActive(true);
    }

    public void toggleGameRules ()
    {
        isActive = !isActive;
        if (isActive)
            openGameRules();
        else
            closeGameRules();
    }

    private void OnScrollValueChanged ( Vector2 _ )
    {
        moveHandle();
    }

    public void moveHandle ()
    {
        float slidingHeight = slidingArea.rect.height;
        float handleHeight = handle.rect.height;

        // Total vertical range the handle can move within
        float movementRange = slidingHeight - handleHeight;

        // Calculate Y position from bottom (normalizedPosition: 1 at top, 0 at bottom)
        float y = ( scrollRect.verticalNormalizedPosition - 0.5f ) * movementRange;

        handle.anchoredPosition = new Vector2(handle.anchoredPosition.x , y);
    }
}
