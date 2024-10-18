using Godot;
using System;
using System.Collections.Generic;

public partial class RotationManager
{
	public Vector2 LeftRotationPoint {get; private set;}

	public Vector2 RightRotationPoint {get; private set;}

	private RotationDirection rotationDirection = RotationDirection.None;

	public void StartTurningClockwise() => rotationDirection = RotationDirection.Right;
	
	public void StartTurningCounterClockwise() => rotationDirection = RotationDirection.Left;

	public void StopTurning() => rotationDirection = RotationDirection.None;

	public Vector2 GetRotation(float currentRot, float rotationSpeed, double deltaTime, out float newRot)
	{
		if(rotationDirection == RotationDirection.None)
		{
			newRot = currentRot;
			return Vector2.Zero;
		}
		float rotChange = (float)(rotationSpeed * deltaTime);
		if(rotationDirection == RotationDirection.Left) { rotChange *= -1;}
		newRot = currentRot + rotChange;
		Vector2 rotEdgeVector;
		Vector2 relativeRotationPoint;
		if(rotationDirection == RotationDirection.Right)
		{
			rotEdgeVector = RightRotationPoint.Rotated(newRot);
			relativeRotationPoint = RightRotationPoint.Rotated(currentRot);
			return relativeRotationPoint - rotEdgeVector;
		}
		else
		{
			rotEdgeVector = LeftRotationPoint.Rotated(newRot);
			relativeRotationPoint = LeftRotationPoint.Rotated(currentRot);
			return rotEdgeVector - relativeRotationPoint;
		}
	}

	public Vector2 CalculateCentreOfMass(List<Vector2> vertices)
	{
		Vector2 topLeft = new(int.MaxValue, int.MaxValue);
		Vector2 topRight = new(int.MinValue, int.MaxValue);
		Vector2 bottomLeft = new(int.MaxValue, int.MinValue);
		Vector2 bottomRight = new(int.MinValue, int.MinValue);
		foreach(Vector2 vertice in vertices)
		{
			if(GetDownLeftMagnitude(vertice) > GetDownLeftMagnitude(bottomLeft)) { bottomLeft = vertice; }
			if(GetDownRightMagnitude(vertice) > GetDownRightMagnitude(bottomRight)) { bottomRight = vertice; }
			if(GetUpLeftMagnitude(vertice) > GetUpLeftMagnitude(topLeft)) { topLeft = vertice; }
			if(GetUpRightMagnitude(vertice) > GetUpRightMagnitude(topRight)) { topRight = vertice; }
		}
		LeftRotationPoint = (topLeft + bottomLeft) / 2;
		RightRotationPoint = (topRight + bottomRight) / 2;
		return new((topLeft.X + topRight.X + bottomLeft.X + bottomRight.X)/4, (topLeft.Y + topRight.Y + bottomLeft.Y + bottomRight.Y)/4);
	}

	private float GetDownRightMagnitude(Vector2 vector) => vector.X + vector.Y;

	private float GetDownLeftMagnitude(Vector2 vector) => -vector.X + vector.Y;

	private float GetUpRightMagnitude(Vector2 vector) => vector.X - vector.Y;

	private float GetUpLeftMagnitude(Vector2 vector) => -vector.X - vector.Y;

	private enum RotationDirection
	{
		None,
		Right,
		Left
	}

}

