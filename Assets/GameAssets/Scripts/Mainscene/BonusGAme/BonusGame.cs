using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BonusOptions
{
    Gold,
    Silver,
    Bronze,
}
public class BonusGame : MonoBehaviour
{
    public BonusGameUI bonusUI;
    public BonusOptions DefaultOption;
    public List<BonusOptions> newBonusOptionsOrder = new List<BonusOptions>(); 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void restBonusOptions ()
    {
        newBonusOptionsOrder.Clear();
    }


    public void randomizeBonusOptionsOrder ()
    {
        // Reset the list
        restBonusOptions();

        // Step 1: Get all enum values
        List<BonusOptions> options = new List<BonusOptions>((BonusOptions [])System.Enum.GetValues(typeof(BonusOptions)));

        // Step 2: Shuffle the list using Fisher-Yates algorithm
        for (int i = 0 ; i < options.Count ; i++)
        {
            int randIndex = Random.Range(i , options.Count);
            (options [i], options [randIndex]) = (options [randIndex], options [i]);
        }

        // Step 3: Store the shuffled result
        newBonusOptionsOrder.AddRange(options);

        // Optional Debug
        Debug.Log("Randomized Bonus Options:");
        foreach (var opt in newBonusOptionsOrder)
        {
            Debug.Log(opt);
        }
    }


    IEnumerator showBonusGame ()
    {

        yield return null;
    }
}
