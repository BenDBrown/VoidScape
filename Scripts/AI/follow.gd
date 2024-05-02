extends AIState

class_name Follow
@export var min_distance: float = 100
@export var max_distance: float = 150
@export var out_of_detection_distance: float = 300

func enter():
	super.enter()
	#parent.StartShooting()
	
func exit():
	super.exit()
	#parent.StopShooting()
	parent.StopTurning()
	parent.StopThrusting()
	transitioned.emit(self, "idle")

func physics_update(_delta):
	var dist = parent.global_position.distance_to(player.global_position)
	if dist > out_of_detection_distance:
		exit()
	elif dist > max_distance:
		parent.ForwardThrust()
		rotate(player.global_position)
	elif dist < min_distance:
		retreat()
	else:
		parent.StopThrusting()
		parent.StopTurning()

func rotate(globalPos: Vector2):
	var angle:float = Utils.get_angle(parent.global_position, globalPos, parent.global_rotation)
	if angle > 0.1:
		parent.StartTurningClockwise()
		pass
	elif angle < -0.1:
		pass
		parent.StartTurningCounterClockwise()
	else:
		parent.StopTurning()

func retreat():
	parent.StopTurning()
	parent.BackThrust()
