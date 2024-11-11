using Desktop.Ship.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
		foreach (ShipComponent shipComponent in shipComponents) { shipComponent.Visible = false; }
		ExplosionAnim.Visible = true;
		ExplosionAnim.Play();
	}

	public void ComponentDestroyed(ShipComponent shipComponent)
	{
		thrustManager.SetWeight(thrustManager.weight - 1);
		GD.Print(shipComponent.Name + " destroyed");
		if (shipComponent is FuelTank || shipComponent is Generator || shipComponent is Thruster || shipComponent is Cockpit)
		{
			Type destroyedComponentType = shipComponent.GetType();
			foreach (ShipComponent s in shipComponents)
			{
				if (s.GetType() == destroyedComponentType && (!s.IsDestroyed()) && s != shipComponent)
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
			if (node is not ShipComponent shipComponent) { continue; }

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
			globalVertices.Add(shipComponent.GlobalPosition);
		}

		Vector2 center = centerCalculator.GetGlobalShipCenter(globalVertices);
		foreach (Node n in GetChildren())
		{
			if (n is Camera2D) { continue; }
			if (n is Node2D n2) { n2.Position -= ToLocal(center); }
			if (n is ShipComponent shipComponent)
			{
				shipComponent.collider.Owner = null; //prevents warning.
				shipComponent.collider.Reparent(this);
				shipComponent.collider.Owner = this;
			}
		}
		thrustManager.SetWeight(shipComponents.Count);

		return hasThruster;
	}

	/// <summary>
	/// Removes all ShipComponents from the ship. Used when reusing ship.
	/// </summary>
	public void Reset() //Change it to be better, maybe keep track of old parts before trybuild and replace if it fails?
	{
		thrustManager = new(); //preferably a reset method that removes all existing thrusters
		gunManager = new();
		Node[] children = GetChildren().ToArray();
		for (int i = 0; i < children.Length; i++)
		{
			if (children[i] is ShipComponent component)
			{
				component.OnDestroyed -= ComponentDestroyed;
				component.collider.Free();
				component.Free();
			}
		}
	}

	/// <summary>
	/// Add Ship components based on a coordinate system. 
	/// This is used primarily when building ship in code to make it easier to connect pieces together
	/// </summary>
	/// <param name="component"></param>
	/// <param name="coordinate"></param>
	public void AddComponent(ShipComponent component, Vector2 coordinate)
	{
		AddChild(component);
		component.Position = coordinate * 32;
	}
}
