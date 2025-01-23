using Godot;
using System;
using System.Collections.Generic;

public partial class ThrustManager
{
	private const double ACCELERATION = 0.4f;

	public delegate void ThrustChangedEventHandler(bool active);

	public event ThrustChangedEventHandler ForwardThrustChanged;

	public event ThrustChangedEventHandler BackwardThrustChanged;

	public event ThrustChangedEventHandler RightThrustChanged;

	public event ThrustChangedEventHandler LeftThrustChanged;

	public Vector2 Force { get; private set; } = Vector2.Zero;

	public float PotentialForwardThrust { get; private set; } = 0;

	public float PotentialBackwardThrust { get; private set; } = 0;

	public float PotentialRightThrust { get; private set; } = 0;

	public float PotentialLeftThrust { get; private set; } = 0;

	public int FuelUsage => GetFuelUsage();

	// this is wrong atm cuz it is total fuel usage for all thrusters even when not all of them are in use
	// will see if i can fix tmrw
	private int fuelUsage = 0;

	public int Weight { get; private set; } = 1; // to avoid division by 0 errors

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

		if (thrustingForward)
		{
			forward = Vector2.Up.Rotated(rotation);
			float currentForceInDirection = Force.Dot(forward);
			forward = GetAddedForce(forward, acceleration, PotentialForwardThrust, currentForceInDirection);
		}
		else if (thrustingBackward)
		{
			backward = Vector2.Down.Rotated(rotation);
			float currentForceInDirection = Force.Dot(backward);
			backward = GetAddedForce(backward, acceleration, PotentialBackwardThrust, currentForceInDirection);
		}
		if (thrustingRight)
		{
			right = Vector2.Right.Rotated(rotation);
			float currentForceInDirection = Force.Dot(right);
			right = GetAddedForce(right, acceleration, PotentialRightThrust, currentForceInDirection);
		}
		else if (thrustingLeft) // these elses are for minor performance gains and should be irrelevant if the controller is working properly
		{
			left = Vector2.Left.Rotated(rotation);
			float currentForceInDirection = Force.Dot(left);
			left = GetAddedForce(left, acceleration, PotentialLeftThrust, currentForceInDirection);
			
		}

		Force = forward + backward + left + right + momentum;
		// current limiting methodology does not allow ship to be pushed by an object to move faster than its max speed ie Potential forward thrust

		return Force;
	}

	public void SetWeight(int weight) => this.Weight = weight;

	public void StartThrustingForward()
	{
		thrustingBackward = false;
		thrustingForward = true;
		ForwardThrustChanged?.Invoke(thrustingForward);
	}

	public void StartThrustingBackward()
	{
		thrustingForward = false;
		thrustingBackward = true;
		BackwardThrustChanged?.Invoke(thrustingBackward);
	}

	public void StartThrustingRight()
	{
		thrustingLeft = false;
		thrustingRight = true;
		RightThrustChanged?.Invoke(thrustingRight);
	}

	public void StartThrustingLeft()
	{
		thrustingRight = false;
		thrustingLeft = true;
		LeftThrustChanged?.Invoke(thrustingLeft);
	}
	public void StopThrustingForward() 
	{
		thrustingForward = false;
		ForwardThrustChanged?.Invoke(thrustingForward);
	}
	public void StopThrustingBackward()
	{
		thrustingBackward = false;
		BackwardThrustChanged?.Invoke(thrustingBackward);
	}
	public void StopThrustingRight()
	{
		thrustingRight = false;
		RightThrustChanged?.Invoke(thrustingRight);
	}
	public void StopThrustingLeft()
	{
		thrustingLeft = false;
		LeftThrustChanged?.Invoke(thrustingLeft);
	}
	public void StopThrusting()
	{
		thrustingForward = false;
		thrustingBackward = false;
		thrustingLeft = false;
		thrustingRight = false;
	}

	public void AddThruster(Thruster thruster)
	{
		if(thruster.ThrustDirection == Vector2.Up) 
		{
			PotentialForwardThrust += thruster.GetThrust();
			ForwardThrustChanged += thruster.SetThrustAnimationActive;
		}
		else if (thruster.ThrustDirection == Vector2.Down)
		{
			PotentialBackwardThrust += thruster.GetThrust();
			BackwardThrustChanged += thruster.SetThrustAnimationActive;
		}
		else if (thruster.ThrustDirection == Vector2.Right) 
		{
			PotentialRightThrust += thruster.GetThrust();
			RightThrustChanged += thruster.SetThrustAnimationActive;
		}
		else if (thruster.ThrustDirection == Vector2.Left) 
		{
			PotentialLeftThrust += thruster.GetThrust();
			LeftThrustChanged += thruster.SetThrustAnimationActive;
		}
		else GD.PrintErr($"thruster direction was not in cardinal direction vector: {thruster.ThrustDirection.X},{thruster.ThrustDirection.Y}");
		
		thrusters.Add(thruster);
		fuelUsage += thruster.GetFuelUsage();
		thruster.OnDestroyed += OnThrusterDestroyed;
	}

	public int GetFuelUsage()
	{
		if (thrustingBackward || thrustingLeft || thrustingRight || thrustingForward) return fuelUsage;
		return 0;
	}

	public void KillMomentum() => Force = new(0, 0);

	public void RemoveForceInDirection(Vector2 direction)
	{
		direction = direction.Normalized();
		float dot = Force.Dot(direction);
		if (dot <= 0) return;
		Force -= direction * dot;
	}

	public void Reset()
	{
		ForwardThrustChanged = null;
		BackwardThrustChanged = null;
		RightThrustChanged = null;
		LeftThrustChanged = null;
		thrusters.Clear();
		PotentialForwardThrust = 0;
		PotentialBackwardThrust = 0;
		PotentialRightThrust = 0;
		PotentialLeftThrust = 0;
		fuelUsage = 0;
	}

	private void OnThrusterDestroyed(ShipComponent shipComponent)
	{
		if (shipComponent is not Thruster thruster) { GD.PushError("non thruster ship component sent to thrust manager on destroy event"); return; }
		if(thruster.ThrustDirection == Vector2.Up) 
		{
			PotentialForwardThrust -= thruster.GetThrust();
			ForwardThrustChanged -= thruster.SetThrustAnimationActive;
		}
		else if (thruster.ThrustDirection == Vector2.Down)
		{
			PotentialBackwardThrust -= thruster.GetThrust();
			BackwardThrustChanged -= thruster.SetThrustAnimationActive;
		}
		else if (thruster.ThrustDirection == Vector2.Right) 
		{
			PotentialRightThrust -= thruster.GetThrust();
			RightThrustChanged -= thruster.SetThrustAnimationActive;
		}
		else if (thruster.ThrustDirection == Vector2.Left) 
		{
			PotentialLeftThrust -= thruster.GetThrust();
			LeftThrustChanged -= thruster.SetThrustAnimationActive;
		}
		else GD.PrintErr($"thruster direction was not in cardinal direction vector: {thruster.ThrustDirection.X},{thruster.ThrustDirection.Y}");
		thruster.SetThrustAnimationActive(false);
		thrusters.Remove(thruster);
		fuelUsage -= thruster.GetFuelUsage();
		thruster.OnDestroyed -= OnThrusterDestroyed;
	}

	private Vector2 GetAddedForce(Vector2 direction, double acceleration, float potentialForce, float currentForceInDirection) 
	{
		Vector2 addedForce = direction * ((float)acceleration * potentialForce);
		addedForce = addedForce.LimitLength(potentialForce - currentForceInDirection);
		return addedForce;
	}

	private Vector2 DecayMomentum(Vector2 momentum, float decceleration)
	{
		decceleration = Math.Min(decceleration, 0.9f); // ensuring that decceleration does not get equal or higher than 1 which would result in no decceleration
		float newX = momentum.X - (momentum.X * decceleration);
		float newY = momentum.Y - (momentum.Y * decceleration);
		return new Vector2(newX, newY);
	}


}
