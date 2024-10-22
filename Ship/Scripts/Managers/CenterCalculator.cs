using Godot;
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

	private float GetDownRightMagnitude(Vector2 vector) => vector.X + vector.Y;

	private float GetDownLeftMagnitude(Vector2 vector) => -vector.X + vector.Y;

	private float GetUpRightMagnitude(Vector2 vector) => vector.X - vector.Y;

	private float GetUpLeftMagnitude(Vector2 vector) => -vector.X - vector.Y;
}
