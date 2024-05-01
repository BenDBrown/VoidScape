using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ShipComponent: Area2D
{
	[Signal]
    public delegate void OnDestroyedEventHandler(ShipComponent shipComponent);

	[Export]
    private DefenseInfo defenseInfo;

	[Export]
	private Node2D[] vertices;

	private bool destroyed = false;

	DefenseInfo GetDefenseInfo() => defenseInfo;
	public bool IsDestroyed() => destroyed;

    public void CollideWithBody(Node2D node2D)
	{
		if(node2D is IDamager)
		{
			IDamager damager = node2D as IDamager;
			TakeDamage(damager.GetDamageInfo());
		}
	}

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

	public Vector2[] GetVertices()
	{
		Vector2[] v2Vertices = new Vector2[vertices.Length];
		for(int i = 0; i < vertices.Length; i++)
		{
			Vector2 v2 = vertices[i].GlobalPosition;
			v2Vertices[i] = v2;
		}
		return v2Vertices;
	}

	private void Destroyed()
	{
		destroyed = true;
		CollisionLayer = 0;
		CollisionMask = 0;
		Hide();
		EmitSignal(SignalName.OnDestroyed, this);
	}

	private void Revived()
	{
		CollisionLayer = 0b00000000_00000000_00000000_00000001;
		CollisionMask = 0b00000000_00000000_00000000_00000001;
		destroyed = false;
		Visible = true;	
	}
}
