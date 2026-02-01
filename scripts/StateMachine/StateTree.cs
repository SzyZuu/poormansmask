using Godot;
using System;

[GlobalClass]
public partial class StateTree : Node
{
	private Node _owner;

	public new Node Owner => _owner;

	[Export]
	private StateBase _activeState = null;

	public override void _Ready()
	{
		_owner = GetParent();
	}

	public override void _Process(double delta)
	{
		_owner = GetParent();

		if (_activeState != null && IsInstanceValid(_activeState))
			_activeState.Process((float)delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_activeState != null && IsInstanceValid(_activeState))
			_activeState.PhysicsProcess((float)delta);
	}
	
	public void ChangeState(StateBase newState)
	{
		if (!IsInstanceValid(newState))
		{
			return;
		}
		if (_activeState != null && IsInstanceValid(_activeState))
		{
			_activeState.Deactivate();
			GD.Print(_activeState.Name + " changed " + newState.Name);
		}

		_activeState = newState;
		_activeState.Activate();
	}
}
