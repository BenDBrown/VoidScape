class_name weapon_option_ui
extends Node2D

@onready var weapon_type: Sprite2D = $WeaponType
@onready var border: AnimatedSprite2D = $Border
		
func set_type_icon(texture: Texture):
	weapon_type.texture = texture

func clear_type_icon():
	weapon_type.texture = null
	
func set_selected():
	border.frame = 1
	
func set_unselected():
	border.frame = 0
	
func set_unavailable():
	border.frame = 2
