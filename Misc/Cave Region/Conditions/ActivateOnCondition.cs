using Godot;
using System;

public partial class ActivateOnCondition : Node
{
	[Signal]
	public delegate void ActivateEventHandler();

	[Signal]
	public delegate void OnProgressChangedEventHandler();

	[Export]
	private ConditionObject[] conditionTargets;

	private int completedTargets = 0;

	public override void _Ready()
	{
		base._Ready();
		SubscribeToSignals();
	}

	/// <summary>
	/// This provides an overview of the total required conditions that are part of this event.
	/// </summary>
	/// <returns>Returns the number of required conditions.</returns>
    public int GetAmountOfConditions(){
        return conditionTargets.Length;
    }

	/// <summary>
	/// This provides an overview on the amount of completed condition for the main event.
	/// </summary>
	/// <returns>Returns the amount of completed conditions.</returns>
    public int GetAmountCompleted(){
        return completedTargets;
    }

    private void SubscribeToSignals(){
        foreach( ConditionObject target in conditionTargets){
            target.OnConditionMet += TrackProgress;
        }
    }

	private void TrackProgress(){
		completedTargets++;
		EmitSignal(SignalName.OnProgressChanged);

		if(HasRequirementMet(completedTargets)){
			TryCompleteCondition();
		}
	}

	private void TryCompleteCondition(){
		// Re-checking wether the condition is actually met.
		int progressCheck = 0;
		foreach(ConditionObject target in conditionTargets){

			if(target.ConditionMet) {
				progressCheck++;
				}
		}

        if(HasRequirementMet(progressCheck)){
            
            EmitSignal(SignalName.Activate);
        }
    }

	private bool HasRequirementMet(int currentProgress){
		return conditionTargets.Length == currentProgress;
	}
}
