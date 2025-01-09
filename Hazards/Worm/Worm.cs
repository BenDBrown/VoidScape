using Godot;
using System;

[GlobalClass]
public partial class Worm : AnimatableBody2D
{
    [Export]
    private WormSegment headSegment = null;

    [Export]
    private double speed = 100;

    [Export]
    private float targetLeeway = 5;

    [Export]
    private float maxAngleFromTarget = 0.09f;

    [Export]
    private float radiansTillBendBack = 0.7f;

    [Export]
    private float radiansToBendPerSecond = 1;

    [Export]
    private int segmentsTillBendBack = 2;

    private Vector2 targetLocation = Vector2.Zero;

    public void MoveTo(Vector2 globalTarget) => targetLocation = globalTarget;
    public override void _PhysicsProcess(double delta)
    {
        if(GlobalPosition.DistanceTo(targetLocation) < targetLeeway) return;
        Vector2 moveVector = Vector2.Up.Rotated(GlobalRotation) * (float)(speed * delta);
        MoveAndCollide(moveVector);
        Vector2 lineToTarget = targetLocation - GlobalPosition; // direction is a global direction, should be local FIX TMRW
        lineToTarget = lineToTarget.Normalized();
        float angleToTarget = moveVector.Normalized().AngleTo(lineToTarget);

        if(Math.Abs(angleToTarget) > maxAngleFromTarget)
        {
            int flipValue;
            if(angleToTarget == 0) flipValue = 0;
            else if (angleToTarget > 0) flipValue = 1;
            else flipValue = -1;
            Rotation += ((float)delta * radiansToBendPerSecond) * flipValue;
        }

        headSegment.RotateSegment(delta, 0, 0, headSegment.FlippedRotation);
    }

    public override void _Ready()
    {
        targetLocation = GlobalPosition;
        headSegment.SetRadiansTillBendBack(radiansTillBendBack);
        headSegment.SetRadiansToBendPerSecond(radiansToBendPerSecond);
        headSegment.SetSegmentsTillBendBack(segmentsTillBendBack);
    }

}
