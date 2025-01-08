using Godot;
using System;

[GlobalClass]
public partial class Worm : WormSegment
{
    [Export]
    private double speed = 100;

    [Export]
    private float targetLeeway = 5;

    [Export]
    private float maxAngleFromTarget = 0.7f;

    private Vector2 targetLocation = Vector2.Zero;

    private bool flippedRotation = false; // true means negative rotation direction, false means positive

    public void MoveTo(Vector2 globalTarget) => targetLocation = globalTarget;
    public override void _PhysicsProcess(double delta)
    {
        if(GlobalPosition.DistanceTo(targetLocation) < targetLeeway) return;
        MoveAndCollide(Vector2.Up.Rotated(GlobalRotation) * (float)(speed * delta));
        Vector2 direction = (targetLocation - GlobalPosition).Rotated(-GlobalRotation); // direction is a global direction, should be local FIX TMRW
        direction = direction.Normalized();
        float angleToTarget = Vector2.Up.AngleTo(direction);
        float inverseAngleToTarget = ((2 * (float)Math.PI) - angleToTarget);

        if(angleToTarget >= maxAngleFromTarget && inverseAngleToTarget >= maxAngleFromTarget) // this is for when target is outside of max target angle
        {
            if(angleToTarget > inverseAngleToTarget) flippedRotation = true;
            else flippedRotation = false;
        }
        RotateSegment(delta, 0, flippedRotation);
    }

    public override void _Ready()
    {
        targetLocation = GlobalPosition;
        connectedSegment.SetSegmentsToBendDirectionSwap(segmentsToBendDirectionSwap);
        connectedSegment.SetRadiansToBendPerSecond(radiansToBendPerSecond);
    }

}
