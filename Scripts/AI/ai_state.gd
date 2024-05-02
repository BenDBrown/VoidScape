@icon("res://Assets/Other/State.png")
extends Node
class_name AIState

@onready var parent = $"../.."
@onready var player = $"../../../PlayerShip"
@export var detect_radius = 200.0

signal transitioned(AIState, new_state:String)

func enter():
	print(name + " Entered")

func exit():
	print(name + " Exited")

func update(_delta):
	pass
func physics_update(_delta):
	pass
