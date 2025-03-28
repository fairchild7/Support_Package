using System;
using UnityEngine;

public class CountdownManager : Singleton<CountdownManager>
{
    /// <summary>
    /// Starts a countdown for a specific key by saving the target end time.
    /// </summary>
    /// <param name="key">The key identifier for the countdown (e.g., MyConstants.START_WATCH_ADS_FOR_COIN_TIME)</param>
    /// <param name="time">The duration to wait in seconds</param>
    public void StartCountdown(string key, float time)
    {
        DateTime targetTime = DateTime.Now.AddSeconds(time);
        ES3.Save<DateTime>(key, targetTime);
    }

    /// <summary>
    /// Gets the remaining time in seconds for a countdown with the given key.
    /// Returns 0 if the countdown does not exist or has expired.
    /// </summary>
    /// <param name="key">The key identifier for the countdown</param>
    /// <returns>The remaining seconds of the countdown</returns>
    public float GetCountdownTimerLeft(string key)
    {
        if (ES3.KeyExists(key))
        {
            DateTime targetTime = ES3.Load<DateTime>(key);
            float remainingSeconds = (float)(targetTime - DateTime.Now).TotalSeconds;
            return Mathf.Max(remainingSeconds, 0);
        }

        return 0f;
    }
}
