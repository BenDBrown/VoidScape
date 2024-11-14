extends AIState

class_name Idle

@export var wait_time: float = 3.0

var timer:Timer

func _ready():
	timer = Timer.new()
	timer.timeout.connect(on_timeout)
	add_child(timer)

func update(_delta):
	if parent.global_position.distance_to(player.global_position) < detect_radius:
		exit()
		transitioned.emit(self, "follow")
func enter():
	super.enter()
	timer.start(wait_time)

func exit():
	super.exit()
	timer.stop()
	
func on_timeout():
	exit()
	transitioned.emit(self, "patrol")
