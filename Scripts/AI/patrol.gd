extends AIState

class_name Patrol

@export var speed = 50;
@export var radius = 200.0;
var start_pos;
var point_reached = true;
var point: Vector2;
var timer: Timer;
var parent: Node2D;

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
	if(parent.position == point):
		timer.start(2);
	else:
		parent.position = parent.position.move_toward(point, delta * speed)

func timeout():
	timer.stop();
	point_reached = true;

func exit():
	super.exit();
	point_reached = true;
