using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class frogCroaking : MonoBehaviour
{
    public delegate void FrogCroaking();

    public static event FrogCroaking OnFrogCroaking;


    private bool _isWaiting;
    private float _waitTime;

    private void Update()
    {
        //choose random seconds from 3-10 and croak after that many seconds
        if (!_isWaiting)
        {
            _isWaiting = true;
            _waitTime = Random.Range(3f, 10f);
        }

        if (_isWaiting)
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime < 0)
            {
                _isWaiting = false;
                OnFrogCroaking?.Invoke();
            }
        }
    }
}
