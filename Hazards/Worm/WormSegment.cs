using Godot;
using System;

[GlobalClass]
public partial class WormSegment : AnimatableBody2D
{
    [Export]
    protected WormSegment connectedSegment = null;

    [Export]
    protected int segmentsToBendDirectionSwap = 3;

    [Export]
    protected float radiansToBendPerSecond = 1;

    public void RotateSegment(double delta, int segmentsBentInDirection, bool flippedRotation)
    {
        int flipValue = 1;
        if(flippedRotation) flipValue = -1; 
        Rotation += ((float)delta * radiansToBendPerSecond) * flipValue;
        if(connectedSegment == null) return;

        if(segmentsBentInDirection >= segmentsToBendDirectionSwap)
        {
            flippedRotation = !flippedRotation;
            segmentsBentInDirection = 0;
        }
        else segmentsBentInDirection++;
        connectedSegment.RotateSegment(delta, segmentsBentInDirection, flippedRotation);
    }

    public void SetSegmentsToBendDirectionSwap(int segments)
    {
        segmentsToBendDirectionSwap = segments;
        if(connectedSegment != null) connectedSegment.SetSegmentsToBendDirectionSwap(segments);
    }

    public void SetRadiansToBendPerSecond(float radians)
    {
        radiansToBendPerSecond = radians;
        if(connectedSegment != null) connectedSegment.SetRadiansToBendPerSecond(radians);
    }
}
