using Godot;
using System;

public partial class WormAI : Node
{
    [Export]
    private Worm worm;

    public override void _Process(double delta)
    {
        worm.MoveTo(GetViewport().GetMousePosition());
    }

}
