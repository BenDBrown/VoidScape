using Godot;
using System;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text;

public partial class ExternalForceManager
{
	private const float BASE_DECCELERATION = 0.4f;
	private const float RELATIVE_DECCELERATION = 0.1f;

	public Vector2 Force { get; private set; } = Vector2.Zero;

	public int Weight { get; private set; } = 1;

	public void SetWeight(int weight)
	{
		this.Weight = weight;
	}

	public Vector2 GetForce(double deltaTime)
	{
		DecayMomentum();
		return Force;
	}

	public void HandleBodyCollision(KinematicCollision2D col, double deltaTime, Vector2 velocity)
	{
		if (col.GetCollider() is RigidBody2D rb) HandleRigidBodyCollision(col, rb, deltaTime, velocity);
		else if (col.GetCollider() is Ship ship) HandleKinematicBodyCollision(col, ship);
		else if (col.GetCollider() is StaticBody2D) HandleStaticBodyCollision(col);

	}

	//Handle Collision (RigidBody)
	public void HandleRigidBodyCollision(KinematicCollision2D collision, RigidBody2D rigidBody, double deltaTime, Vector2 velocity)
	{
		Vector2 impactForceFromCollider = collision.GetNormal() * (rigidBody.LinearVelocity.Length() * rigidBody.Mass) / Weight;
		Vector2 impactForceToCollider = -collision.GetNormal() * (velocity.Length() * Weight);
		rigidBody.ApplyCentralImpulse(impactForceToCollider * (float)deltaTime);
		Force += impactForceFromCollider;
	}

	//Handle Collision (Kinematic Body) aka Ship
	public void HandleKinematicBodyCollision(KinematicCollision2D collision, Ship ship)
	{
		Vector2 impactForceFromCollider = collision.GetNormal() * (ship.Velocity.Length() * ship.Weight) / Weight;
		Force += impactForceFromCollider;
	}

	//Handle Collision (Static Body)
	public void HandleStaticBodyCollision(KinematicCollision2D collision)
	{
		Vector2 impactForceFromCollider = collision.GetNormal() / Weight;
		Force += impactForceFromCollider;
	}

	public void AddExternalImpulse(Vector2 impulse) => Force += impulse;


	private void DecayMomentum()
	{
		float baseXDecceleration;
		float baseYDecceleration;

		if (Force.X == 0) baseXDecceleration = 0;
		else baseXDecceleration = (Force.X / Force.X) * -BASE_DECCELERATION;

		if (Force.Y == 0) baseYDecceleration = 0;
		else baseYDecceleration = (Force.Y / Force.Y) * -BASE_DECCELERATION;

		Func<float, float, float> clampingFunctionX;
		if (Force.X > 0) clampingFunctionX = Math.Max;
		else clampingFunctionX = Math.Min;

		Func<float, float, float> clampingFunctionY;
		if (Force.Y > 0) clampingFunctionY = Math.Max;
		else clampingFunctionY = Math.Min;

		float newX = Force.X - (Force.X * RELATIVE_DECCELERATION) - baseXDecceleration;
		float newY = Force.Y - (Force.Y * RELATIVE_DECCELERATION) - baseYDecceleration;
		newX = clampingFunctionX.Invoke(0, newX);
		newY = clampingFunctionY.Invoke(0, newY);

		Force = new(newX, newY);
	}
}
