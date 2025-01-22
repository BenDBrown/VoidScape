extends Area2D

func _on_area_entered(node : Node2D):
	if(node == Game.PlayerShip):
		set_spawn_point()
		
func set_spawn_point():
	print("setting spawn point")
	var spawnPointSave = SpawnPointSave.new()
	spawnPointSave = spawnPointSave.load_save()
	spawnPointSave.spawn_point = global_position
	spawnPointSave.save()
