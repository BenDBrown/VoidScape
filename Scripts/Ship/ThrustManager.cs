using Godot;
using System;

public partial class ThrustManager : IPowerable
{
	private const double ACCELERATION = 0.2f;

	public Vector2 Force {get; private set;} = Vector2.Zero;

	public float PotentialForwardThrust {get; private set;} = 0;

	public float PotentialBackwardThrust {get; private set;} = 0;

	public float PotentialSideThrust {get; private set;} = 0;

	public int PowerDraw {get; private set;} = 0;

	public int weight {get; private set;} = 1; // to avoid division by 0 errors

	// values from 0-1 that determines the percent of thrust in direction
	private float forwardThrustRatio = 0; 
	private float backwardThrustRatio = 0;
	private float rightThrustRatio = 0;
	private float leftThrustRatio = 0;

	private bool thrustingForward = false;
	private bool thrustingBackward = false;
	private bool thrustingRight = false;
	private bool thrustingLeft = false;

	public ThrustManager() { }

    public Vector2 GetForce(double deltaTime)
    {
		double acceleration = ACCELERATION * deltaTime;
		double decceleration = acceleration * 2;
		// updating the thrust ratios FTR stands for forward thrust ratio, BTRO for backwards thrust ratio opposed, etc
        UpdateThrustRatio(thrustingForward, forwardThrustRatio, backwardThrustRatio, acceleration, decceleration, out double newFTR, out double newBTRO);
		forwardThrustRatio = (float)newFTR;
		backwardThrustRatio = (float)newBTRO;

		UpdateThrustRatio(thrustingBackward, backwardThrustRatio, forwardThrustRatio, acceleration, decceleration, out double newBTR, out double newFTRO);
		backwardThrustRatio = (float)newBTR;
		forwardThrustRatio = (float)newFTRO;
		
		UpdateThrustRatio(thrustingRight, rightThrustRatio, leftThrustRatio, acceleration, decceleration, out double newRTR, out double newLTRO);
		rightThrustRatio = (float)newRTR;
		leftThrustRatio = (float)newLTRO;

		UpdateThrustRatio(thrustingLeft, leftThrustRatio, rightThrustRatio, acceleration, decceleration, out double newLTR, out double newRTRO);
		leftThrustRatio = (float)newLTR;
		rightThrustRatio = (float)newRTRO;

		Vector2 forward = new(0, -forwardThrustRatio * PotentialForwardThrust);
		Vector2 backward = new(0, backwardThrustRatio * PotentialBackwardThrust);
		Vector2 right = new(rightThrustRatio * PotentialSideThrust, 0);
		Vector2 left = new(-leftThrustRatio * PotentialSideThrust, 0);
		Vector2 diagonal = Vector2.Zero;
		if(thrustingForward && thrustingLeft)
		{ 
			diagonal = GetLimitedDiagonal(forward, left, forward.Y);
			forward = Vector2.Zero;
			left = Vector2.Zero;
		}
		else if(thrustingForward && thrustingRight)
		{
			diagonal = GetLimitedDiagonal(forward, right, forward.Y);
			forward = Vector2.Zero;
			right = Vector2.Zero;
		}
		else if(thrustingBackward && thrustingLeft)
		{
			diagonal = GetLimitedDiagonal(backward, left, left.X);
			backward = Vector2.Zero;
			left = Vector2.Zero;
		}
		else if(thrustingBackward && thrustingRight)
		{
			diagonal = GetLimitedDiagonal(backward, right, right.X);
			backward = Vector2.Zero;
			right = Vector2.Zero;
		}
		Force = forward + backward + left + right + diagonal;
		// float forceMagnitude = Force.Length();
		// if(forceMagnitude <= 0) { return rotationMoveVect; }
		// // Force = rotationMoveVect.Normalized() + Force.Normalized();
		// Force = (rotationMoveVect + Force).Normalized();
		// Force *= forceMagnitude;

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

	private void UpdateThrustRatio(bool isThrusting, double ratio, double opposingRatio, double acceleration, double decceleration, out double newRatio, out double newOpposingRatio)
	{
		if(isThrusting)
		{
			ratio += acceleration;
			ratio = Math.Min(ratio, 1);
			if(opposingRatio > 0)
			{
				opposingRatio -= acceleration;
				opposingRatio = Math.Max(opposingRatio, 0);
			}
		}
		else 
		{
			if(ratio > 0)
			{
				ratio -= decceleration;
				ratio = Math.Max(ratio, 0);
			}
		}
		newRatio = ratio;
		newOpposingRatio = opposingRatio;
	}

	private Vector2 GetLimitedDiagonal(Vector2 a, Vector2 b, float length)
	{
		Vector2 c = a + b;
		c = c.Normalized();
		c *= Math.Abs(length);
		return c;
	}



}
