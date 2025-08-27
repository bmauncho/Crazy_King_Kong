using System.Collections;
using UnityEngine;

public enum KongAnimType
{
    Idle,
    ThumpBoth,
    ThumpSingle,
    WinBoth,
    WinSingle,
}

[System.Serializable]
public class KongAnimation
{
    public string name;
    public KongAnimType type;
    public Animator anim;
}
public class KongAnim : MonoBehaviour
{
    public KongAnimType currentAimType;
    public KongAnimation [] KongAnimations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayKongAnim ( KongAnimType type )
    {
        // Prevent re-playing the current animation type
        if (currentAimType == type)
            yield break;

        // Try to get the animation data for the requested type
        var anim = GetKongAnim(type);

        // Enable only the selected animation, disable the others
        foreach (var kAnim in KongAnimations)
        {
            bool isActive = ( kAnim.type == type );
            kAnim.anim.gameObject.SetActive(isActive);
        }

        // Set the current type only if the anim is valid
        if (anim?.anim != null)
        {
            currentAimType = type;
        }


    }


    public KongAnimation GetKongAnim ( KongAnimType type )
    {
        foreach (var anim in KongAnimations)
        {
            if (anim.type == type)
                return anim;
        }
        return null;
    }
}
