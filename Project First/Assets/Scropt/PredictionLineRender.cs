using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem; // 사진의 InputValue를 사용하기 위해 추가

[RequireComponent(typeof(LineRenderer))]
public class PredictionLineRenderer : MonoBehaviour
{
    public Transform startPos; // 플레이어 위치 (예: 발사구)
    public Transform endPos;   // 타겟 위치 (우클릭 시 할당됨)
    [Range(1f, 5f)] public float extend = 1.5f;

    private LineRenderer lr;
    public CameraSlerp cs;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.widthMultiplier = 0.05f;
        lr.material = new Material(Shader.Find("Unlit/Color"))
        {
            color = Color.red
        };

    }

    // 사진 속에 있던 우클릭 로직 적용
    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;

        // 마우스 위치에서 레이 발사
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.Log("asd");

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // [타게팅] 
                startPos = transform;
                endPos = hit.transform;
                lr.enabled = true;
                cs.target = endPos;
            }
        }
        else
        {
            // [초기화] 
            endPos = null;
            lr.enabled = false;
        }
    }

    private void Update()
    {
        // 시작점이나 타겟(적)이 없으면 그리지 않음
        if (!startPos || !endPos) return;

        Vector3 a = startPos.position;
        Vector3 b = endPos.position;

        // LerpUnclamped로 적 위치 너머까지 연장된 좌표 계산
        Vector3 pred = Vector3.LerpUnclamped(a, b, extend);

        lr.SetPosition(0, a);
        lr.SetPosition(1, pred);
    }
}