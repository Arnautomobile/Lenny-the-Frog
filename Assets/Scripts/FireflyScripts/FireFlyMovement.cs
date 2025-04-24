using UnityEngine;
using UnityEngine.Serialization;

public class FireflyMovement : MonoBehaviour
{
    public enum MovementType { None, Circular, LeftRight, UpDown }
    
    [SerializeField] private MovementType movementType;
    [SerializeField] private float speed = 1f;
    [FormerlySerializedAs("amplitude")] [SerializeField] private float length = 1f;
    
    [SerializeField] private float radius = 1f;
    [SerializeField] private Vector3 rotationEuler = Vector3.zero;

    private Vector3 startPosition;
    private float timer;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime * speed;

        switch (movementType)
        {
            case MovementType.Circular:
                float x = Mathf.Cos(timer) * radius;
                float y = Mathf.Sin(timer) * radius;
                Quaternion rotation = Quaternion.Euler(rotationEuler);
                Vector3 offset = rotation * new Vector3(x, y, 0); // Apply orbit tilt
                transform.position = startPosition + offset;
                break;

            case MovementType.LeftRight:
                transform.position = startPosition + new Vector3(0, 0, Mathf.Sin(timer) * length);
                break;

            case MovementType.UpDown:
                transform.position = startPosition + new Vector3(0, Mathf.Sin(timer) * length, 0);
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 center;

        if (Application.isPlaying)
        {
            center = startPosition;
        }
        else
        {
            center = transform.position;
        }

        switch (movementType)
        {
            case MovementType.Circular:
                DrawCircle(center, radius, rotationEuler);
                break;

            case MovementType.LeftRight:
                Gizmos.DrawLine(center + Vector3.forward * length, center + Vector3.back * length);
                break;

            case MovementType.UpDown:
                Gizmos.DrawLine(center + Vector3.up * length, center + Vector3.down * length);
                break;
        }
    }

    private void DrawCircle(Vector3 center, float radius, Vector3 rotationEuler)
    {
        const int segments = 60;
        Quaternion rotation = Quaternion.Euler(rotationEuler);
        Vector3 prevPoint = center + rotation * new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            Vector3 nextPoint = center + rotation * new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
