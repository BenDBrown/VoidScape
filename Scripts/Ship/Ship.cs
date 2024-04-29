using Godot;
using System;
using System.Collections.Generic;

public partial class Ship : Node2D
{
	private List<ShipComponent> shipComponents = new();

	private List<Gun> guns = new();

    public override void _Ready()
    {
        foreach(Node node in this.GetChildren())
		{
			if(node is Gun){guns.Add(node as Gun); }
			if(node is ShipComponent) 
			{ 
				ShipComponent shipComponent = node as ShipComponent;
				shipComponents.Add(shipComponent); 
				shipComponent.OnDestroyed += ComponentDestroyed;
			}
			
		}
    }

	public void ComponentDestroyed(ShipComponent shipComponent)
	{
		shipComponent.Hide();
		GD.Print(shipComponent.Name + " destroyed");
	}

	public void Shoot() { foreach(Gun gun in guns) { gun.Shoot(); } }
	
}
