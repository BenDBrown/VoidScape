using Godot;
using System;

public partial class ShipComponent: RigidBody2D, IHittable
{
	[Signal]
    public delegate void OnDestroyedEventHandler(ShipComponent shipComponent);

	[Export]
    private DefenseInfo defenseInfo;

	private bool destroyed = false;

	DefenseInfo GetDefenseInfo() => defenseInfo;
	public bool IsDestroyed() => destroyed;


    public void TakeDamage(DamageInfo damageInfo)
	{
		GD.Print("hit by bullet");
		int newHealth = defenseInfo.currentHealth + defenseInfo.defense;
		newHealth -= damageInfo.damage;
		if(newHealth < defenseInfo.currentHealth)
		{ 
			defenseInfo.currentHealth = newHealth;
			if(defenseInfo.currentHealth <= 0) { Destroyed(); }
		}
	}

	private void Destroyed()
	{
		destroyed = true;
		CollisionLayer = 0;
		CollisionMask = 0;
		Hide();
		EmitSignal(SignalName.OnDestroyed, this);
	}
}
