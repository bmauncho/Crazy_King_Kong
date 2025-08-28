using UnityEngine;

public class SmashedVfxEvent : MonoBehaviour
{
    public GameObject Anim;
    public void showDebri ()
    {
        if(Anim == null)
            return;
        Anim.SetActive(true);
    }

    public void hideDebri ()
    {
        if(Anim == null)
            return;
        Anim.SetActive(false);
    }
}
