extends Node
class_name HealthComponent

signal died

@export var max_health: int = 100
@export var defense: int = 10
var current_health: int

func _ready() -> void:
	current_health = max_health

func take_damage(attackComponent: AttackComponent):
	prints(get_parent().name, " took damage")
	current_health -= max(1, attackComponent.attack - defense)
	if current_health <= 0:
		died.emit()
