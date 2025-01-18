namespace EternalDefenders
{
    public enum ModifierType
    {
        Flat, // eg, health + 10
        PercentAdd // eg, health + 10%, but it is summed with all modifiers of the same stat
        //For now, we will only implement these two types of modifiers, later we can get
        //PercentMultiply
    }
}