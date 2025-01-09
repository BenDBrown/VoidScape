extends Area2D
class_name AttackboxComponent
@export var attack_component: AttackComponent

signal on_hit

func _ready():
	area_entered.connect(on_area_entered)

func on_area_entered(area):
	if !attack_component:
		return
	if area is HitboxComponent && area.get_parent() != get_parent():
		on_hit.emit()
		area.damage(attack_component)
	
