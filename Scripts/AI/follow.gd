extends AIState

class_name Follow

var target;
var speed = 100;
var parent: Node2D;
var distance = 200;

func _ready():
	parent = get_parent().get_parent();

func _physics_process(delta):
	if is_active:
		follow_target(delta);

func follow_target(delta):
	if !target: return;
	var pos = target;
	if target is Node2D: 
		pos = target.position;
		if parent.position.distance_to(pos) > distance:
			parent.position = parent.position.move_toward(pos, delta * speed)
		elif parent.position.distance_to(pos) <= distance/2:
			parent.position = parent.position.move_toward(-pos, delta * speed);
			
	elif target is Vector2:
		parent.position = parent.position.move_toward(pos, delta * speed)
		if parent.position == target:
			exit();
