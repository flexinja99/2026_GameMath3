using UnityEngine;

public class FOVVisualizer : MonoBehaviour
{
    public float viewAngle = 60;

    public float viewDistance = 5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        Vector3 rigjtBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rigjtBoundary);

        Gizmos.color= Color.red;
        Gizmos.DrawRay (transform.position, forward);
    }
}
