using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainShell : MonoBehaviour
{
    [SerializeField] private float _castTime;
    [SerializeField] private float _radius;

    Timer _timer = new Timer();
    MaterialPropertyBlock _props;
    Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _props = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        _renderer.GetPropertyBlock(_props);
        _timer.ResetTime(_castTime);
        transform.localScale = Vector3.one * _radius * 2;
    }

    // Update is called once per frame
    void Update()
    {
        _timer.Update(Time.deltaTime);
        _props.SetFloat("_T", _timer.GetT());
        _renderer.SetPropertyBlock(_props);
    }

    public bool IsClosed()
    {
        return _timer.IsDone();
    }
    
    public float GetT()
    {
        return _timer.GetT();
    }
}
