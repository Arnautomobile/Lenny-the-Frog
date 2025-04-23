using System;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector2 _maxRotation;
    [SerializeField] private List<CameraOffset> _offsetsList;

    private Dictionary<CameraState, (Vector3 offset, float followTime)> _offsets;
    private Camera _camera;
    private Vector3 _velocity;
    private float _yRotation;

    public CameraState State { get; set; }


    void Start()
    {
        _camera = GetComponent<Camera>();
        _offsets = new Dictionary<CameraState, (Vector3 offset, float followTime)>();
        foreach (CameraOffset value in _offsetsList) {
            _offsets.Add(value.state, (value.offset, value.followTime));
        }
        State = CameraState.BASEFOLLOW;
    }

    void Update()
    {
        if (State == CameraState.FREE) return;

        if (State == CameraState.BASEFOLLOW) {
            _yRotation = _player.transform.eulerAngles.y;
        }

        Vector3 targetOffset = _offsets[State].offset;
        float followTime = _offsets[State].followTime;

        /*Vector3 offset = _player.transform.position - transform.position;
        offset = Vector3.SmoothDamp(offset, targetOffset, ref _velocity, followTime);*/
        Vector3 rotatedOffset = Quaternion.Euler(0, _yRotation, 0) * targetOffset;
        transform.position = _player.transform.position + rotatedOffset;

        Vector2 mousePos = _camera.ScreenToViewportPoint(Input.mousePosition);
        transform.rotation = Quaternion.Euler(_maxRotation.x * (1 - mousePos.y - 0.5f) * 2,
                                    _yRotation + _maxRotation.y * (mousePos.x - 0.5f) * 2, 0);
    }
}