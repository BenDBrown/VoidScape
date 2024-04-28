extends Objective
class_name EliminationObjective

@export var elim_target: PackedScene;
@export var max_amount: int;
@onready var current_amount = 0;

func is_target(target):
	if state == "COMPLETED":
		return
	if !target.is_instance_of(elim_target):
		return
		
	current_amount += 1;
	if current_amount >= max_amount:
		completed.emit();	
