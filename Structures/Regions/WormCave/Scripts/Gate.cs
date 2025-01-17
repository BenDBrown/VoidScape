using Godot;
using System;

public partial class Gate : Node
{
    private const string PROGRESS_RATIO_PROP_NAME = "progress_ratio";

    [Export]
    private PathFollow2D GateDoor1;

    [Export]
    private PathFollow2D GateDoor2;

    [Export]
    float openTime1 = 1;

    [Export]
    float openTime2 = 1;

    public override void _Ready()
    {
        base._Ready();
        if(GateDoor1 == null && GateDoor2 == null) GD.PrintErr("one of two gate sides must be set");
    }

    public void Open()
    {
        Tween tween = GetTree().CreateTween();
        tween.SetParallel(true);
        Open(tween);
    }

    public void Open(Tween tween)
    {
        if(GateDoor1 != null)
        {
            tween.TweenProperty(GateDoor1, PROGRESS_RATIO_PROP_NAME, 1, openTime1);
        }
        if(GateDoor2 != null)
        {
            tween.TweenProperty(GateDoor2, PROGRESS_RATIO_PROP_NAME, 1, openTime2);
        }
    }

    public void Close()
    {
        Tween tween = GetTree().CreateTween();
        tween.SetParallel(true);
        Close(tween);
    }

    public void Close(Tween tween)
    {
        if(GateDoor1 != null)
        {
            tween.TweenProperty(GateDoor1, PROGRESS_RATIO_PROP_NAME, 0, openTime1);
        }
        if(GateDoor2 != null)
        {
            tween.TweenProperty(GateDoor2, PROGRESS_RATIO_PROP_NAME, 0, openTime2);
        }
    }

}
