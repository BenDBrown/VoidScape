using Godot;
using System;

public partial class Cockpit : RigidBody2D
{
	[Export]
    private DefenseInfo defenseInfo;

    public DefenseInfo GetDefenseInfo() => defenseInfo;
}
