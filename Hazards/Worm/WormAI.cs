using Godot;
using System;

public partial class WormAI : Node
{
    [Export]
    private Worm worm;

    public override void _Ready()
    {
        worm.MoveTo(worm.GlobalPosition + new Vector2(1000, 0));
    }

}
