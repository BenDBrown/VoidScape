using Godot;
using System;

public partial class GeneratorManager : Node
{
    [Export]
    private TileMapLayer ActivePipes;

    [Export]
    private TileMapLayer UnactivePipes;
    
    [Export]
    private ConditionObject generator;

    public override void _Ready()
	{
        generator.OnConditionMet += SwitchPipes;
	}

    public void SwitchPipes(){

        ActivePipes.Visible = !ActivePipes.Visible;
        UnactivePipes.Visible = !UnactivePipes.Visible;
    }

}
