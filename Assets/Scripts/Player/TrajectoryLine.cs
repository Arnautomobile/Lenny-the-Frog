using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private GameObject _endLine;
    [SerializeField] private int _maxSteps = 100;
    [SerializeField] private float _timeStep = 0.05f;
    private LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Disable();
    }


    public (Vector3, Vector3) Render(Vector3 startPosition, Vector3 force)
    {
        Vector3 landingPoint = Vector3.zero;
        Vector3 landingNormal = Vector3.up;

        if (!_lineRenderer.enabled) return (landingPoint, landingNormal);

        float gravity = Physics.gravity.y;
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < _maxSteps; i++) {
            float t = i * _timeStep;
            Vector3 tangent = force + gravity * t * Vector3.up;
            Vector3 currentPoint = startPosition + force * t + 0.5f * gravity * t * t * Vector3.up; // up because gravity < 0
            points.Add(currentPoint);
            
            if (Physics.Raycast(currentPoint, tangent.normalized, out RaycastHit hit, tangent.magnitude * _timeStep)) {
                landingPoint = hit.point;
                landingNormal = hit.normal;
                points.Add(hit.point);
                break;
            }
            if (i == _maxSteps - 1) {
                landingPoint = currentPoint;
            }
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
        _endLine.transform.position = landingPoint;
        _endLine.transform.rotation = Quaternion.FromToRotation(Vector3.up, landingNormal);

        return (landingPoint, landingNormal);
    }

    
    public void Enable()
    {
        if (!_lineRenderer.enabled) {
            _lineRenderer.enabled = true;
            _endLine.SetActive(true);
        }
    }

    public void Disable()
    {
        if (_lineRenderer.enabled) {
            _lineRenderer.enabled = false;
            _endLine.SetActive(false);
        }
    }
}
