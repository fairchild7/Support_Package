public static class StringToEnum<T>
{
    public static UnitType StringToUnitType(string stringType)
    {
        return (UnitType)System.Enum.Parse(typeof(UnitType), stringType);
    }
    
    public static SFXType StringToSFXType(string stringType)
    {
        return (SFXType)System.Enum.Parse(typeof(SFXType), stringType);
    }

    public static T StringToTEnum(string stringType)
    {
        return (T)System.Enum.Parse(typeof(T), stringType);
    }
}
