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

	[Signal]
	public delegate void StallStartedEventHandler(double stallTime);

	[Signal]
	public delegate void StallEndedEventHandler();

	[Export]
	private SingleRunAnimation ExplosionAnim;

	[Export]
	private bool buildOnStart = false;

	

	protected ThrustManager thrustManager = new();
	protected CenterCalculator centerCalculator = new();
	protected RotationManager rotationManager = new();
	protected GunManager gunManager = new();
	protected List<ShipComponent> shipComponents = new();
	public ShipComponent[] shipParts => shipComponents.ToArray();
	protected float rotationSpeed = 3;
	protected bool stalling = false;

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

	protected virtual void ShipDestroyed()
	{
		foreach (ShipComponent shipComponent in shipComponents) { shipComponent.Visible = false; }
		ExplosionAnim.Visible = true;
		ExplosionAnim.Play();
		ExplosionAnim.Reparent(GetTree().CurrentScene);
		ExplosionAnim.AnimationFinished += () => ExplosionAnim.QueueFree();
		EmitSignal(SignalName.OnDestroyed, this);
	}

	public void ComponentDestroyed(ShipComponent shipComponent)
	{
		thrustManager.SetWeight(thrustManager.weight - 1);
		GD.Print(shipComponent.Name + " destroyed");
		if (IsVitalComponent(shipComponent))
		{
			Type destroyedComponentType = shipComponent.GetType();
			foreach (ShipComponent s in shipComponents)
			{
				if (s.GetType() == destroyedComponentType && (!s.IsDestroyed()) && s != shipComponent)	return;
			}
			ShipDestroyed();
		}
	}

	// shooting
	public void StartShooting() {if(!stalling)gunManager.StartShooting();}
	public void StopShooting() => gunManager.StopShooting();

	// movement
	public void StartThrustingForward() {if(!stalling)thrustManager.StartThrustingForward();}
	public void StartThrustingBackward() {if(!stalling)thrustManager.StartThrustingBackward();}
	public void StartThrustingRight() {if(!stalling)thrustManager.StartThrustingRight();}
	public void StartThrustingLeft() {if(!stalling)thrustManager.StartThrustingLeft();}
	public void StopThrustingForward() => thrustManager.StopThrustingForward();
	public void StopThrustingBackward() => thrustManager.StopThrustingBackward();
	public void StopThrustingRight() => thrustManager.StopThrustingRight();
	public void StopThrustingLeft() => thrustManager.StopThrustingLeft();

	// turning
	public void StartTurningClockwise() {if(!stalling)rotationManager.StartTurningClockwise();}
	public void StartTurningCounterClockwise() {if(!stalling)rotationManager.StartTurningCounterClockwise();}
	public void StopTurning() => rotationManager.StopTurning();
	public void KillMomentum() => thrustManager.KillMomentum();

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
			if (n is Node2D n2 && n is not SingleRunAnimation) { n2.Position -= ToLocal(center); } // is not, for bandaid solution to prevent ship destruction anim from being off centre
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

	protected virtual bool IsVitalComponent(ShipComponent shipComponent) => shipComponent is Cockpit;

	protected virtual void InitiateStall(double stallTime)
	{
		stalling = true;
		thrustManager.StopThrusting();
		gunManager.StopShooting();
		rotationManager.StopTurning();
	}

	protected virtual void EndStall() => stalling = false;
	
}
