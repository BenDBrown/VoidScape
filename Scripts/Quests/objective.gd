@icon("res://Assets/Other/Quest Icon.png")
extends Node
#base class for any quest objective.
class_name Objective

signal completed

enum State{ONGOING, COMPLETED}
@export var state:State = State.ONGOING
@export var objective_name: String;
@export_multiline var objective_description: String
