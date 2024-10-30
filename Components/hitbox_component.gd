extends Area2D
class_name HitboxComponent

@export var healthComponent: HealthComponent

func damage(attackComponent: AttackComponent):
	if healthComponent:
		healthComponent.take_damage(attackComponent)