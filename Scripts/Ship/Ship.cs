using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Ship : CharacterBody2D, IShip
{
	[Export]
	protected CollisionPolygon2D collider;
	protected ThrustManager thrustManager = new();
	protected RotationManager rotationManager= new();
	protected GunManager gunManager= new();
	protected List<ShipComponent> shipComponents = new();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 rotationVect = rotationManager.GetRotation(Rotation, 2, delta, out float newRot);
		GD.PrintS(ToGlobal(rotationManager.LeftRotationPoint).ToString(), ToGlobal(rotationManager.RightRotationPoint).ToString(), GlobalPosition.ToString());
		Rotation = newRot;
		Vector2 force = thrustManager.GetForce(delta);

		float forceMagnitude = force.Length();
		if(forceMagnitude <= 0) { MoveAndCollide(rotationVect); return; }
		force = rotationVect.Normalized() + force.Normalized();
		// force = (rotationVect + force).Normalized();
		force *= forceMagnitude;

		MoveAndCollide(force);
	}

    public void ShipDestroyed()
	{
		GD.Print("ship destroyed");
	}

	public void ComponentDestroyed(ShipComponent shipComponent)
	{
		thrustManager.SetWeight(thrustManager.weight-1);
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
		bool hasFuelTank = false;
		bool hasGenerator = false;
		bool hasThruster = false;
		Generator fuckYou;
		
		List<Vector2> unpackedVectors = new();

		foreach(Node node in GetChildren())
		{
			switch(node)
			{
				case Gun gun:
					gunManager.AddGun(gun);
					break;
				case Thruster thruster:
					thrustManager.AddThruster(thruster);
					hasThruster = true;
					break;
				case Generator generator:
					fuckYou = generator;
					break;
				default: break;
			}
			if(node is ShipComponent) 
			{ 
				ShipComponent shipComponent = node as ShipComponent;
				shipComponents.Add(shipComponent); 
				shipComponent.OnDestroyed += ComponentDestroyed;
				
				foreach (Vector2 v2 in shipComponent.GetVertices())
				{ 
					Vector2 localPositon = ToLocal(v2);
					unpackedVectors.Add(localPositon);
				}
			}
			
		}
		Vector2 newGlobalPos = ToGlobal(rotationManager.CalculateCentreOfMass(unpackedVectors));
		Vector2 positionCorrection = newGlobalPos - GlobalPosition;
		foreach(Node n in GetChildren()) 
		{ 
			if(n is Camera2D) {continue;}
			if(n is Node2D n2) {n2.GlobalPosition -= positionCorrection;}
		}
		GlobalPosition = newGlobalPos;
		ConvexPolygonShape2D convexPolygon = new();
		convexPolygon.SetPointCloud(unpackedVectors.ToArray());
		collider.Polygon = convexPolygon.Points;
		thrustManager.SetWeight(shipComponents.Count);

		return hasFuelTank && hasGenerator && hasThruster;
	}
}
