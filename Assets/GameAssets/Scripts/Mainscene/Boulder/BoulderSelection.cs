using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class probabilityWeights
{
    public BoulderType type;
    [Range(0f, 100f)]
    public float weight;
}

public class BoulderSelection : MonoBehaviour
{
    [Header("boulder type config")]
    public BoulderTypeConfig [] boulderTypeConfig;
    public int maxRepeat = 4;
    private int lastIndex = -1;
    private int repeatCount = 0;
    public List<probabilityWeights> probabilityWeights = new List<probabilityWeights>();

    public double totalWeights;

    private void Start ()
    {
        calculatetotalWeights();
    }

    void calculatetotalWeights ()
    {
        foreach(var weight in probabilityWeights)
        {
            totalWeights += weight.weight;
        }
    }

    public double GetTotalWeights ()
    {
        return totalWeights;
    }

    public BoulderTypeConfig GetRandomBoulderTypeConfig ()
    {
        if (boulderTypeConfig == null || boulderTypeConfig.Length == 0)
        {
            Debug.LogError("BoulderTypeConfig array is null or empty.");
            return null;
        }

        int index = WhichConfig();
        if (index < 0 || index >= boulderTypeConfig.Length)
        {
            Debug.LogError($"Index {index} is out of bounds for BoulderTypeConfig array of length {boulderTypeConfig.Length}.");
            return null;
        }

        return boulderTypeConfig [index];
    }

    public int WhichConfig ()
    {
        int configCount = boulderTypeConfig.Length;

        if (configCount <= 0)
        {
            Debug.LogError("No BoulderTypeConfigs available.");
            return 0;
        }

        List<int> validIndices = new List<int>();
        List<float> validWeights = new List<float>();

        for (int i = 0 ; i < configCount ; i++)
        {
            if (repeatCount >= maxRepeat && i == lastIndex)
                continue;

            validIndices.Add(i);

            // Find matching weight from the probabilityWeights list
            float weight = 1f; // default if not found
            var match = probabilityWeights.Find(p => p.type == boulderTypeConfig [i].type);
            if (match != null)
            {
                weight = match.weight;
            }
            validWeights.Add(weight);
        }

        if (validIndices.Count == 0)
        {
            Debug.LogWarning("Only one boulder type available or all filtered out. Reusing last index.");
            return lastIndex;
        }

        int selectedRelativeIndex = WeightedRandomIndex(validWeights);
        int selectedConfigIndex = validIndices [selectedRelativeIndex];

        // Update repeat tracking
        if (selectedConfigIndex == lastIndex)
            repeatCount++;
        else
            repeatCount = 1;

        lastIndex = selectedConfigIndex;
        return selectedConfigIndex;
    }


    private int WeightedRandomIndex ( List<float> weights )
    {
        float totalWeight = 0f;
        foreach (float w in weights)
            totalWeight += w;

        float randomValue = Random.Range(0f , totalWeight);
        float cumulative = 0f;

        for (int i = 0 ; i < weights.Count ; i++)
        {
            cumulative += weights [i];
            if (randomValue < cumulative)
                return i;
        }

        return weights.Count - 1; // fallback
    }

}
