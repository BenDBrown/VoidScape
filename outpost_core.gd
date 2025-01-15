extends Node2D
signal core_destroyed
@onready var death_anim = $BulletDeathAnimation
@onready var hitbox =$HitboxComponent
@onready var sprite = $Sprite2D


func _on_health_component_died() -> void:
	emit_signal("core_destroyed")
	sprite.visible = false
	hitbox.queue_free()
	death_anim.visible = true
	death_anim.play()
