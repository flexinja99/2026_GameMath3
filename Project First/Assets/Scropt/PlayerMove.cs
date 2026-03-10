using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed
    public float moveSpeed = 5f;
    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isSprinting = false;




    public void OnPoint(InputValue value)
    {
        moveScreenPosition = value.Get<Vector2>();  
    }

    // Update is called once per frame
    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach(RaycastHit hit in hits)
            {
                if(hit.collider.gameObject != gameObject)
                {
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    isMoving = true;
                     
                    break;
                }

                        
            }
        }
    }

    void Update()
    {
      

        if (isMoving)
        {
            Vector3 offset = targetPosition - transform.position;
            float distace = Mathf.Sqrt(offset.x * offset.x + offset.y + offset.z * offset.z);

            transform.positon+=distance

            if(distace < 0.1f)
            {
                isMoving = false;
            }

        }
    }

    public void OnSprint(Inputvalue value)
    {
        isSprinting = value.isPressed;

        if (isSprinting)
        {
            //(? 1 :2) 삼항연산자??

            walkSpeed = isSprinting ? (MoveSpeed * 2f) : walkSpeed;
            
        }
    }
}
