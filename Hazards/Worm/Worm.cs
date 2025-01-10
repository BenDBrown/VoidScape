using Godot;
using System;

[GlobalClass]
public partial class Worm : Path2D
{
    [Export]
    private PathFollow2D[] Segments;

    [Export]
    private float curvePeriod = 96; // in pixels

    [Export]
    private float slitherAmplitude = 64; // in pixels

    [Export]
    private double speed = 100;

    private Vector2 globalTargetLocation = Vector2.Zero;

    private Vector2 targetLocation => globalTargetLocation - Position;

    public void MoveTo(Vector2 targetPos) => globalTargetLocation = targetPos;

    public override void _PhysicsProcess(double delta)
    {
        IncrementProgress((float)(delta * speed));
    }

    private void IncrementProgress(float progress)
    {
        if(Segments[0].ProgressRatio + (progress / Curve.GetBakedLength()) >= 1) ExtendPath();

        for(int i = 0; i < Segments.Length; i++)
        {
            Segments[i].Progress += progress;
        }
    }

    private void ExtendPath()
    {
        Vector2 currentPathEnd = Curve.GetPointPosition(Curve.PointCount - 1);
        Vector2 directionFromEndOfCurrentPath = (targetLocation - currentPathEnd).Normalized();
        Vector2 perpendicularDirection = directionFromEndOfCurrentPath.Orthogonal();

        for(int variable = 0; variable <= curvePeriod; variable++) // variable refers to the amount of pixels along the straight line that this sin wave is deviating from
        {
            double pointAmplitude = Math.Sin(variable * ((2 * Math.PI) / curvePeriod)) * slitherAmplitude; // think of this as those f(x) sin wave functions from highschool
            Vector2 pointToAdd = currentPathEnd + (directionFromEndOfCurrentPath * variable) + (perpendicularDirection * (float)pointAmplitude);
            Curve.AddPoint(pointToAdd);
        }

    }

}
