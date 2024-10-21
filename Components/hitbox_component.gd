extends Area2D
class_name HitboxComponent

@export var healthComponent: HealthComponent

func damage(attackComponent: AttackComponent):
	prints("damage called from hitbox ", name)
	if healthComponent:
		healthComponent.take_damage(attackComponent)
	pass
