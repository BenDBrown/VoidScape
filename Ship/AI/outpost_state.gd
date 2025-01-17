extends AIState
class_name Outpost_State
@export var max_distance = 600
@export var min_distance = 150
var combat:Combat
var player_in_outpost = false

func enter():
	super.enter()
	if !combat:
		combat = Combat.new()
	combat.enter(parent)


func exit():
	super.exit()
	combat.exit()
	transitioned.emit(self, "outpost_idle")

func physics_update(_delta):
	if combat:
		combat.physics_update(player)
	rotate_towards(player.global_position)
	var dist = parent.global_position.distance_to(player.global_position)
	if dist > max_distance:
		parent.StartThrustingForward()
	elif dist < min_distance:
		retreat()
	else:
		parent.StopThrustingForward()

func rotate_towards(globalPos: Vector2):
	var angle: float = Utils.get_angle(parent.global_position, globalPos, parent.global_rotation)
	if angle > 0.3:
		parent.StartTurningClockwise()
	elif angle < -0.3:
		parent.StartTurningCounterClockwise()
	else:
		parent.StopTurning()

func player_is_in_outpost():
	player_in_outpost = true

func retreat():
	parent.StopTurning()
	parent.StartThrustingBackward()
