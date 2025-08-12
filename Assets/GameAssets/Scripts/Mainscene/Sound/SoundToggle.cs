using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    Toggle soundToggle;
    [SerializeField] private bool TheSound;
    private Image toggle;
    [SerializeField] private Sprite on_;
    [SerializeField] private Sprite off_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable ()
    {
        soundToggle = GetComponent<Toggle>();
        toggle = GetComponent<Image>();
        Refresh();
    }

    public void SoundPressed ()
    {
        SettingsManager.Instance.ToogleSound(soundToggle.isOn);
        TheSound = SettingsManager.Instance.Sound;
        if (soundToggle.isOn)
        {
            toggle.sprite = on_;
        }
        else
        {
            toggle.sprite = off_;
        }
        DisableSettings();
    }

    public void Refresh ()
    {
        TheSound = SettingsManager.Instance.Sound;
        soundToggle.isOn = TheSound;
        if (soundToggle.isOn)
        {
            toggle.sprite = on_;
        }
        else
        {
            toggle.sprite = off_;
        }
    }

    void DisableSettings ()
    {
       
    }
}
