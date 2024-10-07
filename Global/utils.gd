extends Node


func get_angle(from: Vector2, to: Vector2, fromRot: float) -> float:
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
