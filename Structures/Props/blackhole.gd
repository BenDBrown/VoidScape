extends Node2D

var ships = []
@export var pull = 12000

func _physics_process(delta: float) -> void:
	for ship in ships:
		var dist = ship.global_position.distance_to(global_position)
		dist = abs(dist)
		ship.AddExternalImpulse(ship.global_position.direction_to(global_position) * pull * 1 / dist)


func _on_area_2d_body_entered(body: Node2D) -> void:
	if body is Ship:
		ships.append(body)


func _on_area_2d_body_exited(body: Node2D) -> void:
	if body is Ship:
		ships.erase(body)
