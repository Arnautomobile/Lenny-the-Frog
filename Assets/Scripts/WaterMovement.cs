using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private Vector3 _initialVelocity;
    private Vector3 _slowedVelocity;
    [SerializeField] private float _slowFactor = 0.5f;
    [SerializeField] private GameObject waterSplashPrefab;
    void Start()
    {
        GameLogic2.OnHitWater += OnHitWater;
    }

    private void OnHitWater()
    {
        // could one line this
        // grab initial velocity, multiply it by a slowFactor variable and then apply that slowed
        // velocity vector to the player
        _initialVelocity = _rigidbody.linearVelocity;
        _slowedVelocity = _initialVelocity * _slowFactor;
        _rigidbody.linearVelocity = _slowedVelocity;
        
        // Water splash particle effect
        Vector3 splashPosition = _rigidbody.transform.position;
        Instantiate(waterSplashPrefab, splashPosition, Quaternion.LookRotation(Vector3.up));
    }

    void OnDestroy()
    {
        GameLogic2.OnHitWater -= OnHitWater;
    }
}
