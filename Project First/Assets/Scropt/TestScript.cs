using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float degrees = 45;
        float radians = degrees * Mathf.Deg2Rad;
        Debug.Log("45도 -> 라디안 : " + radians);

        float radianValue = Mathf.PI/3;
        float degreeValue = radianValue * Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 5f;
        float angle = 30 ; // 이동할 방향 (도 단위)
        float radians = angle * Mathf.Deg2Rad;

        Vector3 direction = new Vector3(Mathf.Cos(radians),0,Mathf.Sin(radians));
        transform.position += direction * speed * Time.deltaTime;
        
    }
}
