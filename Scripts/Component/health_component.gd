extends Node
class_name HealthComponent

signal died

@export var health : int = 10

func take_damage(damageInfo):
	health -= damageInfo.damage
	if health <= 0:
		died.emit()
