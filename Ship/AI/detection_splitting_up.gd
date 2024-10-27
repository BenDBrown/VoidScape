extends Area2D
@onready var player = Game.player_ship
var layer = 5
@onready var parent:Ship = get_parent()
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
	

func _on_area_entered(target: Area2D) -> void:
	var distance:int =  global_position.distance_to(target.global_position)
	var direction_to = global_position.direction_to(target.global_position)
	print(target.get_parent().get_parent().name)
	var target_parent = target.get_parent().get_parent()
	print(direction_to.length())
	if direction_to.length() == 1:
		parent.StartThrustingLeft()
	else:
		parent.StartThrustingRight()



func _on_area_exited(area: Area2D) -> void:
	if area.collision_layer == layer:
		parent.StopThrustingLeft()
		parent.StopThrustingRight()
