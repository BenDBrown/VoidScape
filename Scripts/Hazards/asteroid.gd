extends RigidBody2D

@onready var sprite = $Sprite2D
@export var health : int = 3
@export var direction: Vector2
@export_enum("Random Direction", "Same Direction", "Stationary") var forceType = "Random Direction"
@export var asteroids : Array[CompressedTexture2D]
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
	health-=1
	if health <= 0:
		queue_free()
