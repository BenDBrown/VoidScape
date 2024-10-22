using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Ship : CharacterBody2D, IShip
{
	[Export]
	private bool buildOnStart = false;
	protected ThrustManager thrustManager = new();
	protected CenterCalculator centerCalculator = new();
	protected GunManager gunManager = new();
	protected List<ShipComponent> shipComponents = new();
	private float rotationSpeed = 3;
	private bool isRotatingClockwise = false;
	private bool isRotating = false;
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
		if (isRotating)
		{
			Rotation += (isRotatingClockwise ? rotationSpeed : -rotationSpeed) * (float)delta;
		}
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
	public void StartTurningClockwise()
	{
		isRotating = true;
		isRotatingClockwise = true;
	}

	public void StartTurningCounterClockwise()
	{
		isRotating = true;
		isRotatingClockwise = false;
	}

	public void StopTurning() => isRotating = false;

	public virtual bool TryBuildShip()
	{
		bool hasThruster = false;

		List<Vector2> globalVertices = new();

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
			globalVertices.AddRange(shipComponent.GetVertices());

			
		}

		Vector2 center = centerCalculator.GetGlobalShipCenter(globalVertices);
		foreach (Node n in GetChildren())
		{
			if (n is Camera2D) { continue; }
			if (n is Node2D n2) { n2.Position -= ToLocal(center); }
		}
		thrustManager.SetWeight(shipComponents.Count);

		return hasThruster;
	}
}
