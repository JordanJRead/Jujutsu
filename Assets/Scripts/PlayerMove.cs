using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float WalkSpeed;
    public float RunSpeed;
    public float MoveAcceleration;
    public float TurnAcceleration;

    float _currSpeed;
    float _smoothSpeedVelocity;

    float _currLR;
    float _smoothLRVelocity;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetSpeed = 0;
        Vector3 moveDirection = Vector3.zero;
        Vector3 localMoveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward;
            localMoveDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= transform.forward;
            localMoveDirection -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right;
            localMoveDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= transform.right;
            localMoveDirection -= Vector3.right;
        }

        if (moveDirection.magnitude > 0)
        {
            moveDirection.Normalize();
            localMoveDirection.Normalize();
            if (Input.GetKey(KeyCode.LeftShift))
                targetSpeed = RunSpeed;
            else
                targetSpeed = WalkSpeed;
        }

        _currSpeed = Mathf.SmoothDamp(_currSpeed, targetSpeed, ref _smoothSpeedVelocity, MoveAcceleration);
        _currLR = Mathf.SmoothDamp(_currLR, localMoveDirection.x / 2 + 0.5f, ref _smoothLRVelocity, TurnAcceleration);

        animator.SetFloat("Speed", _currSpeed < WalkSpeed ? (_currSpeed / WalkSpeed / 2) : ((_currSpeed - WalkSpeed) / (RunSpeed - WalkSpeed) / 2 + 0.5f) );
        animator.SetFloat("LeftRight", _currLR);
        transform.position += moveDirection * _currSpeed * Time.deltaTime;
    }
}
