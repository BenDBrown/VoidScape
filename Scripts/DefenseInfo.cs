using System;
using Godot.Collections;
using System.Linq;
using Godot;

[GlobalClass]
public partial class DefenseInfo : Node
{
	[Export]
	public int maxHealth {get; set;}

	public int currentHealth {get; set;}

	[Export]
	public int defense {get; set;}
	
	public override void _Ready()
	{
		currentHealth = maxHealth;
	}

	public override string ToString()
	{
		return "max health: " + maxHealth + ", current health: " + currentHealth + ", defense: " + defense;
	}

}
