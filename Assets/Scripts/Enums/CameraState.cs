using System;

public enum CameraState
{
    BASEFOLLOW,
    ZOOM,
    JUMP,
    FALLING,
    FREE
}


[Serializable]
public class CameraOffset
{
    public CameraState state;
    public UnityEngine.Vector3 offset;
    public float followTime;
}