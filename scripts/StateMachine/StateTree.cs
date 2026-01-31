using Godot;
using System;

[GlobalClass]
public partial class StateTree : Node
{
	private Variant _owner;

	public Variant Owner => _owner;

	[Export]
	private StateBase _activeState = null;

	public override void _Ready()
	{
		_owner = GetParent();
	}

	public override void _Process(double delta)
	{
		if (_activeState != null && IsInstanceValid(_activeState))
		{
			_activeState.Process();
		}
	}
	
	public void ChangeState(StateBase newState)
	{
		_activeState.Deactivate();
		_activeState = newState;
		_activeState.Activate();
		
	}
}
