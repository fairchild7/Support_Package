using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverExample : MonoBehaviour
{
    private int _param;

    /*
     * raise Event 1 on this function call
     * For further example, with func TakeDamage(), event EventID.OnDead can be raise in condition (hp <= 0)
     */
    public void OnTriggerEvent1()
    {
        this.PostEvent(EventID.OnTestEvent1);
    }

    /*
     * raise Event 2 with parameter _param
     * Use when a parameter is needed to be sent together with event, for example _hp
     */
    public void OnTriggerEvent2()
    {
        this.PostEvent(EventID.OnTestEvent2, _param);
    }
}
