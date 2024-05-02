@icon("res://Assets/Other/State.png")
extends Node2D
class_name AIState

signal on_exit;
var is_active: bool = false;

func enter():
	is_active = true;
	print(name + " Entered");

func exit():
	if is_active:
		is_active = false;
		on_exit.emit();
		print(name + " Exited");

