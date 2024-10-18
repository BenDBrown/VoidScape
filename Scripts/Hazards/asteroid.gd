extends RigidBody2D

@onready var sprite = $Sprite2D
@onready var health_component = $HealthComponent
@onready var death_audio_component = $DeathAudioComponent
@onready var loot_drop_component = $LootDropComponent
@onready var collision_shape = $CollisionShape2D

@export var direction: Vector2
@export_enum("Random Direction", "Same Direction", "Stationary") var forceType = "Random Direction"
@export var asteroids : Array[CompressedTexture2D]
@export var destroyAsteroid : CompressedTexture2D
func _ready() -> void:
	var rnd = randi_range(0, asteroids.size()-1)
	sprite.texture = asteroids[rnd]
	randomize_force()

func randomize_force():
	match forceType:
		"Random Direction":
			apply_impulse(Vector2(randi_range(0, 30), randi_range(0,30)))
		"Same Direction":
			apply_impulse(direction)
		"Stationary":
			constant_force = Vector2.ZERO

func _on_body_entered(_body: Node) -> void:
	if _body.has_method("GetDamageInfo"):
		health_component.take_damage(_body.GetDamageInfo())

func _on_health_component_died() -> void:
	if get_child(-1) is Timer:
		return
	sprite.texture = destroyAsteroid
	var timer = Timer.new()
	add_child(timer)
	timer.timeout.connect(queue_free)
	collision_shape.set_deferred("disabled",true)
	death_audio_component.play_death_audio()
	loot_drop_component.drop_loot()
	timer.start(0.2)
