using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    private void Update()
    {

        //Controle mouvement camera, on utilise GetKey pour pouvoir bouger la caméra en maintenant la touche enfoncée
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if(Input.GetKey(KeyCode.W))
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

        Debug.Log(Input.GetKey(KeyCode.G));

        transform.eulerAngles += rotationVector * _rotationSpeed * Time.deltaTime;
    }
}
