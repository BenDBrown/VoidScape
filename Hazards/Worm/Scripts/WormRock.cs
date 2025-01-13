using Godot;
using System;
using System.Collections.Generic;

public partial class WormRock : StaticBody2D
{
    [Export]
    private Node2D[] holeLocations;

    /// <summary>
	/// Returns the global position of the nearest hole entry point.
	/// </summary>
    public Vector2 GetNearestHole(Vector2 globalPoint)
    {
        if(!HoleAssignmentCheck()) return Vector2.Zero;

        float distance = (holeLocations[0].GlobalPosition - globalPoint).Length();
        Node2D chosenNode = holeLocations[0];
        foreach (Node2D hole in holeLocations) 
        {
            float newDistance = (hole.GlobalPosition - globalPoint).Length();
            if(newDistance < distance)
            {
                distance = newDistance;
                chosenNode = hole;
            }
        }

        return chosenNode.GlobalPosition;
    }

    /// <summary>
	/// Returns the global position of a random hole entry point.
	/// </summary>
    public Vector2 GetRandomHoleEntrance()
    {
        if(!HoleAssignmentCheck()) return Vector2.Zero;
        
        Random rng = new();
        int randomHoleIndex = rng.Next(0, holeLocations.Length);
        return holeLocations[randomHoleIndex].GlobalPosition;
    }

        /// <summary>
	/// Returns the global position of a random hole exit point.
	/// </summary>
    public Vector2 GetRandomHoleExit()
    {
        if(!HoleAssignmentCheck()) return Vector2.Zero;
        
        Random rng = new();
        int randomHoleIndex = rng.Next(0, holeLocations.Length);
        return holeLocations[randomHoleIndex].GlobalPosition + holeLocations[randomHoleIndex].Position;
        // this is a gross work around but time pressure
    }

    private bool HoleAssignmentCheck()
    {
        bool assigned = holeLocations.Length > 0;
        if(!assigned) GD.PrintErr("worm rock must have at least 1 hole location assigned");
        return assigned;
    }

}
