using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class WormAI : Node2D
{
    [Export]
    private bool wormFollowsMouse = false; // for testing only 

    [Export]
    private Worm worm;

    [Export]
    private WormRock[] rocks;

    private List<List<Action>> patterns;
    
    private List<Action> currentPattern = null;

    private List<Action> idlePattern;

    private WormRock currentRock = null;

    private Random rng = new();

    private int patternIndex = 0;

    private bool aggro = false;

    public override void _Ready()
    {
        if(wormFollowsMouse) return;
        worm.OnWormDestroyed += QueueFree;
        patterns = new()
        {
            new()
            {
                MoveToNearestRock,
                FoldIntoRock,
                FoldOutOfRock,
                ChargeAtPlayer
            },
            new()
            {
                MoveToRandomRock,
                FoldIntoRock,
                FoldOutOfRock,
                ChargeAtPlayer,
                ChargeAtPlayer,
                ChargeAtPlayer
            }
        };
        idlePattern = new()
        {
            MoveToRandomRock,
            FoldIntoRock,
            FoldOutOfRock
        };
        currentPattern = idlePattern;
        worm.OnWormReachedDestination += NextAction;
        NextAction();
    }

    public void OnBodyEntered(Node2D node) {if(node == Game.Instance.PlayerShip) Aggro();}

    public void OnBodyExited(Node2D node){if(node == Game.Instance.PlayerShip) EndAggro();}

    public override void _Process(double delta)
    {
        if(wormFollowsMouse) worm.MoveTo(GetViewport().GetMousePosition());
    }

    public void Aggro() => aggro = true;
    
    public void EndAggro() => aggro = false;


    private void MoveToNearestRock() => MoveToRock(GetNearestWormRock());

    private void MoveToRandomRock() => MoveToRock(GetRandomWormRock()); 

    private void ChargeAtPlayer() => worm.ChargeAtPlayer();


    private WormRock GetNearestWormRock()
    {
        WormRock rock = null;
        float distance = 0;
        foreach(WormRock newRock in rocks)
        {
            float newDistance = worm.GlobalHeadPos.DistanceTo(newRock.GlobalPosition);
            if(newRock == currentRock) continue;
            if(rock == null || newDistance < distance) 
            {
                rock = newRock;
                distance = newDistance;
            }
        }

        return rock;
    }

    public void NextAction()
    {
        if(currentPattern == null || patternIndex >= currentPattern.Count)
        {
            SelectPattern();
            return;
        }
        patternIndex++;
        currentPattern[patternIndex-1].Invoke();
    }

    private void SelectPattern()
    {
        if(!aggro)
        {
            currentPattern = idlePattern;
            patternIndex = 0;
            NextAction();
            return;
        }
        if(patterns.Count <= 0)
        {
            GD.PrintErr("no patterns defined");
            return;
        }
        currentPattern = patterns[rng.Next(0, patterns.Count)];
        if(currentPattern.Count <= 0) 
        {
            GD.PrintErr("pattern was null");
            return;
        }

        patternIndex = 0;
        NextAction();
    }

    private void MoveToRock(WormRock rock)
    {
        worm.FoldIntoLocation = false;
        currentRock = rock;
        worm.MoveTo(rock.GetNearestHole(worm.GlobalHeadPos));
    }

    private void FoldIntoRock()
    {
        if(currentRock == null) 
        {
            GD.PrintErr("tried folding out of rock that was not set");
            return;
        }
        worm.MoveTo(currentRock.GlobalPosition);
        worm.FoldIntoLocation = true;
    }

    private void FoldOutOfRock()
    {
        if(currentRock == null) 
        {
            GD.PrintErr("tried folding out of rock that was not set");
            return;
        }
        worm.ResetPath();
        worm.FoldIntoLocation = false;
        Vector2 exit = currentRock.GetHoleNearestToPlayer();
        worm.MoveToDirect(exit);
    }

    private WormRock GetRandomWormRock()
    {
        WormRock rock = null;
        int tryCounter = 0;
        int maxTries = 10000;
        while (rock == null)
        {
            WormRock newRock = rocks[rng.Next(0, rocks.Length)];
            if(currentRock == newRock) continue;
            rock = newRock;
            tryCounter++;
            if(tryCounter >= maxTries) break;
        }
        return rock;
    }
}
