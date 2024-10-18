using Godot;
using System;

public interface IShip
{
	public void StartShooting();

	public void StopShooting();

	public void StartThrustingForward();

	public void StartThrustingBackward();

	public void StartThrustingRight();

	public void StartThrustingLeft();

	public void StopThrustingForward();

	public void StopThrustingBackward();

	public void StopThrustingRight();

	public void StopThrustingLeft();

	public void StartTurningClockwise();

	public void StartTurningCounterClockwise();

	public void StopTurning();

	public bool TryBuildShip();
}
