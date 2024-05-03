extends Node

const SAVE_PATH = "res://save.bin"

func save_game():
	var file = FileAccess.open(SAVE_PATH, FileAccess.WRITE)
	file.store_line(Game.save_data)
	
func load_game():
	if !FileAccess.file_exists("res://Resources/Ship.tres"):
		return false
	return true # for testing purposes only. Remove after testing
	
	var file = FileAccess.open(SAVE_PATH, FileAccess.READ)
	var data = JSON.parse_string(file.get_line())
	Game.save_data = data
	return true

func get_angle(from:Vector2, to:Vector2, fromRot: float) -> float:
	var direction: Vector2 = to - from
	var targetAngle: float = direction.angle()
	var difference: float = targetAngle - fromRot
	
	if difference > PI:
		difference -= 2 * PI
	elif difference < -PI:
		difference += 2 * PI
	var adjustedDifference: float = difference + PI / 2
	
	if adjustedDifference > PI:
		adjustedDifference -= 2 * PI
	elif adjustedDifference < -PI:
		adjustedDifference += 2 * PI
	return adjustedDifference
