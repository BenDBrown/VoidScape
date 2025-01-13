using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class WormAI : Node
{
    [Export]
    private Worm worm;

    [Export]
    private WormRock rock1;

    [Export]
    private WormRock rock2;

    private List<List<Action>> patterns;
    
    private List<Action> currentPattern = null;

    private int patternIndex = 0;

    public override void _Ready()
    {
        patterns = new()
        {
            new()
            {
                () => MoveToRock(rock1),
                () => FoldIntoRock(rock1),
                ClearWormPath,
                () => FoldOutOfRock(rock1),
                () => MoveToRock(rock2),
                () => FoldIntoRock(rock2),
                ClearWormPath,
                () => FoldOutOfRock(rock2),
            }
        };
        worm.OnWormReachedDestination += NextAction;
        NextAction();
    }


    public override void _Process(double delta)
    {
        // worm.MoveTo(GetViewport().GetMousePosition()); // doesnt work while player is in scene due to viewport getting misaligned and the method expecting global coords
    }


    public void NextAction()
    {
        if(currentPattern == null || patternIndex >= currentPattern.Count) 
        {
            SelectPattern();
            return;
        }
        GD.Print("next action starting");
        patternIndex++;
        currentPattern[patternIndex-1].Invoke();
    }

    private void SelectPattern()
    {
        if(patterns.Count <= 0)
        {
            GD.Print("no patterns defined");
            return;
        }

        Random rng = new();
        currentPattern = patterns[rng.Next(0, patterns.Count)];
        if(currentPattern.Count <= 0) 
        {
            GD.Print("pattern was null");
            return;
        }

        patternIndex = 0;
        NextAction();
    }

    private void MoveToRock(WormRock rock)
    {
        worm.FoldIntoLocation = false;
        worm.MoveTo(rock.GetNearestHole(worm.GlobalHeadPos));
    }

    private void FoldIntoRock(WormRock rock)
    {
        worm.MoveTo(rock.GlobalPosition);
        worm.FoldIntoLocation = true;
    }

    private void FoldOutOfRock(WormRock rock)
    {
        worm.FoldIntoLocation = false;
        worm.MoveTo(rock.GetRandomHole());
    }

    private void ClearWormPath()
    {
        worm.ResetPath();
        NextAction();
    }


}
