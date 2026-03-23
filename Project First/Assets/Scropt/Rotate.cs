using UnityEngine;

public class Rotate : MonoBehaviour
{
    
    public Transform centerTransform;
    public float radius = 5f;
    public float speed = 2f;

    private float angle = 0f;

    void Update()
    {
        if (centerTransform == null) return;

        
        angle += speed * Time.deltaTime;

        float x = centerTransform.position.x + Mathf.Cos(angle) * radius;
        float z = centerTransform.position.z + Mathf.Sin(angle) * radius;
        float y = centerTransform.position.y;

        transform.position = new Vector3(x, y, z);
    }
}

