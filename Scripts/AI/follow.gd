extends AIState

class_name Follow

@onready var parent = $"../..";
@export var distance = 30;
var target;

func _physics_process(delta):
	if is_active:
		follow_target(delta);
func enter():
	super.enter()
	parent.StartShooting()
	
func exit():
	super.exit()
	parent.StopShooting()
	
func follow_target(delta):
	if !target: return;
	if target is Node2D:
		var dist = parent.global_position.distance_to(target.global_position)
		if dist > 150:
			move_to(target.global_position);
		elif dist < 100:
			retreat()
		else:
			parent.StopThrusting()
			parent.StopTurning()
	elif target is Vector2:
		move_to(target);
		if parent.position.distance_to(target) < distance:
			exit();
			parent.StopTurning();
			parent.StopThrusting();

func move_to(globalPos):
	var direction: Vector2 = globalPos - parent.global_position
	var targetAngle: float = direction.angle()
	var currentAngle: float = parent.global_rotation
	var difference: float = targetAngle - currentAngle
	
	if difference > PI:
		difference -= 2 * PI
	elif difference < -PI:
		difference += 2 * PI
	
	# Adjust the angle so that positive y direction is considered 0 degrees
	var adjustedDifference: float = difference + PI / 2
	
	if adjustedDifference > PI:
		adjustedDifference -= 2 * PI
	elif adjustedDifference < -PI:
		adjustedDifference += 2 * PI
		
	
	parent.ForwardThrust()
	if adjustedDifference > 0.1:
		parent.StartTurningClockwise()
		pass;
	elif adjustedDifference < -0.1:
		pass;
		parent.StartTurningCounterClockwise()
	else:
		parent.StopTurning()

func retreat():
	parent.StopTurning();
	parent.BackThrust();
