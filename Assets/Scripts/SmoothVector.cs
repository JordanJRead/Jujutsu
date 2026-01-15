using UnityEngine;

public class SmoothVector
{
    Vector3 _curr;
    Vector3 _velocity = Vector3.zero;
    float _acceleration;

    public SmoothVector(Vector3 value, float acceleration)
    {
        _curr = value;
        _acceleration = acceleration;
    }

    public void Update(Vector3 target)
    {
        _curr = Vector3.SmoothDamp(_curr, target, ref _velocity, _acceleration);
    }

    public Vector3 GetCurrent()
    {
        return _curr;
    }
}
