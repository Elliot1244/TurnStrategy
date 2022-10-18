using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private Animator _animator;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            _animator.SetBool("isWalking", true);
            Vector3 _moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += _moveDirection * _moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, _moveDirection, Time.deltaTime * _rotateSpeed);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }

    public void Move(Vector3 _targetPosition)
    {
        this._targetPosition = _targetPosition;
    }

   
}
