using Godot;
using System;

public partial class DestroyableObject : Node2D
{
	[Export]
	private Node2D objectToDestroy;

	public void DestroyObject(){
		objectToDestroy.QueueFree();
	}
}
