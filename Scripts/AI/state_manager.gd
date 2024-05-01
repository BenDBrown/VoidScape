@icon("res://Assets/Other/State.png")
extends Node2D
class_name StateManager;

signal state_changed;
@export var radius = 200;
var timer: Timer;
var visibility: Area2D;
var followState: Follow;
var patrolState: Patrol;
var idleState: Idle;
var parent: Node2D;

func _ready():
	create_timer();
	create_visibility_radius();
	init_states();
	idleState.enter();
	parent = get_parent();
	
func create_timer():
	timer = Timer.new();
	add_child(timer);

func create_visibility_radius():
	visibility = Area2D.new();
	var col = CollisionShape2D.new();
	var shape = CircleShape2D.new();
	col.debug_color = Color.RED;
	
	shape.radius = radius;
	col.shape = shape;
	visibility.add_child(col);
	add_child(visibility);
	visibility.body_entered.connect(_on_area_2d_body_entered)
	visibility.body_exited.connect(_on_area_2d_body_exited)

func init_states():
	var states = get_children();
	for state in states:
		if state is Patrol:
			patrolState = state;
		if state is Follow:
			followState = state;
		if state is Idle:
			idleState = state;
	state_changed.connect(followState.exit);
	state_changed.connect(patrolState.exit);
	state_changed.connect(idleState.exit);
	idleState.on_exit.connect(on_idle_exit);
	followState.on_exit.connect(on_follow_exit);

func _on_area_2d_body_entered(body):
	if body == parent: return;
	if !(body is CharacterBody2D): return;
	if !followState: return;
	state_changed.emit();
	followState.target = body;
	followState.enter();

func _on_area_2d_body_exited(body):
	if body == parent: return;
	if !(body is CharacterBody2D): return;
	followState.target = (body as Node2D).position;

func on_follow_exit():
	state_changed.emit();
	idleState.enter();
	
func on_idle_exit():
	state_changed.emit();
	patrolState.enter();

func _draw():
	draw_circle(position, radius, Color.SKY_BLUE);
