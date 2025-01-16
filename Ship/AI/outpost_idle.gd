extends AIState
class_name Outpost_Idle
var detect = 500

func update(_delta):
	if parent.global_position.distance_to(player.global_position) < detect:
		exit()
		transitioned.emit(self, "outpost_state")
func enter():
	super.enter()
func exit():
	super.exit()
