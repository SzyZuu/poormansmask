using poormansmask.scripts.enums;

namespace poormansmask.scripts;

public static class AbilityDescHelper
{
    public static string GetAbilityDesc(Abilities ability)
    {
        switch (ability)
        {
            case Abilities.JUMPADD:
                return "Double jump";
            case Abilities.DOUBLESHOT:
                return "Double shot";
            case Abilities.TELEPORT:
                return "Teleport to predefined position";
            default:
                return "";
        }
    }
}