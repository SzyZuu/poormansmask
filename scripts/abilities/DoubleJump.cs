using poormansmask.scripts.interfaces;

namespace poormansmask.scripts.abilities;

public class DoubleJump : IAbility
{
    public void Activate(PlayerController pc)
    {
        pc.AddJumps(1);
    }

    public void Passive(PlayerController pc)
    {
    }

    public void ActiveAction(PlayerController pc)
    {
    }
}