using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Ship : CharacterBody2D, IShip
{
	[Export]
	private bool buildOnStart = false;
	protected ThrustManager thrustManager = new();
	protected RotationManager rotationManager = new();
	protected GunManager gunManager = new();
	protected List<ShipComponent> shipComponents = new();

	public override void _Ready()
	{
		base._Ready();
		if (buildOnStart)
		{
			TryBuildShip();
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		Rotation = rotationManager.GetRotation(Rotation, 3, delta, out Vector2 rotVector);
		Vector2 force = thrustManager.GetForce(delta, Rotation);
		Velocity = force;
		MoveAndSlide();
	}

	public void ShipDestroyed()
	{
		GD.Print("ship destroyed");
	}

	public void ComponentDestroyed(ShipComponent shipComponent)
	{
		thrustManager.SetWeight(thrustManager.weight - 1);
		GD.Print(shipComponent.Name + " destroyed");
	}

	// shooting
	public void StartShooting() => gunManager.StartShooting();
	public void StopShooting() => gunManager.StopShooting();

	// movement
	public void StartThrustingForward() => thrustManager.StartThrustingForward();
	public void StartThrustingBackward() => thrustManager.StartThrustingBackward();
	public void StartThrustingRight() => thrustManager.StartThrustingRight();
	public void StartThrustingLeft() => thrustManager.StartThrustingLeft();
	public void StopThrustingForward() => thrustManager.StopThrustingForward();
	public void StopThrustingBackward() => thrustManager.StopThrustingBackward();
	public void StopThrustingRight() => thrustManager.StopThrustingRight();
	public void StopThrustingLeft() => thrustManager.StopThrustingLeft();

	// turning
	public void StartTurningClockwise() => rotationManager.StartTurningClockwise();

	public void StartTurningCounterClockwise() => rotationManager.StartTurningCounterClockwise();

	public void StopTurning() => rotationManager.StopTurning();

	public virtual bool TryBuildShip()
	{
		bool hasThruster = false;

		List<Vector2> unpackedVectors = new();

		foreach (Node node in GetChildren())
		{
			if (!(node is ShipComponent shipComponent))
			{ continue; }

			switch (shipComponent)
			{
				case Gun gun:
					gunManager.AddGun(gun);
					break;
				case Thruster thruster:
					thrustManager.AddThruster(thruster);
					hasThruster = true;
					break;
				default: break;
			}

			shipComponents.Add(shipComponent);
			shipComponent.OnDestroyed += ComponentDestroyed;

			foreach (Vector2 v2 in shipComponent.GetVertices())
			{
				Vector2 localPositon = ToLocal(v2);
				unpackedVectors.Add(localPositon);
			}
		}

		Vector2 newGlobalPos = ToGlobal(rotationManager.CalculateCentreOfMass(unpackedVectors));
		Vector2 positionCorrection = newGlobalPos - GlobalPosition;
		foreach (Node n in GetChildren())
		{
			if (n is Camera2D) { continue; }
			if (n is Node2D n2) { n2.GlobalPosition -= positionCorrection; }
		}
		GlobalPosition = newGlobalPos;

		thrustManager.SetWeight(shipComponents.Count);

		return hasThruster;
	}
}
