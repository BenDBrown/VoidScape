using Godot;
using System;

public partial class ThrustManager : Node, IPowerable
{
	private const float ACCELERATION = 0.2f;

	public Vector2 Thrust {get; private set;} = Vector2.Zero;

	public float PotentialForwardThrust {get; private set;} = 0;

	public float PotentialBackwardThrust {get; private set;} = 0;

	public float PotentialSideThrust {get; private set;} = 0;

	public int PowerDraw {get; private set;} = 0;

	private int weight = 1; // to avoid division by 0 errors

	// values from 0-1 that determines the percent of thrust in direction
	private float forwardThrustRatio = 0; 
	private float backwardThrustRatio = 0;
	private float rightThrustRatio = 0;
	private float leftThrustRatio = 0;

	// shorthand
	private float PotentialDiagonalForwardThrust => (PotentialForwardThrust + PotentialSideThrust) / 2;
	private float PotentialDiagonalBackwardThrust => (PotentialBackwardThrust + PotentialSideThrust) / 2;
	private bool thrustingForward = false;
	private bool thrustingBackward = false;
	private bool thrustingRight = false;
	private bool thrustingLeft = false;

	public ThrustManager() { }

    public override void _PhysicsProcess(double delta)
    {
        if(thrustingForward) {forwardThrustRatio += ACCELERATION * (float)delta;}
    }

    public void SetWeight(int weight) => this.weight = weight;

	public void StartThrustingForward()
	{
		thrustingBackward = false;
		thrustingForward = true;
		Vector2 direction;
		if(thrustingRight) { direction = (Vector2.Up + Vector2.Right).Normalized(); }
		else if(thrustingLeft) { direction = (Vector2.Up + Vector2.Left).Normalized(); }
		else { direction = Vector2.Up; }

	}

	public void StartThrustingBackward()
	{
		
	}

	public void StartThrustingRight()
	{
		
	}

	public void StartThrustingLeft()
	{
		
	}

	public void StopThrustingForward()
	{

	}

	public void StopThrustingBackward()
	{
		
	}

	public void StopThrustingRight()
	{
		
	}
	
	public void StopThrustingLeft()
	{
		
	}

	public void AddThruster(Thruster thruster)
	{
		PotentialForwardThrust += thruster.GetThrust();
		UpdateThrust();
		PowerDraw += thruster.GetPowerDraw();
		thruster.OnDestroyed += OnThrusterDestroyed;
	}

	public int GetPowerDraw() => PowerDraw;

	private void OnThrusterDestroyed(ShipComponent shipComponent)
	{
		if(!(shipComponent is Thruster thruster)) { GD.PushError("non thruster ship component sent to thrust manager on destroy event"); return; }
		PotentialForwardThrust -= thruster.GetThrust();
		UpdateThrust();
		PowerDraw -= thruster.GetPowerDraw();
		thruster.OnDestroyed -= OnThrusterDestroyed;
	}

	private void UpdateThrust()
	{
		PotentialBackwardThrust = PotentialForwardThrust / 2;
		PotentialSideThrust = PotentialBackwardThrust * 1.5f;
	}

}
