using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{
    public float moveSpeed = 5f;       // 앞으로 이동하는 속도 (새로 추가)
    public float rotationSpeed = 100f;

    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        // 1. 회전 처리 (기존 코드, 오타 rotation 수정)
        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = rotation * transform.rotation;

        // 2. 이동 처리 (새로 추가)
        // 캐릭터가 바라보는 앞 방향(Vector3.forward)으로 moveInput.y 만큼 이동시킵니다.
        transform.Translate(Vector3.forward * moveInput.y * moveSpeed * Time.deltaTime);
    }
}
