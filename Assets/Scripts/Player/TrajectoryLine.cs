using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private GameObject _endLine;
    [SerializeField] private int _maxSteps = 100;
    [SerializeField] private float _timeStep = 0.05f;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;
    private LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Disable();
    }


    public (Vector3, float) Render(Vector3 startPosition, Vector3 force)
    {
        if (!_lineRenderer.enabled) return (Vector3.up, 0);

        int i = 0;
        float t = 0;
        float gravity = Physics.gravity.y;
        Vector3 landingNormal = Vector3.up;
        Vector3 landingPoint = Vector3.zero;
        List<Vector3> points = new List<Vector3>();

        while (i < _maxSteps) {
            t = i * _timeStep;
            Vector3 tangent = force + gravity * t * Vector3.up; // up because gravity < 0
            landingPoint = startPosition + force * t + 0.5f * gravity * t * t * Vector3.up;
            points.Add(landingPoint);
            
            if (Physics.Raycast(landingPoint, tangent.normalized, out RaycastHit hit, tangent.magnitude * _timeStep)) {
                landingPoint = hit.point;
                landingNormal = hit.normal;
                points.Add(hit.point);
                break;
            }
            i++;
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());

        float scale = Mathf.Lerp(_minScale, _maxScale, (float)i/(float)_maxSteps);
        _endLine.transform.localScale = new Vector3(scale, scale, scale);
        _endLine.transform.SetPositionAndRotation(landingPoint, Quaternion.FromToRotation(Vector3.up, landingNormal));
        
        return (landingNormal, t);
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
