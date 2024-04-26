using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TestPing : MonoBehaviour
{
    public static bool status = false;
    public static bool isDone = false;
    public static string ipAdd;

    public static bool PingThis()
    {
        try
        {
            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

            byte[] buffer = new byte[32];
            int timeout = 10000;
            System.Net.NetworkInformation.PingOptions pingOptions = new System.Net.NetworkInformation.PingOptions(64, true);
            System.Net.NetworkInformation.PingReply reply = ping.Send(ipAdd, timeout, buffer, pingOptions);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                return true;
            }
            else if (reply.Status == System.Net.NetworkInformation.IPStatus.TimedOut)
            {
                return status;
            }
            else
            {
                return false;
            }
        }
        catch (Exception error)
        {
            Debug.Log(error);
            return false;
        }
        finally
        {

        }
    }

    public static string GetIPAddress()
    {
        IPHostEntry host;
        host = Dns.GetHostEntry("google.com");

        try
        {
            host = Dns.GetHostEntry("google.com");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        finally
        {

        }

        foreach(IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        return string.Empty;
    }

    public static void DoPing()
    {
        ipAdd = GetIPAddress();

        if (PingThis())
        {
            if (!status)
            {
                LoggerDebug.Instance?.LogInfo("Connected");
            }
            status = true;
            isDone = true;
        }
        else
        {
            if (status)
            {
                LoggerDebug.Instance?.LogInfo("Disconnected");
            }
            status = false;
            isDone = true;
        }
    }
}

