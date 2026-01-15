using UnityEngine;

public class SmoothFloat
{
    float _curr;
    float _velocity = 0;
    float _acceleration;

    public SmoothFloat(float value, float acceleration)
    {
        _curr = value;
        _acceleration = acceleration;
    }

    public void Update(float target)
    {
        _curr = Mathf.SmoothDamp(_curr, target, ref _velocity, _acceleration);
    }

    public float GetCurrent()
    {
        return _curr;
    }
}
