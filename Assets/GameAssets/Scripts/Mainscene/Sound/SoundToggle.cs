using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    ButtonController soundToggle;
    [SerializeField] private bool TheSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable ()
    {
        soundToggle = GetComponent<ButtonController>();
        Refresh();
    }

    public void SoundPressed ()
    {
        SettingsManager.Instance.ToogleSound(soundToggle.toggledOn);
        TheSound = SettingsManager.Instance.Sound;
        CommandCenter.Instance.soundManager_.ToggleAmbientSound();
    }

    public void Refresh ()
    {
        TheSound = SettingsManager.Instance.Sound;
        soundToggle.toggledOn = TheSound;
    }
}
