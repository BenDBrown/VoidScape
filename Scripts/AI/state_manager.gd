@icon("res://Assets/Other/State.png")
extends Node2D
class_name StateManager

signal state_changed
@export var initial_state: AIState
var current_state: AIState
var states: Dictionary = {}


func _ready():
	var children = get_children()
	for state in children:
		if state is AIState:
			states[state.name.to_lower()] = state as AIState
			(state as AIState).transitioned.connect(on_state_transitioned)
	if initial_state:
		current_state = initial_state
		initial_state.enter()


func _process(delta):
	if current_state:
		current_state.update(delta)


func _physics_process(delta):
	if current_state:
		current_state.physics_update(delta)


func on_state_transitioned(state: AIState, new_state: String):
	if state != current_state:
		return
	if states[new_state.to_lower()]:
		current_state = states[new_state.to_lower()]
		current_state.enter()
