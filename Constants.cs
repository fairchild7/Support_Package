using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const string PPREF_CURRENTCAR = "current_car";
    public const string PPREF_SOUNDSETTING = "sound_setting";
    public const string PPREF_VIBRATESETTING = "vibrate_setting";
    public const string PPREF_FLASHSETTING = "flash_setting";
    public const string PPREF_CARSOUNDSTAT = "sound_stat_car_";
    public const string PPREF_CARDRIVESTAT = "drive_stat_car_";

    public const string FINISHTUTORIAL = "finish_tutorial";
    public const string IS_PREMIUM = "is_premium";

    public const float FUEL_DECREASE_IDLE = 0.01f;
    public const float FUEL_DECREASE_RUN = 0.05f;
}

public struct Pl_Gift
{
    public const string HOMESCREEN = "gift_home";
    public const string SOUNDMODE = "gift_sound_mode";
    public const string REFUEL = "gift_refuel";
}

public struct IAP
{
    public const string PREMIUM = "iap_premium";
}

public struct Pl_Full
{
    public const string HOMESCREEN = "full_home";
    public const string STUNTMODE = "full_stunt_mode";
    public const string SOUNDMODE = "full_sound_mode";
}

public struct Pl_Collapse
{
    public const string HOMESCREEN = "collapse_homescreen";
    public const string SOUNDMODE = "collapse_soundmode";
    public const string STUNTSMODE = "collapse_stuntsmode";
    public const string SHOWRANK = "collapse_showrank";
    public const string SETTING = "collapse_setting";
    public const string REFUEL = "collapse_refuel";
}

public struct Pl_Rect
{
    public const string SETTING = "rect_setting";
}

public struct FIR_Config
{
    public const string CARSOUNDUNLOCKTYPE = "cf_car_sound_unlock_type_";
    public const string CARDRIVEUNLOCKTYPE = "cf_car_drive_unlock_type_";
}
