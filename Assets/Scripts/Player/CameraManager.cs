using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector2 _maxRotation;
    [SerializeField] private List<CameraOffset> _offsetsList; // x offset will not be used

    private Camera _camera;
    private Dictionary<CameraState, (Vector3 offset, float followTime)> _offsets;
    private Vector3 _currentOffset = Vector3.zero;
    private float _yRotation = 0;

    private Vector3 vel1;
    private float vel2;

    public CameraState State { get; set; }


    void Start()
    {
        _camera = GetComponent<Camera>();
        _offsets = new Dictionary<CameraState, (Vector3 offset, float followTime)>();
        foreach (CameraOffset value in _offsetsList) {
            _offsets.Add(value.state, (value.offset, value.followTime));
        }
        State = CameraState.BASEFOLLOW;
        _currentOffset = _offsets[State].offset;
    }

    void Update()
    {
        if (State == CameraState.FREE) return;

        float followTime = _offsets[State].followTime;
        Vector3 targetOffset = _offsets[State].offset;

        float angle = _player.transform.eulerAngles.z * Mathf.PI / 180;
        Vector3 rotatedOffset = new Vector3( -Mathf.Sin(angle) * targetOffset.y,
                                              Mathf.Cos(angle) * targetOffset.y,
                                              targetOffset.z);

        if (State == CameraState.BASEFOLLOW) {
            _yRotation = Mathf.SmoothDampAngle(_yRotation, _player.transform.eulerAngles.y, ref vel2, followTime);
        }

        _currentOffset = Vector3.SmoothDamp(_currentOffset, rotatedOffset, ref vel1, followTime);
        transform.position = _player.transform.position + Quaternion.Euler(0, _yRotation, 0) * _currentOffset;

        Vector2 mousePos = _camera.ScreenToViewportPoint(Input.mousePosition);
        transform.rotation = Quaternion.Euler(_maxRotation.x * (1 - mousePos.y - 0.5f) * 2,
                                    _yRotation + _maxRotation.y * (mousePos.x - 0.5f) * 2, 0);
    }
}