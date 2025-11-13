using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BonusOptions
{
    gold,
    silver,
    bronze,
}

[System.Serializable]
public class BonusGameConfig
{
    public BonusOptions BonusOptions;
    public Sprite Bg;
    public Sprite Icon;
}

public class BonusGame : MonoBehaviour
{
    APIManager apiMan_;
    PoolManager poolMan_;
    public BonusGameUI bonusUI;
    public BonusGameWinUI bonusWinUI;
    public BonusGameConfig [] bonusConfig;
    public List<BonusOptions> newBonusOptionsOrder = new List<BonusOptions>();
    private void Start ()
    {
        poolMan_ = CommandCenter.Instance.poolManager_;
        apiMan_ = CommandCenter.Instance.apiManager_;
    }
    public  void restBonusOptions ()
    {
        newBonusOptionsOrder.Clear();
    }


    public void randomizeBonusOptionsOrder ()
    {
        // Reset the list
        restBonusOptions();

        List<BonusOptions> options = new List<BonusOptions>((BonusOptions [])System.Enum.GetValues(typeof(BonusOptions)));

        // Step 2: Shuffle the list using Fisher-Yates algorithm
        for (int i = 0 ; i < options.Count ; i++)
        {
            int randIndex = Random.Range(i , options.Count);
            (options [i], options [randIndex]) = (options [randIndex], options [i]);
        }

        newBonusOptionsOrder.AddRange(options);

    }

    public void setUpBonusRewards ()
    {
        restBonusOptions ();
        randomizeBonusOptionsOrder ();

        int i = 0;

        foreach(var bonusOpt in bonusUI.options)
        {
            if (bonusUI.options.Length > bonusConfig.Length ||
                bonusUI.options.Length > newBonusOptionsOrder.Count)
            {
                Debug.LogWarning("Mismatch in bonus configuration data length.");
                return;
            }

            Transform parent = bonusOpt.BonusReward.transform;

            GameObject reward = poolMan_.GetFromPool(
                PoolType.BonusReward,
                Vector3.zero,
                Quaternion.identity,
                parent);

            bonusOpt.AddOwner(reward);
            reward.transform.localPosition = Vector3.zero;
            reward.transform.localScale = Vector3.one;

            BonusReward bonusComponenet = reward.GetComponent<BonusReward>();
            bonusComponenet.SetData(bonusConfig [i].Bg , bonusConfig [i].Icon , newBonusOptionsOrder [i]);
            bonusOpt.resetBonusOption();
            i++;
        }
    }
}
