using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int _moveSpeed;
    [SerializeField] private float _stoppingDistance;
    private Vector3 _targetPosition;


    private void Update()
    {
        if(Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            Vector3 _moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += _moveDirection * _moveSpeed * Time.deltaTime;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move(Vector3 _targetPosition)
    {
        this._targetPosition = _targetPosition;
    }

   
}
