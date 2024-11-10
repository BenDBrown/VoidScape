extends Area2D
class_name HitboxComponent

@export var healthComponent: HealthComponent
@export var collision_shape: CollisionShape2D

func _ready() -> void:
	if healthComponent:
		healthComponent.died.connect(disable_collider)

func damage(attackComponent: AttackComponent):
	if healthComponent:
		healthComponent.take_damage(attackComponent)

func disable_collider():
	if collision_shape:
		collision_shape.set_deferred("disabled", true)
