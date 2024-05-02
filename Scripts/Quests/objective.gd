@icon("res://Assets/Other/Quest Icon.png")
extends Node
#base class for any quest objective.
class_name Objective

signal completed

enum State{ONGOING,COMPLETED}
@export var state = State.ONGOING

@export var objective_name: String:
	get:
		return objective_name
	set(value):
		objective_name = value

@export_multiline var objective_description: String:
	get:
		return objective_description
	set(value):
		objective_description = value

func get_status(): #override this if you want custom message like: ONGOING: 0/10 scouts defeated
	return state
