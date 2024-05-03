extends Node

@export var saver:ShipSaver = preload("res://Scripts/Resources/player_ship_saver.gd")

@onready var parent = $Node2D
func _ready():
	saver.create(parent)
