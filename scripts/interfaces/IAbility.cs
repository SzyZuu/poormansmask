using Godot;

namespace poormansmask.scripts.interfaces;

public interface IAbility
{
    public void Activate(PlayerController pc);
    public void Passive(PlayerController pc);
    public void ActiveAction(PlayerController pc,  InputEvent @event);
}