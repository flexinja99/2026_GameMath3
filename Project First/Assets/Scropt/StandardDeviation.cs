using UnityEngine;
using System.Linq; 

public class StandardDeviationTest : MonoBehaviour
{
    
    public int n = 1000;

    
    public float minRange = 0f;

    
    public float maxRange = 1f;

    void Start()
    {
        
        StandardDev();
    }

    public void StandardDev()
    {
        
        float[] samples = new float[n];
        for (int i = 0; i < n; i++)
        {
            samples[i] = Random.Range(minRange, maxRange);
        }

        
        float mean = samples.Average();

       
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x - mean, 2));

       
        float stdDev = Mathf.Sqrt(sumOfSquares / n);

       
        Debug.Log($"평균: {mean}, 표준편차: {stdDev}");
    }

    float GenerateGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value; // 0보다 큰 난수
        float u2 = 1.0f - Random.value; // 0보다 큰 난수

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                              Mathf.Sin(2.0f * Mathf.PI * u2); // 표준 정규분포

        return mean + stdDev * randStdNormal; // 원하는 평균과 표준편차로 변환
    }
}
