using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{

    [SerializeField] Material _greenMatarial;
    [SerializeField] Material _redMatarial;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] GameObject _barrierOne;
    [SerializeField] private Canvas _defeatMenu;

    private GridPosition _gridPosition;
    private Action _onInteractionComplete;
    private float _timer;
    private bool _isActive;
    private bool _isGreen;

    private void Start()
    {
        SetColorRed();

        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }


        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _isActive = false;
            _onInteractionComplete();
        }
    }

    private void SetColorGreen()
    {
        _isGreen = true;
        _meshRenderer.material = _greenMatarial;
        //Destroy(_barrierOne);
        _defeatMenu.gameObject.SetActive(true);
    }

    private void SetColorRed()
    {
        _isGreen= false;
        _meshRenderer.material = _redMatarial;
        //_barrierOne.SetActive(true);
    }

    public void Interact(Action onInteractionComplete)
    {
        this._onInteractionComplete = onInteractionComplete;
        _isActive = true;
        _timer = .5f;
        if(_isGreen)
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }
    }
}
