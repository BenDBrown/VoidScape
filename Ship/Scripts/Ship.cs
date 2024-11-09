using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Ship : CharacterBody2D, IShip
{
	[Signal]
	public delegate void OnDestroyedEventHandler(Ship ship);
	
	[Export]
	private SingleRunAnimation ExplosionAnim;

	[Export]
	private bool buildOnStart = false;
	protected ThrustManager thrustManager = new();
	protected CenterCalculator centerCalculator = new();
	protected RotationManager rotationManager = new();
	protected GunManager gunManager = new();
	protected List<ShipComponent> shipComponents = new();
	private float rotationSpeed = 3;
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
		Rotation = rotationManager.GetRotation(Rotation, rotationSpeed, delta, out Vector2 rotVector);
		Vector2 force = thrustManager.GetForce(delta, Rotation);
		Velocity = force;
		MoveAndSlide();
	}

	protected void ShipDestroyed()
	{
		EmitSignal(SignalName.OnDestroyed, this);
		foreach(ShipComponent shipComponent in shipComponents) { shipComponent.Visible = false; }
		ExplosionAnim.Visible = true;
		ExplosionAnim.Play();
	}

	public void ComponentDestroyed(ShipComponent shipComponent)
	{
		thrustManager.SetWeight(thrustManager.weight - 1);
		GD.Print(shipComponent.Name + " destroyed");
		if(shipComponent is FuelTank || shipComponent is Generator || shipComponent is Thruster || shipComponent is Cockpit)
		{
			Type destroyedComponentType = shipComponent.GetType();
			foreach(ShipComponent s in shipComponents)
			{
				if(s.GetType() == destroyedComponentType && (!s.IsDestroyed()) && s != shipComponent)
				{
					GD.Print("compnent was not last");
					return;
				}
			}
			ShipDestroyed();
		}
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
			GD.Print(shipComponent.Name + " added");
			globalVertices.AddRange(shipComponent.GetVertices());
		}

		Vector2 center = centerCalculator.GetGlobalShipCenter(globalVertices);
		foreach (Node n in GetChildren())
		{
			if (n is Camera2D) { continue; }
			if (n is Node2D n2) { n2.Position -= ToLocal(center); }
			if (n is ShipComponent shipComponent) shipComponent.collider.Reparent(this);
		}
		thrustManager.SetWeight(shipComponents.Count);

		return hasThruster;
	}
}
