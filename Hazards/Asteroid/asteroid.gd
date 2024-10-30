extends RigidBody2D

@onready var sprite = $Sprite2D
@onready var health_component = $HealthComponent
@onready var death_audio_component = $DeathAudioComponent
@onready var loot_drop_component = $LootDropComponent
@onready var hitbox_component = $HitboxComponent

@export var direction: Vector2
@export_enum("Random Direction", "Same Direction", "Stationary") var forceType = "Random Direction"
@export var asteroids: Array[CompressedTexture2D]
@export var destroyAsteroid: CompressedTexture2D

var death_timer

func _ready() -> void:
	var rnd = randi_range(0, asteroids.size() - 1)
	sprite.texture = asteroids[rnd]
	randomize_force()

func randomize_force():
	match forceType:
		"Random Direction":
			apply_impulse(Vector2(randf_range(-direction.x, direction.x), randf_range(-direction.y, direction.y)))
		"Same Direction":
			apply_impulse(direction)
		"Stationary":
			constant_force = Vector2.ZERO
			
func _on_health_component_died() -> void:
	if death_timer:
		return
	sprite.texture = destroyAsteroid
	death_timer = Timer.new()
	add_child(death_timer)
	death_timer.timeout.connect(queue_free)
	hitbox_component.set_deferred("disabled", true)
	death_audio_component.play_death_audio()
	loot_drop_component.drop_loot()
	death_timer.start(0.2)