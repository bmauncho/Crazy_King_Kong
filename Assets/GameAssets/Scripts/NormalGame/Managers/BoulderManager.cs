using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public enum BoulderType
{
    white,
    red,
    blue,
    gold,
}
[System.Serializable]
public class BoulderTypeConfig
{
    public BoulderType type;
    public Sprite boulder;
}
public class BoulderManager : MonoBehaviour
{
    PoolManager poolMan_;
    [Header("settings")]
    public SmashBoulder smash;
    public KongAnim kongAnim;
    public SkipBoulder skip;
    public SmashPosition smashPos;
    public Boulders boulders;
    public BoulderMovement movement;
    public BoulderSelection selection;
    public bool IsSkip = false;

    public GameObject Boulder;

    public GameObject boulderPref;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poolMan_ = CommandCenter.Instance.poolManager_;
        Invoke(nameof(setUpBoulders) , 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUpBoulders ()
    {
        foreach(var boulderPos in boulders.boulderConfigs)
        {
            foreach (var pos in boulderPos.ballPositions)
            {
                if (pos.TheOwner == null)
                {
                    //var boulder = poolMan_.GetFromPool(
                    //    PoolType.Boulder ,
                    //    pos.transform.position ,
                    //    Quaternion.identity ,
                    //    pos.transform);

                    var boulder = Instantiate(boulderPref , pos.transform.position , Quaternion.identity , pos.transform);
                    boulder.transform.localPosition = Vector3.zero;

                    pos.AddOwner(boulder);
                    BoulderTypeConfig newConfig = selection.GetRandomBoulderTypeConfig();
                   // Debug.Log($"type : {newConfig.type.ToString()} ");
                    boulder.GetComponent<Boulder>().SetBoulderType(newConfig.type);
                    boulder.GetComponent<Boulder>().SetBoulderSprite(newConfig.boulder);

                }
            }
        }

        var smashBoulder = poolMan_.GetFromPool(PoolType.Boulder ,
            smashPos.transform.position ,
            Quaternion.identity ,
            smashPos.transform);

        smashBoulder.transform.localPosition = Vector3.zero;
        smashBoulder.GetComponent<RectTransform>().sizeDelta = new Vector2(90f, 90f);

        smashPos.AddOwner(smashBoulder);
        Boulder = smashBoulder;
        BoulderTypeConfig config = selection.GetRandomBoulderTypeConfig();
        //Debug.Log($"type : {config.type.ToString()} ");
        Boulder.GetComponent<Boulder>().SetBoulderType(config.type);
        Boulder.GetComponent<Boulder>().SetBoulderSprite(config.boulder);
    }

    public void SmashBoulder ()
    {
        StartCoroutine(smash.smash_Boulder());
    }

    public void SkipBoulder ()
    {
        if(IsSkip) return;
        IsSkip = true;
        StartCoroutine(skip.skip_Boulder());
    }

    public void returnBoulderToPool ()
    {
        Boulder boulder = Boulder.GetComponent<Boulder>();
        boulder.resetCrack();
        //poolMan_.ReturnToPool(PoolType.Boulder , Boulder);
        Destroy(boulder.gameObject);
    }

    public BoulderType GetCurrentBoulderType ()
    {
        if (Boulder == null)
        {
            Debug.LogWarning("Boulder gameobject missing!Defauling to Rock boulder Type");
            return BoulderType.white;
        }
        return Boulder.GetComponent<Boulder>().boulderType;
    }
}
