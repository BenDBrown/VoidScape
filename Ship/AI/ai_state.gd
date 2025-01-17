@icon("res://Assets/Other/State.png")
extends Node
class_name AIState

@onready var parent:Ship = get_parent().get_parent()
@onready var player: Ship:
	get:
		if !Game.PlayerShip:
			return $"../../../PlayerShip"
		return Game.PlayerShip
@export var detect_radius = 200.0

signal transitioned(AIState, new_state:String)

func enter():
	pass
	#print(name + " Entered")

func exit():
	pass
	#print(name + " Exited")

func update(_delta):
	pass
func physics_update(_delta):
	pass
