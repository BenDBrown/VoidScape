extends AIState


func update(_delta):
	if parent.global_position.distance_to(player.global_position) < detect_radius:
		exit()
		transitioned.emit(self, "outpost_state")
func enter():
	super.enter()
func exit():
	super.exit()
