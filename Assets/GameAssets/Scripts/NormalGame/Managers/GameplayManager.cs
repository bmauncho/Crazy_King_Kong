using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public SettingsUI settingUi;
    public GameRules gameRules;
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

    public void skip()
    {
        CommandCenter.Instance.boulderManager_.SkipBoulder();
        Debug.Log("Skip button clicked!");
    }

    public void ToggleSettings ()
    {
        settingUi.ToggleSettings();
    }

    public void ToggleGameRules ()
    {
        gameRules.toggleGameRules();
    }
}
