using poormansmask.scripts.enums;

namespace poormansmask.scripts;

public static class StatNameHelper
{
    public static string GetDisplayString(StatImprovements stat)
    {
        switch (stat)
        {
            case StatImprovements.MAXHEALTH:
                return "max hp";
            case StatImprovements.MAXARMOR:
                return "armor";
            case StatImprovements.DAMAGEMULTIPLIER:
                return "% dmg";
            default:
                return "something no yes";
        }
    }
}