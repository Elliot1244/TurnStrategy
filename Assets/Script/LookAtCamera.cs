using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool _invert;
    private Transform _camera;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if(_invert)
        {
            Vector3 dirToCam = (_camera.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCam * -1);
        }
        else
        {
            transform.LookAt(_camera);
        }
        
    }
}
