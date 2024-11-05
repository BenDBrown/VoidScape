extends Node2D
class_name SplitUp
var split_up_scene = preload("res://Ship/AI/detection_circle_for_splitting_up.tscn")
var detection_circle: Area2D
var dir = Vector2.ZERO
var target;
var ship: Ship
	
func enter(new_ship):
	ship = new_ship
	detection_circle = split_up_scene.instantiate()
	ship.add_child(detection_circle)
	detection_circle.position = Vector2.ZERO
	detection_circle.area_entered.connect(on_area_entered)
	detection_circle.area_exited.connect(on_area_exited)
	print("split_up added")

func exit():
	detection_circle.queue_free()
	print("split_up removed")

func on_area_entered(area: Area2D) -> void:
	if !ship:
		return
	if !target:
		target = area;

func get_dir():
	if !target:
		return Vector2.ZERO
	return global_position.direction_to(target.global_position)

func on_area_exited(_area: Area2D) -> void:
		target = null
