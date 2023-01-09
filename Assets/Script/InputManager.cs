#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInputActions _playerInputActions;

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

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.PlayerActions.Enable();
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputActions.PlayerActions.CameraMovement.ReadValue<Vector2>();
#else
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
#endif
    }

    public float GetCamerarotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputActions.PlayerActions.CameraRotation.ReadValue<float>();
#else
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
#endif
    }

    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputActions.PlayerActions.CameraZoom.ReadValue<float>();
#else
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
#endif
    }


    public bool IsMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
       return _playerInputActions.PlayerActions.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    
}
