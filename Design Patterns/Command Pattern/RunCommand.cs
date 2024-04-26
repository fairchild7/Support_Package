using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCommand : Command
{
    private DemoPlayer _player;
    private float _speed;

    public RunCommand(DemoPlayer player, float speed)
    {
        _player = player;
        _speed = speed;
    }

    public override void Execute()
    {
        _player.Run(_speed);
    }
}

