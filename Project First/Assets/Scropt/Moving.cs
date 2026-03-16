
using UnityEngine;
using UnityEngine.InputSystem;

public class Moving : MonoBehaviour
{
    public float moveSpeed = 5f;        
    public float sprintMultiplier = 2f;  

    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isSprinting = false;

   
    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>();
    }

  
    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                
                targetPosition = hit.point;
               
                targetPosition.y = transform.position.y;

                isMoving = true;
            }
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    void Update()
    {
        if (isMoving)
        {
            //»ïÇ×¿¬»êÀÚ?
            float currentSpeed = isSprinting ? (moveSpeed * sprintMultiplier) : moveSpeed;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

           
            Vector3 direction = targetPosition - transform.position;
            if (direction.magnitude > 0.1f)
            {
                transform.forward = direction.normalized;
            }

            
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }
}
