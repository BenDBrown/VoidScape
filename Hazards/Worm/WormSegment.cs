using Godot;
using System;

[GlobalClass]
public partial class WormSegment : AnimatableBody2D
{
    [Export]
    private WormSegment connectedSegment = null;

    private float radiansTillBendBack = 0.7f;

    private float radiansToBendPerSecond = 1;

    private int segmentsTillBendBack = 2;

    public bool FlippedRotation { get; private set; } = false; // true means negative rotation direction, false means positive

    public void RotateSegment(double delta, float netRotation, int segmentsBent, bool previousSegmentRotaionFlipped)
    {
        int flipValue = 1;
        if(FlippedRotation) flipValue = -1; 
        Rotation += ((float)delta * radiansToBendPerSecond) * flipValue;
        netRotation += Rotation;

        if(Math.Abs(netRotation) >= radiansTillBendBack) FlippedRotation = Rotation > 0;
        else
        {
            if(segmentsBent < segmentsTillBendBack) segmentsBent++;
            else
            {
                segmentsBent = 1;
                FlippedRotation = !previousSegmentRotaionFlipped;
            }
        }

        if(connectedSegment == null) return;
        connectedSegment.RotateSegment(delta, netRotation, segmentsBent, FlippedRotation);
    }

    public void SetRadiansTillBendBack(float radians)
    {
        radiansTillBendBack = radians;
        if(connectedSegment != null) connectedSegment.SetRadiansTillBendBack(radians);
    }

    public void SetRadiansToBendPerSecond(float radians)
    {
        radiansToBendPerSecond = radians;
        if(connectedSegment != null) connectedSegment.SetRadiansToBendPerSecond(radians);
    }

    public void SetSegmentsTillBendBack(int segments)
    {
        segmentsTillBendBack = segments;
        if(connectedSegment != null) connectedSegment.SetSegmentsTillBendBack(segments);
    }
}
