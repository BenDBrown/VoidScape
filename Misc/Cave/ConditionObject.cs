using Godot;
using System;

public partial class ConditionObject : Node2D
{
    [Signal]
    public delegate void OnConditionMetEventHandler();

    private bool conditionMet = false;

    public bool ConditionMet
	{
		get { return conditionMet; }
		protected set
		{
			conditionMet = value;
		}
	}
}
