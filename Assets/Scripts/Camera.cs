using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform CameraTarget;
    public Transform Player;
    public float Sensitivity;
    public float DistanceFromPlayer;

    private float _yaw = 0;
    private float _pitch = 45;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");

        _yaw -= mouseX * Sensitivity;
        _pitch -= mousey * Sensitivity;
        _pitch = Mathf.Clamp(_pitch, -80, 80);

        Vector3 unitSpherePosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * _yaw), 0, Mathf.Sin(Mathf.Deg2Rad * _yaw));
        unitSpherePosition = Vector3.RotateTowards(unitSpherePosition, Vector3.up, Mathf.Deg2Rad * _pitch, 0);

        transform.position = CameraTarget.position + unitSpherePosition * DistanceFromPlayer;
        transform.LookAt(CameraTarget.position);

        Vector3 playerLookDirection = (new Vector3(unitSpherePosition.x, 0, unitSpherePosition.z) * -1).normalized;
        Player.forward = playerLookDirection;
    }
}
