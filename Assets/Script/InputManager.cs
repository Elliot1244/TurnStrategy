using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        //Vérifie qu'il n'y ait qu'une instance sinon destruction
        if (Instance != null)
        {
            Debug.LogError("More than one InputManager" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Vector2 GetMouseScreenPosition()
    {
        return Input.mousePosition;
    }

    public Vector2 GetCameraMoveVector()
    {
        //Controle mouvement camera, on utilise GetKey pour pouvoir bouger la caméra en maintenant la touche enfoncée
        Vector2 inputMoveDir = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        return inputMoveDir;
    }

    public float GetCamerarotateAmount()
    {
        float rotateAmount = 0f;

        if (Input.GetKey(KeyCode.X))
        {
            rotateAmount = +1f;
        }

        if (Input.GetKey(KeyCode.C))
        {
            rotateAmount = -1f;
        }

        return rotateAmount;
    }

    public float GetCameraZoomAmount()
    {
        float zoomAmount = 0f;

        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = +1f;
        }

        return zoomAmount;
    }


    public bool IsMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    
}
