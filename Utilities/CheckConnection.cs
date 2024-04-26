using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckConnection : Singleton<CheckConnection>
{
    private string linkURL;

    bool _connected = false;
    public bool Connected => _connected;

    public void SimpleCheckNetworkConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (_connected)
            {
                LoggerDebug.Instance?.LogInfo("Disconnected");
            }
            _connected = false;
        }
        else
        {
            if (!_connected)
            {
                LoggerDebug.Instance?.LogInfo("Connected");
            }
            _connected = true;
        }
    }

    public void CheckNetworkConnection()
    {
        string m_ReachabilityText = "";

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            m_ReachabilityText = "Not connected.";
            Debug.Log("Internet : " + m_ReachabilityText);
        }
        else
        {
            StartCoroutine(DoPing());
        }
    }

    IEnumerator DoPing()
    {
        TestPing.DoPing();
        yield return new WaitUntil(() => TestPing.isDone);
        bool connected = TestPing.status;

        if (connected)
        {
            Debug.Log("Connected");
            Debug.Log(TestPing.ipAdd);
        }
        else
        {
            Debug.Log(TestPing.ipAdd);
            Debug.Log("Please check your network connections or network permissions");
        }
    }
}
