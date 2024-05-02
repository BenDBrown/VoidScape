extends AIState

class_name Idle

@export var wait_time: float = 3.0;

var timer:Timer;

func _ready():
	timer = Timer.new();
	timer.timeout.connect(exit);
	add_child(timer);

func enter():
	super.enter();
	timer.start(wait_time);

func exit():
	super.exit();
	timer.stop();
