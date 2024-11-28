using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerShip : Ship, IShip
{
	[Signal]
	public delegate void PowerChangedEventHandler(float powerToMaxPowerPercentage);

	[Signal]
	public delegate void GunChangedEventHandler();

	[Export]
	private PowerManager powerManager;
	private CargoManager cargoManager = new();
	private FuelManager fuelManager = new();

    public override void _Ready()
    {
        base._Ready();
		// setting up wrapper signals and stall signals
		powerManager.StallStarted += InitiateStall;
		powerManager.StallStarted += (stallTime)  =>EmitSignal(SignalName.StallStarted, stallTime);
		powerManager.StallEnded += EndStall;
		powerManager.StallEnded += () => EmitSignal(SignalName.StallEnded);
		powerManager.PowerChanged += (powerToMaxPowerPercentage) => EmitSignal(SignalName.PowerChanged, powerToMaxPowerPercentage);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta); // done in physics process after base so that power draw values on ThrustManager are updated first in the same thread
		powerManager.TryUsePower(GetPowerDraw((float)delta), out float fuelUsed);
		// ToDo add fuel manager logic with fuel used
    }

    public override bool TryBuildShip()
	{
		bool hasFuelTank = false;
		bool hasGenerator = false;
		bool hasThruster = false;

		List<Vector2> globalVertices = new();

		foreach(Node node in GetChildren())
		{
			if (node is not ShipComponent shipComponent) { continue; }
			switch(node)
			{
				case Gun gun:
					gunManager.AddGun(gun);
					break;
				case Hull hull:
					cargoManager.AddHull(hull);
					break;
				case FuelTank fuelTank:
					fuelManager.AddFuelTank(fuelTank);
					hasFuelTank = true;
					break;
				case Generator generator:
					powerManager.AddGenerator(generator);
					hasGenerator = true;
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

		return hasFuelTank && hasGenerator && hasThruster;
	}

	private float GetPowerDraw(float delta) // add per frame power draw here
	{
		return (thrustManager.PowerDraw + gunManager.PowerDraw) * delta;
	}

		/// <summary>
	/// Get a list of the types of guns that are available on this ship.
	/// This is used to create a correct overview for the weapon menu UI.
	/// </summary>
	/// <returns>A list of the gun types</returns>
	public String[] GetAvailableGunTypes(){
		return gunManager.GetGunGroupTypes().ToArray();
	}

	/// <summary>
	/// This is used by the weapon menu ui to know which weapon is selected in the list of available weapons
	/// </summary>
	/// <returns> the index of the gungroup that is selected</returns>
	public int GetActiveWeaponIndex(){
		return gunManager.GetSelectedWeaponIndex();
	}

}