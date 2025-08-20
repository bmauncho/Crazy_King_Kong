using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public SettingsUI settingUi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spin ()
    {
        Debug.Log("Spin button clicked!");
    }

    public void ToggleSettings ()
    {
        settingUi.ToggleSettings();
    }
}
