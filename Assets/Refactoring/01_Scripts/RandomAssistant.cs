using System.Collections.Generic;
using UnityEngine;

public class RandomAssistant
{
    /// <summary>
    /// percent 확률로 true를 반환
    /// </summary>
    /// <returns></returns>
    public bool TryChance(float percent)
    {
        percent = Mathf.Clamp(percent, 0f, 100f);

        float randomValue = Random.Range(0f, 100f);

        return randomValue < percent;
    }

    public int WeightedRandomSelector(List<(int value, int weight)> weightsTable)
    {
        float totalWeight = 0f;

        foreach (var entry in weightsTable)
        {
            totalWeight += entry.weight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float sum = 0f;

        foreach (var entry in weightsTable)
        {
            sum += entry.weight;

            if(randomValue < sum)
            {
                return entry.value;
            }
        }

        return 0;
    }
}
