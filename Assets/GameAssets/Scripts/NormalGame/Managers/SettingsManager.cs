using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public SoundManager SoundManager_;
    public bool Sound = false;
    void Awake ()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            //DontDestroyOnLoad(this);
            Instance = this;
        }
    }

    private void OnEnable ()
    {
        RefreshSettings();
    }

    public void RefreshSettings ()
    {
        if (PlayerPrefs.HasKey("Sound_Setting"))
        {
            if (PlayerPrefs.GetFloat("Sound_Setting") > 0)
            {
                Sound = true;
            }
            else
            {
                Sound = false;

            }
        }
        else
        {
            PlayerPrefs.SetFloat("Sound_Setting" , 1);
            Sound = true;
        }

        ToogleSound(Sound);
    }

    public void ToogleSound ( bool IsOn )
    {
        Sound = IsOn;

        if (IsOn)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }

        PlayerPrefs.SetFloat("Sound_Setting" , AudioListener.volume);
        SoundManager_.ToggleAllSounds(Sound);
    }

    public float SoundVolume ()
    {
        return PlayerPrefs.GetFloat("Sound_Setting");
    }
}
