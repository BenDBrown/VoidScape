using Godot;

public partial class DefenseInfo : Node
{
    public override void _Ready()
    {
        currentHealth = maxHealth;
    }

    [Export]
    public int maxHealth {get; set;}

    [Export]
    public int currentHealth {get; set;}

    [Export]
    public int defense {get; set;}

    public override string ToString()
    {
        return "max health: " + maxHealth + ", current health: " + currentHealth + ", defense: " + defense;
    }

}