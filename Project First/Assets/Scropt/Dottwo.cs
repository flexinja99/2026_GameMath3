using UnityEngine;

public class Dottwo : MonoBehaviour
{
    public Transform player;

    public float viewAngle = 60f;// 시야각

    private void Update()
    {
        Vector3 toplayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        float dot = Vector3.Dot(forward, toplayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if(angle < viewAngle / 2)
        {
            Debug.Log("나 여기있지롱");
        }
    }
}
