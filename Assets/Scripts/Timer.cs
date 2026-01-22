using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Timer
{
    [SerializeField] private float _currentTime;
    private float _startTime;

    public Timer(float startTime = 0)
    {
        _currentTime = startTime;
        _startTime = startTime;
    }

    /// <summary>
    /// Update the time stored in the timer
    /// </summary>
    /// <returns>Whether the timer just reached 0 or not</returns>
    public bool Update(float deltaTime)
    {
        float prevTime = _currentTime;
        _currentTime -= deltaTime;
        _currentTime = Mathf.Max(_currentTime, 0);

        return prevTime > 0 && _currentTime == 0;
    }

    public void ResetTime(float time)
    {
        _currentTime = time;
        _startTime = time;
    }

    public bool IsDone() { return _currentTime == 0; }
    public float GetT() { return (_startTime - _currentTime) / _startTime; }
}
