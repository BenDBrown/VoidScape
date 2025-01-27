using Godot;
using System;
using System.Collections.Generic;

public partial class CenterCalculator
{
	public Vector2 GetGlobalShipCenter(List<Vector2> shipVectors)
	{
		Vector2 topLeft = new(int.MaxValue, int.MaxValue);
		Vector2 topRight = new(int.MinValue, int.MaxValue);
		Vector2 bottomLeft = new(int.MaxValue, int.MinValue);
		Vector2 bottomRight = new(int.MinValue, int.MinValue);
		foreach (Vector2 vector in shipVectors)
		{
			if (GetDownLeftMagnitude(vector) > GetDownLeftMagnitude(bottomLeft)) { bottomLeft = vector; }
			if (GetDownRightMagnitude(vector) > GetDownRightMagnitude(bottomRight)) { bottomRight = vector; }
			if (GetUpLeftMagnitude(vector) > GetUpLeftMagnitude(topLeft)) { topLeft = vector; }
			if (GetUpRightMagnitude(vector) > GetUpRightMagnitude(topRight)) { topRight = vector; }
		}

		return new((topLeft.X + topRight.X + bottomLeft.X + bottomRight.X) / 4, (topLeft.Y + topRight.Y + bottomLeft.Y + bottomRight.Y) / 4);
	}

	/// <summary>
	/// Returns a vector that represents the size of the ship in unit of shipComponents
	/// </summary>
	public Vector2 GetNrOfComponentsScale(List<Vector2> shipVectors)
	{
		float minX = float.MaxValue;
		float maxX = float.MinValue;
		float minY = float.MaxValue;
		float maxY = float.MinValue;
		foreach (Vector2 vector in shipVectors)
		{
			if (vector.X > maxX) maxX = vector.X;
			if (vector.X < minX) minX = vector.X;
			if (vector.Y > maxY) maxY = vector.Y;
			if (vector.Y < minY) minY = vector.Y;
		}

		int shieldSpriteSize = 32; // base shield asset is designed to enclose a 32x32 square
		return new((maxX - minX) / shieldSpriteSize, (maxY - minY) / shieldSpriteSize);
	}

	private float GetDownRightMagnitude(Vector2 vector) => vector.X + vector.Y;

	private float GetDownLeftMagnitude(Vector2 vector) => -vector.X + vector.Y;

	private float GetUpRightMagnitude(Vector2 vector) => vector.X - vector.Y;

	private float GetUpLeftMagnitude(Vector2 vector) => -vector.X - vector.Y;
}
