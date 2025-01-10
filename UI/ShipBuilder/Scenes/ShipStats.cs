using Godot;
using System;
using System.Collections.Generic;

public partial class ShipStats : Panel
{
    [Export]
    private Label healthShip, powerShip, fuelShip, partsShip, weightShip, warningShip;

    protected int health;
    protected int weight;
    protected int power;
    protected int fuel;
    protected List<Piece> parts = new();
    protected string warning;


    public override void _Ready()
    {
        ResetValues();


    }

    public void UpdateStats(List<Piece> pieces)
    {
        weight = pieces.Count;

        foreach (Piece p in pieces)
        {
            health += p.ComponentData.MaxHealth;
            parts.Add(p);
            if (p.ComponentData is GeneratorData generator) power += generator.maxPowerGenerated;
            else if (p.ComponentData is FuelTankData fuelTank) fuel += fuelTank.fuelCapacity;
        }

    }

    public void ResetValues()
    {
        health = 0;
        weight = 0;
        fuel = 0;
        power = 0;
        warning = "";
        parts.Clear();
    }
}
