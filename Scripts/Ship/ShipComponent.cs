using Godot;
using System;

public partial class ShipComponent: RigidBody2D, IHittable
{
	[Signal]
    public delegate void OnDestroyedEventHandler(ShipComponent shipComponent);

	[Export]
    private DefenseInfo defenseInfo;

	DefenseInfo GetDefenseInfo() => defenseInfo;

    public void TakeDamage(DamageInfo damageInfo)
	{
		GD.Print("hit by bullet");
		int newHealth = defenseInfo.currentHealth + defenseInfo.defense;
		newHealth -= damageInfo.damage;
		if(newHealth < defenseInfo.currentHealth)
		{ 
			defenseInfo.currentHealth = newHealth;
			if(defenseInfo.currentHealth <= 0) { EmitSignal(SignalName.OnDestroyed, this);}
		}
	}

}
