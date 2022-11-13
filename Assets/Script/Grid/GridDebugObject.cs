using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{

    [SerializeField] private TextMeshPro _textMeshPro;

    private object _gridObject;
    public virtual void SetGridObject(object gridObject)
    {
        this._gridObject = gridObject;
    }

    protected virtual void Update()
    {
        _textMeshPro.text = _gridObject.ToString();
    }
}
