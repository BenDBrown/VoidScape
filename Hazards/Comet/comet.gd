extends RigidBody2D
@onready var health_component = $HealthComponent
@export var min_distance: float = 500
@export var min_distance_linear_update = 50
@export_enum("Random Direction", "Player", "Same Direction") var forceType = "Player"
@export var speed = 1000

var playership: PlayerShip
var direction: Vector2
var death_timer
var last_target_location
var last_update = false
var vel_timer


func _ready() -> void:
	global_transform.origin = direction
	random_forcetype()

func _physics_process(_delta: float) -> void:
	direction = playership.global_position
	if forceType == "Player":
		check_distance_to_target_vector2(direction)
		velocity_timer(direction)

func randomize_force():
	match forceType:
		"Random Direction":
			linear_velocity = (Vector2(randf_range(-direction.x, direction.x), randf_range(-direction.y, direction.y)) - global_transform.origin) * speed
			create_death_timer(10)
		"Player":
			direction = playership.global_position
			linear_velocity = ((direction - global_transform.origin)).normalized() * speed
			print("Player_target")
		"Same Direction":
			apply_impulse(direction)
			create_death_timer(1)

func random_forcetype():
	var rng = RandomNumberGenerator.new()
	var rng_number = rng.randf_range(0, 10)

	if rng_number >= 5:
		forceType = "Player"
	else:
		forceType = "Random Direction"
	randomize_force()
func _on_health_component_died() -> void:
	create_death_timer(0.2)

func create_death_timer(time):
	if death_timer:
		return
	death_timer = Timer.new()
	death_timer.timeout.connect(queue_free)
	add_child(death_timer)
	death_timer.start(time)

func check_distance_to_target_vector2(target: Vector2):
	if transform.origin.distance_to(target) < min_distance:
		if !last_update:
			if last_target_location:
				create_death_timer(10)
				print("set last target location")
				last_update = true
			else:
				last_target_location = target
	last_target_location = target

func velocity_timer(target: Vector2):
	if vel_timer:
		if vel_timer.get_time_left() == 0:
			vel_timer.start(5)
		return
	vel_timer = Timer.new()
	vel_timer.timeout.connect(update_linear_velocity.bind(target))
	add_child(vel_timer)
	vel_timer.start(5)

func update_linear_velocity(target: Vector2):
	if !last_update:
		linear_velocity = ((target - global_transform.origin)).normalized() * speed


func _on_body_entered(_body: Node) -> void:
	create_death_timer(0.2)
