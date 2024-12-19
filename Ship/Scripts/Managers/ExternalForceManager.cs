using Godot;
using System;

public partial class ExternalForceManager
{
	public Vector2 Force {get; private set;} = Vector2.Zero;

	public Vector2 GetForce(double delta)
	{
		


		DecayMomentum();
		return Force;
	}

	private void DecayMomentum()
	{

	}
}
