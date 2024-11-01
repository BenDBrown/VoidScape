extends AIState

class_name Follow
@export var min_distance: float = 150
@export var max_distance: float = 300
@export var out_of_detection_distance: float = 500
var split_up: SplitUp
var combat_scene = preload("res://Ship/AI/ai_combat.tscn")
var combat: Combat

func enter():
	super.enter()
	if !split_up:
		split_up = SplitUp.new()
	if !combat:
		combat = combat_scene.instantiate()
		combat.ship = parent

	split_up.enter(parent)
	combat.enter(parent)

func exit():
	super.exit()
	split_up.exit()
	combat.exit()

	parent.StopTurning()
	parent.StopThrustingForward()
	transitioned.emit(self, "idle")
	parent.remove_child(combat)
	

func physics_update(_delta):
	if combat:
		combat.physics_update(player)

	rotate_towards(player.global_position)

	var splitDir = split_up.get_dir()
	var dist = parent.global_position.distance_to(player.global_position)
	if splitDir.x > 0:
		parent.StartThrustingRight()
	elif splitDir.x < 0:
		parent.StartThrustingLeft()
	else:
		parent.StopThrustingLeft()
		parent.StopThrustingRight()

	if dist > out_of_detection_distance:
		exit()
	elif dist > max_distance:
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

func retreat():
	parent.StopTurning()
	parent.StartThrustingBackward()


func strafe_around_target(_target):
	pass
	#This Func should allow the NPC to move around the player while they are shooting the player
	#TODO
