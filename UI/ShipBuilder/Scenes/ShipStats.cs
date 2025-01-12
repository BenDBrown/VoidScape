using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public partial class ShipStats : Panel
{
    [Export]
    private Label healthShip, powerShip, fuelShip, partsShip, weightShip, warningShip;

    protected int health;
    protected int weight;
    protected int power;
    protected int fuel;
    protected List<Piece> parts = new();
    protected List<string> missingParts = new();
    protected string warning;

    public override void _Ready()
    {
        CheckWarnings(parts);
    }

    public void UpdateStats(List<Piece> pieces)
    {
        ResetValues();
        weight = pieces.Count;
        foreach (Piece p in pieces)
        {
            health += p.ComponentData.MaxHealth;
            parts.Add(p);
            if (p.ComponentData is GeneratorData generator) power += generator.maxPowerGenerated;
            else if (p.ComponentData is FuelTankData fuelTank) fuel += fuelTank.fuelCapacity;
        }
        CheckWarnings(parts);
        PopulateList();

    }

    public void PopulateList()
    {
        healthShip.Text = "Health: " + health.ToString();
        weightShip.Text = "Weight: " + weight.ToString();
        powerShip.Text = "Power: " + power.ToString();
        fuelShip.Text = "Fuel: " + fuel.ToString();
        partsShip.Text = "Parts: " + parts.Count.ToString();

        if (missingParts.Count > 0)
        {
            string warningmsg = "These parts are missing: ";
            foreach (var part in missingParts)
            {
                warningmsg += $"{part}" + " \n";
            }
            warningShip.Text = warningmsg;
        }
        else warningShip.Text = "No Warnings";

    }

    public void CheckWarnings(List<Piece> pieces)
    {
        bool hasFuelTank = false;
        bool hasGenerator = false;
        bool hasThruster = false;
        bool hasGun = false;

        foreach (Piece p in pieces)
        {
            if (p.ComponentData is FuelTankData) hasFuelTank = true;
            if (p.ComponentData is GeneratorData) hasGenerator = true;
            if (p.ComponentData is ThrusterData) hasThruster = true;
            if (p.ComponentData is GunData) hasGun = true;
        }

        if (!hasFuelTank) missingParts.Add(nameof(FuelTank).ToString());
        if (!hasGenerator) missingParts.Add(nameof(Generator).ToString());
        if (!hasThruster) missingParts.Add(nameof(Thruster).ToString());
        if (!hasGun) missingParts.Add(nameof(Gun).ToString());
    }

    public void ResetValues()
    {
        health = 0;
        weight = 0;
        fuel = 0;
        power = 0;
        warning = "";
        parts.Clear();
        missingParts.Clear();
    }
}
