using System;
using UnityEngine;

public class DailyTracker : Singleton<DailyTracker>
{
    private const string DATE_KEY = "LastResetDate";

    void Awake()
    {
        // Check if a new day has started and reset values if needed.
        CheckAndResetIfNewDay();
    }

    /// <summary>
    /// Checks if the current day is different from the last reset day.
    /// If it is, reset the keys to their default values.
    /// </summary>
    void CheckAndResetIfNewDay()
    {
        // Load the last reset date if it exists; otherwise, use DateTime.MinValue.
        DateTime lastReset = ES3.KeyExists(DATE_KEY) ? ES3.Load<DateTime>(DATE_KEY) : DateTime.MinValue;
        DateTime today = DateTime.Today;

        if (lastReset.Date < today)
        {
            // Reset the values if a new day has started.
            ResetDefaultValues();
            // Save the new reset date.
            ES3.Save<DateTime>(DATE_KEY, today);
            Debug.Log("Keys have been reset to default values for the new day.");
        }
    }

    /// <summary>
    /// Resets all keys to their default values.
    /// Modify this method to include any additional keys as needed.
    /// </summary>
    void ResetDefaultValues()
    {
        SetValue<int>(MyConstants.DAILY_WATCH_ADS_TIME, 6);

        // Add additional keys and default values here if needed.
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// Returns the stored value if the key exists, otherwise returns the default value of the type.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key name.</param>
    /// <returns>The value associated with the key, or default(T) if the key does not exist.</returns>
    public T GetValue<T>(string key)
    {
        if (ES3.KeyExists(key))
        {
            return ES3.Load<T>(key);
        }
        return default;
    }

    /// <summary>
    /// Sets or updates the value for the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to save.</typeparam>
    /// <param name="key">The key name.</param>
    /// <param name="value">The value to store.</param>
    public void SetValue<T>(string key, T value)
    {
        ES3.Save<T>(key, value);
    }
}
