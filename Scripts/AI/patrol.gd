extends AIState

class_name Patrol

@export var radius = 200.0;
@export var distance = 30.0;
@onready var parent = $"../..";
var start_pos;
var point_reached = true;
var point: Vector2;
var timer: Timer;

func _ready():
	parent = get_parent().get_parent();
	start_pos = parent.position;
	timer = Timer.new();
	timer.timeout.connect(timeout);
	add_child(timer);

func _physics_process(delta):
	if (!is_active):
		return;
	if(point_reached):
		generate_new_point();
	elif timer.is_stopped(): 
		go_to_point(delta);

func generate_new_point():
	var x = randf_range(-radius, radius);
	var y = randf_range(-radius, radius);
	point = Vector2(x+start_pos.x, y + start_pos.y);
	point_reached = false;

func go_to_point(delta):
	var dist = parent.position.distance_to(point);
	if !timer.is_stopped():
		return;
	if(dist < distance):
		timer.start(2);
	else:
		move_to(point);

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
	elif adjustedDifference < -0.1:
		parent.StartTurningCounterClockwise()
	else:
		parent.StopTurning()

func timeout():
	timer.stop();
	point_reached = true;

func exit():
	super.exit();
	point_reached = true;
