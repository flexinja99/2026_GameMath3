using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float rotationSpeed = 45f;
    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime,0);
    }
}
