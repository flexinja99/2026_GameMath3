using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyParry : MonoBehaviour
{
    public enum EnemyType { White, Yellow, Red }
    public EnemyType type;

    private Transform player;
    private PlayerController playerScript;

    // 능력치 변수들
    private float viewAngle;
    private float rotationSpeed;
    private float viewDistance;
    private float dashSpeed = 3f;
    private bool isChasing = false;

    void Start()
    {
        // [중요] 태그가 "Player"인 오브젝트를 찾습니다.
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerScript = playerObj.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("씬에 'Player' 태그를 가진 오브젝트가 없습니다!");
        }

        SetupStats();
    }

    void SetupStats()
    {
        // 타입별 초기 설정
        switch (type)
        {
            case EnemyType.White:
                viewAngle = 60f; rotationSpeed = 30f; viewDistance = 5f;
                GetComponent<Renderer>().material.color = Color.white;
                break;
            case EnemyType.Yellow:
                viewAngle = 90f; rotationSpeed = 45f; viewDistance = 8f;
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case EnemyType.Red:
                viewAngle = 180f; rotationSpeed = 60f; viewDistance = 12f;
                GetComponent<Renderer>().material.color = Color.red;
                break;
        }
    }

    void Update()
    {
        if (player == null) return; // 플레이어가 없으면 실행 안 함

        if (!isChasing)
        {
            // 매 초 회전
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            CheckForPlayer();
        }
        else
        {
            // 플레이어에게 돌진
            ChaseAndParryCheck();
        }
    }

    void CheckForPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        // 1. 내적(Dot Product)을 이용한 시야 판별
        // transform.forward와 플레이어 방향 벡터 사이의 각도 계산
        float dot = Vector3.Dot(transform.forward, directionToPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle < viewAngle * 0.5f && distance <= viewDistance)
        {
            isChasing = true;
            Debug.Log(type + " 적이 플레이어를 발견했습니다!");
        }
    }

    void ChaseAndParryCheck()
    {
        // 플레이어 방향으로 이동
        transform.position = Vector3.MoveTowards(transform.position, player.position, dashSpeed * Time.deltaTime);

        // 거리가 1.2 미터 이하면 패링 체크 (수치는 적 크기에 따라 조절)
        if (Vector3.Distance(transform.position, player.position) < 1.2f)
        {
            PerformParrySystem();
        }
    }

    void PerformParrySystem()
    {
        // 2. 외적(Cross Product)을 이용한 좌우 판별
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 cross = Vector3.Cross(transform.forward, directionToPlayer);

        // cross.y가 0보다 작으면 플레이어는 적의 기준 왼쪽, 크면 오른쪽
        bool isPlayerOnLeft = cross.y < 0;

        if (isPlayerOnLeft && playerScript.isParryingLeft) // Q 누름
        {
            Debug.Log("왼쪽 패링 성공!");
            Destroy(gameObject);
        }
        else if (!isPlayerOnLeft && playerScript.isParryingRight) // E 누름
        {
            Debug.Log("오른쪽 패링 성공!");
            Destroy(gameObject);
        }
        else
        {
            // 패링 실패: 씬 재시작
            Debug.Log("패링 실패! 씬을 다시 로드합니다.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}