using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This can be an UI for updating text whenever event occur, or anything need to be handled
 */
public class EventReceiveExample : MonoBehaviour
{
    private void Start()
    {
        this.RegisterListener(EventID.OnTestEvent1, (param) => HandleEvent1());
        this.RegisterListener(EventID.OnTestEvent2, (param) => HandleEvent2((int)param));
    }

    private void HandleEvent1()
    {
        //what if event 1 occur?
    }

    private void HandleEvent2(int param)
    {

    }
}
