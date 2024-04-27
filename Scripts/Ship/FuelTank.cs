using Godot;
using System;

public partial class FuelTank : Node, IShipComponent
{
    private int maxHealth;
    private int currentHealth;
    private long fuel;


    [Signal]
    public delegate void DestroyedEventHandler(FuelTank fuelTank);

    public override void _Ready()
    {
        currentHealth = maxHealth;
    }

    public override void TakeDamage(DamageInfo damageInfo)
    {
        currentHealth -= damageInfo.damage;
        if (currentHealth <= 0) 
        {
            currentHealth = 0;
            EmitSignal(SignalName.Destroyed, this);
        }
    }

    public override void Heal(int healing)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth) { currentHealth = maxHealth; }
    }

}
