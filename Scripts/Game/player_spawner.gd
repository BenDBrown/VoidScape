extends Node

@onready var parent = $Node2D
var saver: ShipSaver = ShipSaver.new()

func _ready():
	saver.create(parent)
