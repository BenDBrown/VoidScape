using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Worm : Path2D
{
    [Signal]
    public delegate void OnWormReachedDestinationEventHandler();

    [Export]
    private PathFollow2D[] Segments;

    [Export]
    private float curvePeriod = 96; // in pixels

    [Export]
    private float slitherAmplitude = 64; // in pixels

    [Export]
    private double speed = 100;

    [Export]
    private float targetPosLeeway = 3;

    [Export]
    public bool FoldIntoLocation = false;

    public Vector2 GlobalHeadPos => Segments[0].GlobalPosition;

    private Vector2 globalTargetLocation = Vector2.Zero;

    private Vector2 targetLocation => globalTargetLocation - GlobalPosition;

    // records the distance between segments with each segment corresponding to the distance to the proceeding segment
    // the array starts with the head so this means that the head should always have a value of 0 while the tail
    // should always have the distance to the second to last piece of the worm
    private Dictionary<PathFollow2D, float> segmentGapDict = new(); 

    private bool reachedTarget = false;

    public void MoveTo(Vector2 targetPos)
    {
        globalTargetLocation = targetPos;
        reachedTarget = false;
    }

    /// <summary>
	/// Takes a global point to move through as a parameter.
	/// </summary>
    public void AddPointToMoveThrough(Vector2 point)
    {
        Vector2 newPoint = point - GlobalPosition;
        Curve.AddPoint(newPoint);
        GD.Print("added via moveThrough: " + newPoint);
    }

    public override void _Ready()
    {
        base._Ready();
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

    /// <summary>
	/// Will collapse the worm into 1 tile. Should only be called while worm is already fully collapsed.
	/// </summary>
    public void ResetPath()
    {
        Vector2 newStartPos = Curve.GetPointPosition(Curve.PointCount-1);
        Curve.ClearPoints();
        Curve.AddPoint(newStartPos);
        foreach(PathFollow2D segment in Segments) segment.Progress = 0;
        GD.Print("path reset added: " + newStartPos);
    }

    private void ReachTarget()
    {
        reachedTarget = true;
        EmitSignal(SignalName.OnWormReachedDestination);
    }

    private void IncrementProgress(float progress)
    {
        if(Segments[0].ProgressRatio + (progress / Curve.GetBakedLength()) >= 1) ExtendPath();

        for(int i = 0; i < Segments.Length; i++)
        {
            if
            (
                Math.Abs(Segments[i].Position.X - targetLocation.X) <= targetPosLeeway
                &&
                Math.Abs(Segments[i].Position.Y - targetLocation.Y) <= targetPosLeeway
            ) 
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
        if(distanceFromTarget < curvePeriod)
        {
            Curve.AddPoint(targetLocation);
            GD.Print("added via straight: " + targetLocation);
        }
        else ExtendSlitherPath(currentPathEnd, directionFromEndOfCurrentPath);
    }

    private void ExtendSlitherPath(Vector2 currentPathEnd, Vector2 directionFromEndOfCurrentPath)
    {
        Vector2 perpendicularDirection = directionFromEndOfCurrentPath.Orthogonal();

        for(int variable = 0; variable <= curvePeriod; variable++) // variable refers to the amount of pixels along the straight line that this sin wave is deviating from
        {
            double pointAmplitude = Math.Sin(variable * ((2 * Math.PI) / curvePeriod)) * slitherAmplitude; // think of this as those f(x) sin wave functions from highschool
            Vector2 pointToAdd = currentPathEnd + (directionFromEndOfCurrentPath * variable) + (perpendicularDirection * (float)pointAmplitude);
            GD.Print("added via slither: " + pointToAdd);
            Curve.AddPoint(pointToAdd);
        }
    }


}
