using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public GameObject normal;
    public GameObject glow;
    public GameObject Btn_off;
    [SerializeField]private bool IsToggled = false;
    private Button Btn;
    private bool lastState;

    private void Start ()
    {
        Btn = GetComponent<Button>();
        lastState = Btn.interactable;
    }

    void Update ()
    {
        if (Btn.interactable != lastState)
        {
            lastState = Btn.interactable;
            OnInteractableChanged(lastState);
        }
    }

    private void OnInteractableChanged ( bool isInteractable )
    {
       // Debug.Log("Button interactable changed: " + isInteractable);

        if(normal != null)
            normal.SetActive(isInteractable);

        if(glow != null)
            glow.SetActive(false);

        if(Btn_off != null)
            Btn_off.SetActive(!isInteractable);
    }



    public void click ()
    {
        CancelInvoke("HideGlow");
        ShowGlow();
        Invoke(nameof(HideGlow) , .25f);
    }

    public void Toggle ()
    {
        IsToggled = !IsToggled;
        normal.SetActive(!IsToggled);
        glow.SetActive(IsToggled);
    }
    public void ShowGlow ()
    {
        normal.SetActive (false);
        glow.SetActive (true);
    }

    public void HideGlow ()
    {
        normal .SetActive (true);
        glow .SetActive (false);
    }
}
