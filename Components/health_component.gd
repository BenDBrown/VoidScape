extends Node
class_name HealthComponent

signal died
signal health_changed(current_health:int)

@export var max_health: int = 100
@export var defense: int = 10
var current_health: int

func _ready() -> void:
	current_health = max_health

func take_damage(attackComponent: AttackComponent):
	current_health -= max(1, attackComponent.attack - defense)
	health_changed.emit(current_health)
	if current_health <= 0:
		died.emit()

func set_component(maxHealth, def):
	max_health = maxHealth
	defense = def
	current_health = maxHealth
