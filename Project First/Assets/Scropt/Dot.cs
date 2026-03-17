using UnityEngine;

public class Dot : MonoBehaviour
{
    public Transform player;

     void Update()
    {
        Vector3 toplayer = player.position - transform.position;

        toplayer.y = 0;

        Vector3 forward = transform.forward;
        forward.y = 0;

        forward.Normalize();
        toplayer.Normalize();

        float dot = Vector3.Dot(forward, toplayer);

        if( dot > 0f)
        {
            Debug.Log("플레이어 앞");
        }
        else if(dot < 0f)
        {
            Debug.Log("플레이어 뒤");
        }
        else
        {
            Debug.Log("플레이어 옆");
        }


    }
}
