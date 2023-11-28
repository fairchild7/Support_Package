using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public IState currentState;

    private void Update()
    {
        currentState.OnExecute(this);
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
}
