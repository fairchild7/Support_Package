using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] DemoPlayer _player;
    [SerializeField] float _runSpeed = 5f;

    private Command buttonZ, buttonX, buttonC;

    private void Awake()
    {
        buttonZ = new FireCommand();
        buttonC = new JumpCommand();
        buttonX = new RunCommand(_player, _runSpeed);
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            buttonZ.Execute();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            buttonC.Execute();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            buttonX.Execute();
        }
    }
}
