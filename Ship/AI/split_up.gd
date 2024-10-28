extends Node2D
class_name SplitUp
var split_up_scene =preload("res://Ship/AI/detection_circle_for_splitting_up.tscn")
var detection_circle:Area2D
var dir
var ship:Ship:
	set(value):
		value.add_child(self)
		ship = value
		detection_circle.reparent(value)
	get:
		return ship

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	detection_circle = split_up_scene.instantiate()
	add_child(detection_circle)
	detection_circle.area_entered.connect(on_area_entered)
	detection_circle.area_exited.connect(on_area_exited)
	print("called")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func on_area_entered(target: Area2D) -> void:
	if !ship :
		return
	dir = global_position.direction_to(target.global_position)
	

func get_dir():
	if!dir:
		return Vector2.ZERO
	return dir

func on_area_exited(area: Area2D) -> void:
		dir = Vector2.ZERO
