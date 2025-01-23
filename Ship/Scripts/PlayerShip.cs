using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerShip : Ship
{
	[Signal]
	public delegate void PowerChangedEventHandler(float powerToMaxPowerPercentage);

	[Signal]
	public delegate void FuelChangedEventHandler(float fuelToMaxFuelPercentage);

	[Signal]
	public delegate void GunCycleChangedEventHandler(int cycleNum);

	[Signal]
	public delegate void WeaponMenuToggledEventHandler(bool isOpen);
	[Signal]
	public delegate void InteractableInteractedEventHandler();

	[Signal]
	public delegate void CreditsChangedEventHandler(float totalCredits);

	[Export]
	private Shield shield;

	[Export]
	private Blinking blinking;
	[Export]
	private PowerManager powerManager;
	private CargoManager cargoManager = new();
	private FuelManager fuelManager = new();

	private CreditsManager creditsManager = new();

	private bool weaponMenuIsOpen = false;

	public void StartShielding()
	{
		if (stalling) return;
		shield.StartShielding();
	}
	public void StopShielding() => shield.StopShielding();

	public override void _Ready()
	{
		base._Ready();
		// setting up wrapper signals and stall signals
		powerManager.StallStarted += InitiateStall;
		powerManager.StallStarted += (stallTime) => EmitSignal(SignalName.StallStarted, stallTime);
		powerManager.StallEnded += EndStall;
		powerManager.StallEnded += () => EmitSignal(SignalName.StallEnded);
		powerManager.PowerChanged += (powerToMaxPowerPercentage) => EmitSignal(SignalName.PowerChanged, powerToMaxPowerPercentage);

		fuelManager.FuelChanged += (fuelToMaxFuelPercentage) => EmitSignal(SignalName.FuelChanged, fuelToMaxFuelPercentage);
		fuelManager.NoFuel += ShipDestroyed;

		shield.ShieldHit += UsePowerChunk;
		blinking.Blink += UsePowerChunk;

		creditsManager.CreditsChanged += (totalCredits) => EmitSignal(SignalName.CreditsChanged, totalCredits);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta); // done in physics process after base so that power draw values on ThrustManager are updated first in the same thread
		powerManager.TryUsePower(GetPowerDraw((float)delta));
		// ToDo add fuel manager logic with fuel used
		fuelManager.UseFuel(thrustManager.FuelUsage);
	}

	public override bool TryBuildShip()
	{
		bool hasFuelTank = false;
		bool hasGenerator = false;
		bool hasThruster = false;

		List<Vector2> globalVertices = new();

		foreach (Node node in GetChildren())
		{
			if (node is not ShipComponent shipComponent) { continue; }
			switch (node)
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

			// TEMPORARY TO ALLOW SHIP TO START WITH FULL FUEL
			fuelManager.AddFuel(fuelManager.FuelCapacity);
		}

		Vector2 center = centerCalculator.GetGlobalShipCenter(globalVertices);
		shield.Scale = centerCalculator.GetNrOfComponentsScale(globalVertices);
		foreach (Node n in GetChildren())
		{
			if (n is Camera2D) { continue; }
			if (n is ShipComponent shipComponent)
			{
				shipComponent.Position -= ToLocal(center);
				shipComponent.collider.Owner = null; //prevents warning.
				shipComponent.collider.Reparent(this);
				shipComponent.collider.Owner = this;
			}
		}
		Weight = shipComponents.Count;
		return hasFuelTank && hasGenerator && hasThruster;
	}

	public override void ShipDestroyed()
	{
		base.ShipDestroyed();

		GC.Collect();
	}

	public override void Reset()
	{
		powerManager.Reset();
		fuelManager.Reset();
		base.Reset();
	}

	protected override bool IsVitalComponent(ShipComponent shipComponent)
	{
		// gun intentionally not included as vital atm
		return shipComponent is Cockpit || shipComponent is Thruster || shipComponent is Generator || shipComponent is FuelTank;
	}

	protected override void InitiateStall(double stallTime)
	{
		base.InitiateStall(stallTime);
		shield.StopShielding();
		AnimationPlayer animPLayer = Game.Instance.Hud.StallAnimPlayer;
		animPLayer.Play("stalling_warning");
	}

	protected override void EndStall()
	{
		base.EndStall();
		AnimationPlayer animPLayer = Game.Instance.Hud.StallAnimPlayer;
		animPLayer.Stop();
	}

	/// <summary>
	/// Intended for power usage which does not occur as part of process
	/// </summary>
	private void UsePowerChunk(int powerUsed) => powerManager.TryUsePower(powerUsed);

	private float GetPowerDraw(float delta) // add per frame power draw here
	{
		return (gunManager.PowerDraw + shield.PowerDraw + blinking.PowerDraw) * delta;
	}

	public void Interact() => EmitSignal(SignalName.InteractableInteracted);

	public void PerformBlink(Vector2 vector2)
	{
		if (stalling) return;
		blinking.Async_PerformBlink(vector2);
	}

	#region GUN
	public void ToggleWeaponMenu(bool isOpen)
	{
		weaponMenuIsOpen = isOpen;
		EmitSignal(SignalName.WeaponMenuToggled, isOpen);
	}

	public void CycleGunGroup(int cycleNum)
	{
		if (!weaponMenuIsOpen) { return; }

		gunManager.CycleGunGroup(cycleNum);
		EmitSignal(SignalName.GunCycleChanged, cycleNum);
	}

	/// <summary>
	/// Get a list of the types of guns that are available on this ship.
	/// This is used to create a correct overview for the weapon menu UI.
	/// </summary>
	/// <returns>A list of the gun types</returns>
	public List<GunType> GetAvailableGunTypes() => gunManager.GetGunGroupTypes();

	/// <summary>
	/// Return the corresponding icon for the gun type. this is used in the weapon menu
	/// </summary>
	/// <param name="type">The gun type where you want the icon for.</param>
	/// <returns>Texture of the gun type icon</returns>
	public Texture2D GetGunTypeIcon(GunType type) => gunManager.GetGunTypeIcon(type);

	/// <summary>
	/// This is used by the weapon menu ui to know which weapon is selected in the list of available weapons
	/// </summary>
	/// <returns> the index of the gungroup that is selected</returns>
	public int GetActiveWeaponIndex() => gunManager.GetSelectedWeaponIndex();
	#endregion GUN

	#region CARGO
	public void CollectCargo(Cargo cargo) => cargoManager.AddCargo(cargo);
	public void CollectComponent(ShipComponentData scd) => cargoManager.AddShipComponent(scd);
	public Dictionary GetCargos() => cargoManager.GetCargos();
	#endregion CARGO

	#region CREDITS

	/// <summary>
	/// Add the credits currency to the player.
	/// </summary>
	/// <param name="amountToAdd">Amount of credits to add</param>
	public void AddCredits(float amountToAdd) => creditsManager.AddCredits(amountToAdd);

	/// <summary>
	/// Removing credits from the players available credits.
	/// </summary>
	/// <param name="decreaseAmount">Amount to take away.</param>
	/// <returns>Returns wether this action has succeeded or not. A false means that no money was taking away.</returns>
	public bool TryTakeCredits(float decreaseAmount) => creditsManager.TryDecreaseCredits(decreaseAmount);

	/// <summary>
	/// A check to see wether the player has enough credits to purchase something with the given price.
	/// </summary>
	/// <param name="priceToCheck"></param>
	/// <returns></returns>
	public bool HasEnoughCredits(float priceToCheck) => creditsManager.HasEnoughMoney(priceToCheck);

	#endregion CREDITS

	public void Refuel(int fuelAmount = -1)
	{
		if (fuelAmount == -1)
		{
			fuelManager.AddFuel(fuelManager.FuelCapacity);
		}
		else
		{
			fuelManager.AddFuel(fuelAmount);
		}
	}
}
