using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    //public CS_CarData carData;
    //public CharacterData characterData;
    //public CarKeyData carKeyData;

    //private void Awake()
    //{
    //    for (int i = 0; i < carData.Length; i++)
    //    {
    //        SetCarUnlockType(i);
    //        SetCarDriveUnlockType(i);
    //    }
    //    FIRParserOtherConfig.parserInGameConfig();
    //}

    //public void SetCarUnlockType(int carId)
    //{
    //    PlayerPrefs.SetString(FIR_Config.CARSOUNDUNLOCKTYPE + carId.ToString(), carData.GetCar(carId).unlockType.ToString());
    //}

    //public string GetCarUnlockType(int carId)
    //{
    //    return PlayerPrefs.GetString(FIR_Config.CARSOUNDUNLOCKTYPE + carId.ToString(), "Free");
    //}

    //public void SetCarDriveUnlockType(int carId)
    //{
    //    PlayerPrefs.SetString(FIR_Config.CARDRIVEUNLOCKTYPE + carId.ToString(), carData.GetCar(carId).unlockDriveType.ToString());
    //}

    //public string GetCarDriveUnlockType(int carId)
    //{
    //    return PlayerPrefs.GetString(FIR_Config.CARDRIVEUNLOCKTYPE + carId.ToString(), "Free");
    //}

    //public void SaveCurrentCar(int carId)
    //{
    //    PlayerPrefs.SetInt(Constants.PPREF_CURRENTCAR, carId);
    //    PlayerPrefs.Save();
    //}

    //public int GetCurrentCar()
    //{
    //    return PlayerPrefs.GetInt(Constants.PPREF_CURRENTCAR, 0);
    //}

    //public bool IsUnlockCar(int carId)
    //{
    //    //if (carData.GetCar(carId).unlockType == CS_UnlockType.Free)
    //    if (GetCarUnlockType(carId) == CS_UnlockType.Free.ToString())
    //    {
    //        return true;
    //    }
    //    if (PlayerPrefs.GetInt(Constants.PPREF_CARSOUNDSTAT + carId.ToString(), 0) == 1)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //public void UnlockCar(int carId)
    //{
    //    PlayerPrefs.SetInt(Constants.PPREF_CARSOUNDSTAT + carId.ToString(), 1);
    //}

    //public void UnlockAll()
    //{
    //    UnlockAllCar();
    //    UnlockAllCarDriveMode();
    //}

    //public void UnlockAllCar()
    //{
    //    for (int i = 0; i < carData.Length; i++)
    //    {
    //        UnlockCar(i);
    //    }
    //}

    //public void UnlockAllCarDriveMode()
    //{
    //    for (int i = 0; i < carData.Length; i++)
    //    {
    //        UnlockCarDriveMode(i);
    //    }
    //}

    //public bool IsUnlockCarDriveMode(int carId)
    //{
    //    //if (carData.GetCar(carId).unlockDriveType == UnlockDriveType.Free)
    //    if (GetCarDriveUnlockType(carId) == UnlockDriveType.Free.ToString())
    //    {
    //        return true;
    //    }
    //    if (PlayerPrefs.GetInt(Constants.PPREF_CARDRIVESTAT + carId.ToString(), 0) == 1)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //public void UnlockCarDriveMode(int carId)
    //{
    //    PlayerPrefs.SetInt(Constants.PPREF_CARDRIVESTAT + carId.ToString(), 1);
    //}

    //public int CurrentLevelIndexCar(int carIndex)
    //{
    //    return PlayerPrefs.GetInt("CAR_MAP_LEVEL_" + carIndex.ToString(), 1);
    //}
    //public void UpCurrentLevelIndexCar(int carIndex)
    //{
    //    PlayerPrefs.SetInt("CAR_MAP_LEVEL_" + carIndex.ToString(), CurrentLevelIndexCar(carIndex) + 1);
    //}

    //public bool GetSoundSetting()
    //{
    //    if (PlayerPrefs.GetInt(Constants.PPREF_SOUNDSETTING, 1) == 1)
    //    {
    //        return true;
    //    }
    //    else if (PlayerPrefs.GetInt(Constants.PPREF_SOUNDSETTING, 1) == 0)
    //    {
    //        return false;
    //    }
    //    return true;
    //}

    //public void SaveSoundSetting(bool state)
    //{
    //    if (state)
    //    {
    //        PlayerPrefs.SetInt(Constants.PPREF_SOUNDSETTING, 1);
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt(Constants.PPREF_SOUNDSETTING, 0);
    //    }
    //}

    //public bool GetVibrateSetting()
    //{
    //    if (PlayerPrefs.GetInt(Constants.PPREF_VIBRATESETTING, 1) == 1)
    //    {
    //        return true;
    //    }
    //    else if (PlayerPrefs.GetInt(Constants.PPREF_VIBRATESETTING, 1) == 0)
    //    {
    //        return false;
    //    }
    //    return true;
    //}

    //public void SaveVibrateSetting(bool state)
    //{
    //    if (state)
    //    {
    //        PlayerPrefs.SetInt(Constants.PPREF_VIBRATESETTING, 1);
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt(Constants.PPREF_VIBRATESETTING, 0);
    //    }
    //}

    //public bool GetFlashSetting()
    //{
    //    if (PlayerPrefs.GetInt(Constants.PPREF_FLASHSETTING, 1) == 1)
    //    {
    //        return true;
    //    }
    //    else if (PlayerPrefs.GetInt(Constants.PPREF_FLASHSETTING, 1) == 0)
    //    {
    //        return false;
    //    }
    //    return true;
    //}

    //public void SaveFlashSetting(bool state)
    //{
    //    if (state)
    //    {
    //        PlayerPrefs.SetInt(Constants.PPREF_FLASHSETTING, 1);
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt(Constants.PPREF_FLASHSETTING, 0);
    //    }
    //}

    //public bool IsFinishTutorial()
    //{
    //    if (PlayerPrefs.GetInt(Constants.FINISHTUTORIAL, 0) == 1)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //public void FinishTutorial()
    //{
    //    PlayerPrefs.SetInt(Constants.FINISHTUTORIAL, 1);
    //}

    //public bool IsPremium()
    //{
    //    if (PlayerPrefs.GetInt(Constants.IS_PREMIUM, 0) == 1)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    } 
    //}

    //public void BuyPremium()
    //{
    //    PlayerPrefs.SetInt(Constants.IS_PREMIUM, 1);
    //}
}
