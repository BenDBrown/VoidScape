using Godot;
using System;

public partial class Pipes : Node
{

    [Export]
    private TileMapLayer Active;

    [Export]
    private TileMapLayer Unactive;
    
    [Export]
    private ConditionObject generator;

    public override void _Ready()
	{
        generator.OnConditionMet += Switch;
	}

    public void Switch(){

        Active.Visible = !Active.Visible;
        Unactive.Visible = !Unactive.Visible;
    }

}
