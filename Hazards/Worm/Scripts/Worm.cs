using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Worm : Path2D
{
    [Signal]
    public delegate void OnWormReachedDestinationEventHandler();

    [Signal]
    public delegate void OnWormDestroyedEventHandler();

    [Signal]
    public delegate void OnWormHealthChangedEventHandler(int newHealth);

    [Export]
    private PathFollow2D[] Segments;

    [Export]
    private PackedScene deathAnim;

    [Export]
    private float knockback = 50;

    [Export]
    private float curvePeriod = 96; // in pixels

    [Export]
    private float slitherAmplitude = 64; // in pixels

    [Export]
    private double baseSpeed = 100;

    [Export]
    private double boostSpeed = 200;

    public bool FoldIntoLocation = false;

    public Vector2 GlobalHeadPos => Segments[0].GlobalPosition;

    private Vector2 globalTargetLocation = Vector2.Zero;

    private Vector2 targetLocation => globalTargetLocation - GlobalPosition;

    // records the distance between segments with each segment corresponding to the distance to the proceeding segment
    // the array starts with the head so this means that the head should always have a value of 0 while the tail
    // should always have the distance to the second to last piece of the worm
    private Dictionary<PathFollow2D, float> segmentGapDict = new(); 

    private double speed;

    private bool reachedTarget = false;


    public void OnPlayerHit()
    {
        PlayerShip player = Game.Instance.PlayerShip;
        Vector2 perpendicularDirection = Vector2.Right.Rotated(Segments[0].GlobalRotation).Orthogonal();
        Vector2 knockbackA = perpendicularDirection * knockback;
        Vector2 knockbackB = perpendicularDirection * -knockback;
        if((GlobalHeadPos + knockbackA).DistanceTo(player.GlobalPosition) > (GlobalHeadPos + knockbackB).DistanceTo(player.GlobalPosition))
        {
            player.AddExternalImpulse(knockbackB);
        }
        else player.AddExternalImpulse(knockbackA);
    }

    public void OnDeath()
    {
        EmitSignal(SignalName.OnWormDestroyed);
        SingleRunAnimation anim = deathAnim.Instantiate() as SingleRunAnimation;
        anim.AnimationFinished += QueueFree;
        foreach(PathFollow2D segment in Segments)
        {
            segment.Visible = false;
        }
        GetTree().CurrentScene.AddChild(anim);
        anim.GlobalPosition = GlobalHeadPos;
        anim.Play();
    }

    public void OnHealthChanged(int newHealth) => EmitSignal(SignalName.OnWormHealthChanged, newHealth);

    public override void _Ready()
    {
        base._Ready();
        speed = baseSpeed;
        for(int i = 0; i < Segments.Length; i++)
        {
            if(0 == i)
            {
                segmentGapDict.Add(Segments[i], 0);
                continue;
            }
            segmentGapDict.Add(Segments[i], Segments[i - 1].Progress - Segments[i].Progress);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(reachedTarget) return;
        IncrementProgress((float)(delta * speed));
    }


    public void MoveTo(Vector2 globalTargetPos)
    {
        globalTargetLocation = globalTargetPos;
        reachedTarget = false;
    }

    /// <summary>
	/// Takes a global point to move through as a parameter.
	/// </summary>
    public void MoveToDirect(Vector2 globalTargetPos) 
    {
        globalTargetLocation = globalTargetPos;
        Curve.AddPoint(targetLocation);
        reachedTarget = false;
    }

    public void ChargeAtPlayer()
    {
        Boost(true);
        float overShootValue = 192;
        Vector2 lineToPlayer =  Game.Instance.PlayerShip.GlobalPosition - GlobalHeadPos;
        MoveToDirect(Game.Instance.PlayerShip.GlobalPosition + (lineToPlayer.Normalized() * overShootValue));
    }

    public void Boost(bool yes)
    {
        if(yes) speed = boostSpeed;
        else speed = baseSpeed;
    }

    /// <summary>
	/// Will collapse the worm into 1 tile. Should only be called while worm is already fully collapsed.
	/// </summary>
    public void ResetPath()
    {
        Vector2 newStartPos = Curve.GetPointPosition(Curve.PointCount-1);
        Curve.ClearPoints();
        Curve.AddPoint(newStartPos);
        foreach(PathFollow2D segment in Segments) segment.Progress = 0;
    }

    private void ReachTarget()
    {
        reachedTarget = true;
        Boost(false);
        EmitSignal(SignalName.OnWormReachedDestination);
    }

    private void IncrementProgress(float progress)
    {
        if(Segments[0].ProgressRatio + (progress / Curve.GetBakedLength()) >= 1) ExtendPath();

        for(int i = 0; i < Segments.Length; i++)
        {
            if(Segments[i].ProgressRatio >= 1) 
            {
                if(i == Segments.Length - 1) ReachTarget();
                if(FoldIntoLocation) continue;
                else 
                {
                    ReachTarget();
                    return;
                }
            }
            if(i != 0 && (!FoldIntoLocation))
            {
                float currentProgressGapToNextSegment = Segments[i-1].Progress - Segments[i].Progress;
                if(currentProgressGapToNextSegment < segmentGapDict[Segments[i]]) continue;
            }
            Segments[i].Progress += progress;
        }
    }

    private void ExtendPath()
    {
        Vector2 currentPathEnd = Curve.GetPointPosition(Curve.PointCount - 1);
        Vector2 lineToTarget = targetLocation - currentPathEnd;
        Vector2 directionFromEndOfCurrentPath = lineToTarget.Normalized();
        float distanceFromTarget = lineToTarget.Length();
        if(distanceFromTarget < curvePeriod) Curve.AddPoint(targetLocation);
        else ExtendSlitherPath(currentPathEnd, directionFromEndOfCurrentPath);
    }

    private void ExtendSlitherPath(Vector2 currentPathEnd, Vector2 directionFromEndOfCurrentPath)
    {
        Vector2 perpendicularDirection = directionFromEndOfCurrentPath.Orthogonal();

        for(int variable = 0; variable <= curvePeriod; variable++) // variable refers to the amount of pixels along the straight line that this sin wave is deviating from
        {
            double pointAmplitude = Math.Sin(variable * ((2 * Math.PI) / curvePeriod)) * slitherAmplitude; // think of this as those f(x) sin wave functions from highschool
            Vector2 pointToAdd = currentPathEnd + (directionFromEndOfCurrentPath * variable) + (perpendicularDirection * (float)pointAmplitude);
            Curve.AddPoint(pointToAdd);
        }
    }


}
