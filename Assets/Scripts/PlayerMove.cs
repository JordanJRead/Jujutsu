using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Assertions.Must;

public class PlayerMove : MonoBehaviour
{
    public float WalkSpeed;
    public float RunSpeed;
    public float MoveAcceleration;
    public float TurnAcceleration;
    public float JumpStrength;

    // Animator
    Animator _animator;
    SmoothFloat _smoothLR;
    SmoothFloat _smoothSpeed;
    SmoothVector _smoothMoveDirection;
    bool _isRunning = true;

    // Physics
    public float Gravity;
    CharacterController _characterController;
    float _yVelocity = 0;

    PhotonView _view;
    public KeyboardState keyboardState = new KeyboardState();

    // Start is called before the first frame update
    void Start()
    {
        _smoothMoveDirection = new SmoothVector(Vector3.zero, MoveAcceleration);
        _smoothLR = new SmoothFloat(0.5f, TurnAcceleration);
        _smoothSpeed = new SmoothFloat(0, MoveAcceleration);
        _animator = GetComponent<Animator>();
        _view = GetComponent<PhotonView>();
        _characterController = GetComponent<CharacterController>();
        if (_view.IsMine)
        {
            transform.Find("Camera").GetComponent<Camera>().enabled = true;
            transform.Find("Camera").GetComponent<AudioListener>().enabled = true;
            transform.Find("Camera").GetComponent<PlayerCamera>().enabled = true;
        }
    }

    void UpdateKeyboardState()
    {

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
    }

    (Vector3, Vector3) GetTargetAndLocalMoveDirection()
    {
        Vector3 targetMoveDirection = Vector3.zero;
        Vector3 localMoveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            targetMoveDirection += transform.forward;
            localMoveDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            targetMoveDirection -= transform.forward;
            localMoveDirection -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetMoveDirection += transform.right;
            localMoveDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            targetMoveDirection -= transform.right;
            localMoveDirection -= Vector3.right;
        }
        targetMoveDirection.Normalize();
        localMoveDirection.Normalize();

        return (targetMoveDirection, localMoveDirection);
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
            // Input direction
            UpdateKeyboardState();
            var (targetMoveDirection, localMoveDirection) = GetTargetAndLocalMoveDirection();
            _smoothMoveDirection.Update(targetMoveDirection);
            _smoothLR.Update(localMoveDirection.x / 2 + 0.5f);

            // Gravity
            _yVelocity += Gravity * Time.deltaTime;
            if (_characterController.isGrounded)
            {
                _yVelocity = -0.2f;
            }

            // Move speed
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _isRunning = !_isRunning;
            }

            if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
            {
                _yVelocity = JumpStrength;
                _animator.SetBool("Jump", true);
            }

            float targetSpeed = 0;
            if (targetMoveDirection.magnitude > 0)
            {
                targetSpeed = _isRunning ? RunSpeed : WalkSpeed;
            }
            _smoothSpeed.Update(targetSpeed);
            float speed = _smoothMoveDirection.GetCurrent().magnitude * _smoothSpeed.GetCurrent();

            // Final velocity
            Vector3 velocity = _smoothMoveDirection.GetCurrent() * speed + Vector3.up * _yVelocity;

            _characterController.Move(velocity * Time.deltaTime);
            speed = new Vector2(_characterController.velocity.x, _characterController.velocity.z).magnitude;

            // Animator values
            _animator.SetFloat("Speed", speed < WalkSpeed ? (speed / WalkSpeed / 2) : ((speed - WalkSpeed) / (RunSpeed - WalkSpeed) / 2 + 0.5f));
            _animator.SetFloat("LeftRight", _smoothLR.GetCurrent());
            _animator.SetBool("IsGrounded", _characterController.isGrounded);
        }
    }
}
