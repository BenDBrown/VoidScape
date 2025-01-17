using Godot;
using System;

public partial class CaveCollapse : Node
{
    [Export]
    private Gate[] gates;

    public void CheckForPlayerEnteredTrigger(Node2D body)
    {
        if(body is PlayerShip) Collapse();
    }

    public void Collapse()
    {
        Tween tween = GetTree().CreateTween();
        tween.SetParallel(false);
        foreach(Gate gate in gates)
        {
            gate.Close(tween);
        }
    }
}
