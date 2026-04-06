using UnityEngine;

public class RandomSeed : MonoBehaviour
{
    void Start()
    {
        // .NET 스타일 시드 고정 난수
        System.Random rand = new System.Random(1234);   // 항상 같은 순서로 출력됨
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(rand.Next(1, 7)); // 1~6 사이의 정수
        }

        // Unity에서의 시드 고정 난수
        Random.InitState(1234);     // Unity 난수 시드 고정
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(Random.Range(1, 7));  // 1~6 사이의 난수
        }
    }
}