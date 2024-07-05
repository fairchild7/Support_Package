using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    private void Awake()
    {
        CheckNewDayLogin();

        DateTime currentTime = DateTime.Now;
        DataManager.Ins.SetLastLoginDate(currentTime.ToString());
    }

    public bool CheckNewDayLogin()
    {
        DateTime currentTime = DateTime.Now;
        Debug.Log("Current login: " + Utilities.FormatDateTime(currentTime));

        DateTime lastLoginTime = DateTime.Now;
        if (DataManager.Ins.GetLastLoginDate() != "")
        {
            lastLoginTime = DateTime.Parse(DataManager.Ins.GetLastLoginDate());
        }
        else
        {
            lastLoginTime = DateTime.Now;
        }

        Debug.Log("Last login: " + Utilities.FormatDateTime(lastLoginTime));

        DateTime nextClaimTime = lastLoginTime.AddDays(1).Date;
        Debug.Log("Next day: " + Utilities.FormatDateTime(nextClaimTime));
        if (currentTime >= nextClaimTime)
        {
            Debug.Log("New day!");
            DataManager.Ins.AddSpin(1);
            return true;
        }
        else
        {
            Debug.Log("Still old day..");
            return false;
        }
    }
}
