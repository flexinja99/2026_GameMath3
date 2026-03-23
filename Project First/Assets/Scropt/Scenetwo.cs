using UnityEngine;

public class Scenetwo : MonoBehaviour
{
    public Transform player;      
    public float maxDistance = 8f; 
    public float viewAngle = 50f;  
    public float scaleSpeed = 5f;  

    private Vector3 originScale;

    void Start()
    {
        originScale = transform.localScale;
    }

    void Update()
    {
        if (player == null) return;

      
        float distance = Vector3.Distance(transform.position, player.position);
        bool isInside = false;

        if (distance <= maxDistance)
        {
           
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            if (angle <= viewAngle)
            {
                isInside = true;
            }
        }

        Vector3 targetScale = isInside ? Vector3.one * 2f : originScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }


}
