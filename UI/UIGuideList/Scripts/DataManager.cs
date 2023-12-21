using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public SOGuideList guideData;

    public bool IsUnlockedGuide(int guideId)
    {
        if (PlayerPrefs.GetInt(Constants.GUIDE_UNLOCK_STATUS + guideId, 0) == 1)
        {
            return true;
        }
        else if (PlayerPrefs.GetInt(Constants.GUIDE_UNLOCK_STATUS + guideId, 0) == 0)
        {
            return false;
        }
        return false;
    }

    public void UnlockGuide(int guideId)
    {
        PlayerPrefs.SetInt(Constants.GUIDE_UNLOCK_STATUS + guideId, 1);
    }
}
