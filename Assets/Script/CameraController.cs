using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    //Déclaration valeur min/max du scroll possible
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _zoomAmount;
    [SerializeField] private float _zoomSpeed;

    private Vector3 _targetFollowOffset;
    private CinemachineTransposer _cinemachineTransposer;

    private void Start()
    {
        //Lecture du component scroll de la souris
        _cinemachineTransposer  = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }
    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    //Déplacement
    private void HandleMovement()
    {
        //Controle mouvement camera, on utilise GetKey pour pouvoir bouger la caméra en maintenant la touche enfoncée
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        //Permet de déplacer la caméra selon son angle de rotation
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * _moveSpeed * Time.deltaTime;
    }

    //Rotation
    private void HandleRotation()
    {
        //Roatation de la camera
        Vector3 rotationVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.X))
        {
            rotationVector.y = +1f;
        }

        if (Input.GetKey(KeyCode.C))
        {
            rotationVector.y = -1f;
        }

        transform.eulerAngles += rotationVector * _rotationSpeed * Time.deltaTime;
    }

    //Zoom
    private void HandleZoom()
    {
        //Zoom camera
        if (Input.mouseScrollDelta.y > 0)
        {

            _targetFollowOffset.y -= _zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {

            _targetFollowOffset.y += _zoomAmount;
        }

        //Check valeur du scroll est entre valeurs min et max possible avant récupération de la valeur
        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        //Application de la nouvelle valeur de la position du scroll
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * _zoomSpeed);
    }
}
