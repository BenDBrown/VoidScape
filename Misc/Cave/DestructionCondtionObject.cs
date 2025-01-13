using Godot;
using System;

public partial class DestructionCondtionObject : ConditionObject
{
	[ExportCategory("Developer Input")]
	[Export]
	private float objectHealth = 10;

	[Export]
	private float objectDef = 10;

	[ExportCategory("Do not touch")]
    [Export]
    private Node healthComponent;

	[Export]
	private Node2D targetObject;
	
	private DestroyableObject targetDestroyable;
	private bool destroyed = false;

	[Export]
    private SingleRunAnimation explosionAnim;

	public override void _Ready()
	{
		if (healthComponent != null && healthComponent.HasSignal("died"))
        {
            healthComponent.Connect("died", Callable.From(Destroyed));
        }

		if(targetObject is DestroyableObject){
			targetDestroyable = (DestroyableObject) targetObject;
		}
	}

	public bool IsDestroyed() => destroyed;

	public void Destroyed(){ 
		if(destroyed) {return;}

		destroyed = true;
		ConditionMet = true;
		
		explosionAnim.Visible = true;
		explosionAnim.Play();

		targetDestroyable.DestroyObject();
		EmitSignal(SignalName.OnConditionMet);
	}

}
