extends Node

func _ready():
	var ship = Game.json_to_node();
	get_parent().add_child(ship);
	queue_free()
