extends Node
class_name Comet_tracker
signal not_allowed_to_spawn
var comets = [] #if at a later stage we want to have this be multiple comets
var playership: PlayerShip
var comet_scene:PackedScene = preload("res://Hazards/Comet/Comet.tscn")
var comet
var comet_location
var spawn_location
var spawn_timer

@export var allowed_to_spawn = false
@export var extra_distance = 5000

func _ready():
	playership = Game.PlayerShip
	
	
func _process(delta: float) -> void:
	if allowed_to_spawn:
		spawn_timer_reset()
	

func spawn_comet():
	if comet:
		return
	spawn_location_calculation()
	comet = comet_scene.instantiate()
	comet.playership = playership
	comet.direction = spawn_location
	add_child(comet)
	
func  spawn_timer_reset():
	if spawn_timer :
		if spawn_timer.get_time_left() == 0:
			spawn_timer.start(random_spawn_time())
		return
	spawn_timer = Timer.new()
	spawn_timer.timeout.connect(spawn_comet)
	add_child(spawn_timer)
	spawn_timer.start(random_spawn_time())

func spawn_location_calculation():
	pass
	var direction:Vector2
	direction.x = playership.global_position.x + extra_distance
	direction.y = playership.global_position.y + extra_distance
	spawn_location = Vector2(randf_range(-direction.x, direction.x), randf_range(-direction.y, direction.y))
func set_allowed_to_spawn(value:String):
	if value == "true":
		allowed_to_spawn = true
		print("Allowed is true")
	else:
		allowed_to_spawn = false
		print("Allowed is false")

func random_spawn_time() -> float :
	var rng = RandomNumberGenerator.new()
	var spawn_time = rng.randf_range(60,180)
	return spawn_time
