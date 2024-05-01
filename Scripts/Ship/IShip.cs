using Godot;
using System;

public interface IShip
{
	public void StartShooting();

	public void StopShooting();

	public void ForwardThrust();

	public void BackThrust();

	public void StopThrusting();

	public void StartTurningClockwise();

	public void StartTurningCounterClockwise();

	public void StopTurning();

	public bool TryBuildShip();
}
