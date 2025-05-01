using UnityEngine;

public class DualTargetCamera : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float minZoom = 5f;
    public float maxZoom = 10f;
    public float zoomLimiter = 10f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);

        float distance = Vector3.Distance(player1.position, player2.position);
        float newZoom = Mathf.Lerp(maxZoom, minZoom, distance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        return (player1.position + player2.position) / 2f;
    }
}