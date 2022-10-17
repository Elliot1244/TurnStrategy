using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{

    [SerializeField] private LayerMask _mouseLayerMask;

    private static MouseWorld _instance;

    private void Awake()
    {
        _instance = this;
    }
    void Update()
    {
        transform.position = MouseWorld.GetPosition();
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, _instance._mouseLayerMask));
        return rayCastHit.point;
    }
}
