using Godot;
using System;

public partial class WormHealthBar : ProgressBar
{
    public void OnHealthChanged(int newHealth) => Value = newHealth;
}
