using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private Vector3 _initialVelocity;
    private Vector3 _slowedVelocity;
    [SerializeField] private float _slowFactor = 0.5f;
    void Start()
    {
        GameLogic2.OnHitWater += OnHitWater;
    }

    private void OnHitWater()
    {
        _initialVelocity = _rigidbody.linearVelocity;
        _slowedVelocity = _initialVelocity * _slowFactor;
        _rigidbody.linearVelocity = _slowedVelocity;
    }

    void OnDestroy()
    {
        GameLogic2.OnHitWater -= OnHitWater;
    }
}
