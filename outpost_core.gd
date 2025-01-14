extends Node2D
signal core_destroyed
@onready var death_anim = $BulletDeathAnimation


func _on_health_component_died() -> void:
	emit_signal("core_destroyed")
	death_anim.visible = true
	death_anim.play()
