
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    // 현재 프레임의 패링 상태
    public bool isParryingLeft { get; private set; }
    public bool isParryingRight { get; private set; }

    void Update()
    {
        // 1. 이동 및 회전
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;

        transform.Translate(0, 0, move);
        transform.Rotate(0, rotate, 0);

        // 2. 패링 입력 감지 (Q: 왼쪽, E: 오른쪽)
        isParryingLeft = Input.GetKey(KeyCode.Q);
        isParryingRight = Input.GetKey(KeyCode.E);
    }

    public void Die()
    {
        // 패링 실패 시 씬 재로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}