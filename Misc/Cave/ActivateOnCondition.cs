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
            GD.Print("OPEN DOOR");
            EmitSignal(SignalName.Activate);
        }
    }

    private bool HasRequirementMet(int currentProgress){
        return conditionTargets.Length == currentProgress;
    }
}
