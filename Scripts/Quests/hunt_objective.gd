extends Objective

class_name HuntObjective

@export var hunt_target: PackedScene

func is_target(target):
	if state == State.COMPLETED: return
	if target.is_instance_of(hunt_target):
		completed.emit()
		state = State.COMPLETED
