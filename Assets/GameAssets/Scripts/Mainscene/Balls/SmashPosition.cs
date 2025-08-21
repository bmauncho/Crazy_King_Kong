using UnityEngine;

public class SmashPosition : MonoBehaviour
{
    public GameObject TheOwner;

    public void AddOwner ( GameObject owner )
    {
        if (TheOwner != null) return;

        TheOwner = owner;
    }

    public void RemoveOwner ()
    {
        if (TheOwner == null) return;
        TheOwner = null;
    }
}
