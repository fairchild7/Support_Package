using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
    public void Run(float speed)
    {
        Debug.Log("Player is running at " + speed.ToString() + " speed!");
    }
}
