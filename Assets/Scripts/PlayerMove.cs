using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using Photon.Pun;

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
    Animator _animator;

    PhotonView _view;
    public KeyboardState keyboardState = new KeyboardState();

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _view = GetComponent<PhotonView>();
        if (_view.IsMine)
        {
            transform.Find("Camera").GetComponent<Camera>().enabled = true;
            transform.Find("Camera").GetComponent<AudioListener>().enabled = true;
            transform.Find("Camera").GetComponent<PlayerCamera>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator.GetBool("IsStunned"))
        {
            _animator.SetFloat("Speed", 0);
            _animator.SetFloat("LeftRight", 0.5f);
            return;
        }

        // Input
        if (_view.IsMine)
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

            if (Input.GetKey(KeyCode.Mouse0))
            {
                keyboardState.LeftClick = true;
            }
            else
            {
                keyboardState.LeftClick = false;
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                keyboardState.RightClick = true;
            }
            else
            {
                keyboardState.RightClick = false;
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

            _animator.SetFloat("Speed", _currSpeed < WalkSpeed ? (_currSpeed / WalkSpeed / 2) : ((_currSpeed - WalkSpeed) / (RunSpeed - WalkSpeed) / 2 + 0.5f));
            _animator.SetFloat("LeftRight", _currLR);
            transform.position += moveDirection * _currSpeed * Time.deltaTime;
        }
    }
}
