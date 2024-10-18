extends AIState

class_name Patrol

@export var patrol_radius = 150.0

var start_pos
var point_reached = true
var point: Vector2
var timer: Timer
var direction

func _ready():
	
	start_pos = parent.position
	timer = Timer.new()
	timer.stop()
	timer.timeout.connect(timer.stop)
	add_child(timer)

func enter():
	super.enter()

func exit():
	super.exit()
	point_reached = true
	transitioned.emit(self, "follow")

func physics_update(_delta):
	if timer.is_stopped():
		var x = randf_range(-1, 1)
		var y = randf_range(-1, 1)
		direction = Vector2(x,y)
		timer.start(randf_range(1, 6))
	else: 
		parent.StartThrustingForward()
	rotate(direction+parent.global_position)
	if player.global_position.distance_to(parent.global_position) > detect_radius: return
	exit()

func rotate(globalPos):
	var angle = Utils.get_angle(parent.global_position, globalPos, parent.global_rotation)
	if angle > 0.1:
		parent.StartTurningClockwise()
	elif angle < -0.1:
		parent.StartTurningCounterClockwise()
	else:
		parent.StopTurning()
