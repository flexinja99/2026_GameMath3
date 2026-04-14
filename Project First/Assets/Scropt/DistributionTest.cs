using UnityEngine;

public class DistributionTest : MonoBehaviour
{
    int PoissonDistrubution(float lamda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lamda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k;
    }
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            int count = PoissonDistrubution(3f);
            Debug.Log($"Minute{i + 1}: {count} events");
        }
    }

}
