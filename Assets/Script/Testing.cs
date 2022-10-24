using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    [SerializeField] private Unit _unit;
    private void Start()
    {

    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.T))
        {
            GridSystemVisual.Instance.HideAllGridPosition();
            GridSystemVisual.Instance.ShowGridPositionList(
              _unit.GetMoveAction().GetValidActionGridPosition()
            );
        }
    }
}
