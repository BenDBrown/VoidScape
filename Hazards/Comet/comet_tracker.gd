extends Node
class_name Comet_tracker
signal not_allowed_to_spawn
var comets = [] # if at a later stage we want to have this be multiple comets
var playership: PlayerShip
var comet_scene: PackedScene = preload("res://Hazards/Comet/Comet.tscn")
var comet
var comet_location
var spawn_location
var spawn_timer

@export var spawn_time_min = 60
@export var spawn_time_max = 180

@export var allowed_to_spawn = false
@export var extra_distance = 5000

func _ready():
	set_deferred("playership", Game.PlayerShip)
	spawn_timer_reset()
	
		
func spawn_comet():
	spawn_timer.start(random_spawn_time())
	if !allowed_to_spawn:
		return
	if comet:
		return
	spawn_location_calculation()
	comet = comet_scene.instantiate()
	comet.playership = playership
	comet.direction = spawn_location
	add_child(comet)
	comet.tree_exited.connect(on_comet_exit)
	
func spawn_timer_reset():
	if spawn_timer:
		return
	spawn_timer = Timer.new()
	spawn_timer.timeout.connect(spawn_comet)
	add_child(spawn_timer)
	spawn_timer.start(random_spawn_time())

func spawn_location_calculation():
	var direction: Vector2
	direction.x = playership.global_position.x + extra_distance
	direction.y = playership.global_position.y + extra_distance
	spawn_location = Vector2(randf_range(-direction.x, direction.x), randf_range(-direction.y, direction.y))

func set_allowed_to_spawn(value: String):
	if value == "true":
		allowed_to_spawn = true
		print("Allowed is true")
	else:
		allowed_to_spawn = false
		print("Allowed is false")

func random_spawn_time() -> float:
	var rng = RandomNumberGenerator.new()
	var spawn_time = rng.randf_range(spawn_time_min, spawn_time_max)
	return spawn_time

func on_comet_exit():
	comet = null