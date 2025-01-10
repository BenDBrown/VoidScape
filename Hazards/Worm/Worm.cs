using Godot;
using System;

[GlobalClass]
public partial class Worm : Path2D
{
    [Export]
    private PathFollow2D[] Segments;

    [Export]
    private float curvePeriod; // in pixels

    [Export]
    private int NrOfPointsPerPeriod = 20;

    [Export]
    private double speed = 100;

    private Vector2 targetLocation = Vector2.Zero;

    public void MoveTo(Vector2 globalTarget) => targetLocation = globalTarget;

    public override void _Ready()
    {
        base._Ready();
        MakeInitialPath();
    }

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
        int nrOfPoints = 0;
        for(int variable = 0; variable < nrOfPoints; variable++)
        {
            double pointAmplitude = Math.Sin(variable * ((2 * Math.PI) / curvePeriod));
            // think of this as those f(x) sin wave functions from highschool
        }

    }

    private void MakeInitialPath()
    {
        Curve2D curve = new();
        for(int i = Segments.Length-1; i >= 0; i--) curve.AddPoint(Segments[i].Position);
    }

}
