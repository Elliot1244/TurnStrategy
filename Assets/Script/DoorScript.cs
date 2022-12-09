using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private bool _isOpen;

    private GridPosition _gridPosition;
    private Animator _animator;
    private Action _onInteractComplete;
    private float _timer;
    private bool _isActive;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(_gridPosition, this);

        if(_isOpen == true)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void Update()
    {

        if(!_isActive)
        {
            return;
        }


        _timer -= Time.deltaTime;

        if(_timer <= 0f)
        {
            _isActive = false;
            _onInteractComplete();
        }
    }
    public void Interact(Action onInteractComplete)
    {

        _isActive = true;
        _timer = 0.5f;
        this._onInteractComplete = onInteractComplete;
        if (_isOpen == true)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }

        Debug.Log("door interaction");
    }

    private void OpenDoor()
    {
        _isOpen = true;
        _animator.SetBool("isOpen", _isOpen);
        Pathfinding.Instance.SetisWalkableGridPosition(_gridPosition, true);
    }

    private void CloseDoor()
    {
        _isOpen = false;
        _animator.SetBool("isOpen", _isOpen);
        Pathfinding.Instance.SetisWalkableGridPosition(_gridPosition, false);
    }
}
