using Godot;
using System;

[GlobalClass]
public abstract partial class StateBase : Node
{
	private StateTree _stateTree = null;
	public StateTree OwnerTree 
	{ 
		get 
		{
			if (_stateTree == null)
			{
				var parent = GetParent();
				while (IsInstanceValid(parent) && parent is not StateTree)
					parent = parent.GetParent();
				_stateTree = parent as StateTree;
			}
			return _stateTree;
		} 
	}
	public abstract void Activate();
	public abstract void Deactivate();
	public abstract void Process(float delta);
	public abstract void PhysicsProcess(float delta);
}
