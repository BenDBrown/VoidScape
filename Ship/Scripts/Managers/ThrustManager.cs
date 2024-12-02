using Godot;
using System;
using System.Collections.Generic;

public partial class ThrustManager : IPowerable
{
	private const double ACCELERATION = 0.4f;

	public Vector2 Force { get; private set; } = Vector2.Zero;

	public float PotentialForwardThrust { get; private set; } = 0;

	public float PotentialBackwardThrust { get; private set; } = 0;

	public float PotentialSideThrust { get; private set; } = 0;

	public int PowerDraw => GetPowerDraw();

	private int powerDraw = 0;

	public int weight { get; private set; } = 1; // to avoid division by 0 errors

	private List<Thruster> thrusters = new();

	private bool thrustingForward = false;
	private bool thrustingBackward = false;
	private bool thrustingRight = false;
	private bool thrustingLeft = false;

	public ThrustManager() { }

	public Vector2 GetForce(double deltaTime, float rotation)
	{ 
		double acceleration = ACCELERATION * deltaTime;
		double decceleration = acceleration * 2;

		Vector2 momentum = DecayMomentum(Force, (float)decceleration);
		Vector2 forward = Vector2.Zero;
		Vector2 backward = Vector2.Zero;
		Vector2 right = Vector2.Zero;
		Vector2 left = Vector2.Zero;
		if(thrustingBackward || thrustingRight || thrustingLeft || thrustingForward) foreach(Thruster thruster in thrusters) { thruster.SetThrustAnimationActive(true); }
		else foreach(Thruster thruster in thrusters) { thruster.SetThrustAnimationActive(false); }

		if(thrustingForward) 
		{
			forward = GetAddedForce(Vector2.Up, acceleration, PotentialForwardThrust);
			forward = forward.Rotated(rotation);
		}
		else if(thrustingBackward) 
		{
			backward = GetAddedForce(Vector2.Down, acceleration, PotentialBackwardThrust);
			backward = backward.Rotated(rotation);
		}

		if(thrustingRight) 
		{
			right = GetAddedForce(Vector2.Right, acceleration, PotentialSideThrust);
			right = right.Rotated(rotation);
		}
		else if(thrustingLeft) // these elses are for minor performance gains and should be irrelevant if the controller is working properly
		{
			left = GetAddedForce(Vector2.Left, acceleration, PotentialSideThrust);
			left = left.Rotated(rotation);
		}

		Force = forward + backward + left + right + momentum;

		float limitedX = Math.Min(PotentialForwardThrust, Force.X); // choosing to use potential forward thrust so that it doesnt affect momentum too much
		limitedX = Math.Max(-PotentialForwardThrust, Force.X);

		float limitedY = Math.Min(PotentialForwardThrust, Force.Y);
		limitedY = Math.Max(-PotentialForwardThrust, Force.Y);
		Force = new(limitedX, limitedY);

		// current limiting methodology does not allow ship to be pushed by an object to move faster than its max speed ie Potential forward thrust

		return Force;
	}

	public void SetWeight(int weight) => this.weight = weight;

	public void StartThrustingForward()
	{
		thrustingBackward = false;
		thrustingForward = true;
	}

	public void StartThrustingBackward()
	{
		thrustingForward = false;
		thrustingBackward = true;
	}

	public void StartThrustingRight()
	{
		thrustingLeft = false;
		thrustingRight = true;
	}

	public void StartThrustingLeft()
	{
		thrustingRight = false;
		thrustingLeft = true;
	}
	public void StopThrustingForward() => thrustingForward = false;
	public void StopThrustingBackward() => thrustingBackward = false;
	public void StopThrustingRight() => thrustingRight = false;
	public void StopThrustingLeft() => thrustingLeft = false;
	public void StopThrusting()
	{
		thrustingForward = false;
		thrustingBackward = false;
		thrustingLeft = false;
		thrustingRight = false;
	}

	public void AddThruster(Thruster thruster)
	{
		PotentialForwardThrust += thruster.GetThrust();
		UpdateThrust();
		thrusters.Add(thruster);
		powerDraw += thruster.GetPowerDraw();
		thruster.OnDestroyed += OnThrusterDestroyed;
	}

	public int GetPowerDraw() 
	{
		if(thrustingBackward || thrustingLeft || thrustingRight || thrustingForward) return powerDraw;
		return 0;
	}

	private void OnThrusterDestroyed(ShipComponent shipComponent)
	{
		if (shipComponent is not Thruster thruster) { GD.PushError("non thruster ship component sent to thrust manager on destroy event"); return; }
		PotentialForwardThrust -= thruster.GetThrust();
		UpdateThrust();
		thrusters.Remove(thruster);
		powerDraw -= thruster.GetPowerDraw();
		thruster.OnDestroyed -= OnThrusterDestroyed;
	}

	private void UpdateThrust()
	{
		PotentialBackwardThrust = PotentialForwardThrust / 2;
		PotentialSideThrust = PotentialBackwardThrust * 1.5f;
	}

	private Vector2 GetAddedForce(Vector2 direction, double acceleration, float potentialForce) => direction * ((float)acceleration * potentialForce);

	private Vector2 DecayMomentum(Vector2 momentum, float decceleration)
	{
		decceleration = Math.Min(decceleration, 0.9f); // ensuring that decceleration does not get equal or higher than 1 which would result in no decceleration
		float newX = momentum.X - (momentum.X * decceleration);
		float newY = momentum.Y - (momentum.Y * decceleration);
		return new Vector2(newX, newY);
	}


}
