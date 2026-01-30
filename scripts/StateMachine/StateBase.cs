using Godot;
using System;

[GlobalClass]
public abstract partial class StateBase : Node
{
	StateTree _stateTree = null;
	public StateTree OwnerTree => _stateTree;
	public override void _Ready()
	{
		_stateTree = GetParent<StateTree>();
	}
	public abstract void Activate();
	public abstract void Deactivate();
	public abstract void Process();

}
