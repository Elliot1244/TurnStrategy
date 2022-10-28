using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{

    private float _timer;
    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        _timer -= Time.deltaTime;

        if(_timer <= 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }


    //On met le timer à deux secondes à chaque changement de tour
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        _timer = 2f;
    }
}
