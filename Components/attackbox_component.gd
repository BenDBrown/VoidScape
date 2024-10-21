extends Area2D
class_name AttackboxComponent
@export var attack_component: AttackComponent

func _ready():
	area_entered.connect(on_area_entered)

func on_area_entered(area):
	prints(get_parent().name, "'s ", name, " hits ", area.get_parent().name, "'s ", area.name)
	if !attack_component:
		return
	if area is HitboxComponent && area.get_parent() != get_parent():
		area.damage(attack_component)
