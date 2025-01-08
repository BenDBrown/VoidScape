extends CharacterBody2D
class_name LootDrop

@onready var sprite = $Sprite2D
@onready var player_ship = Game.PlayerShip
@export var magnet_distance: float = 350
@export var magnet_speed: float = 100
var found_player = false
var cargo
var amount: int = 1
func set_sprite(newSprite):
	sprite.texture = newSprite

func _physics_process(delta: float) -> void:
	if !player_ship:
		return
	var dist = global_position.distance_to(player_ship.global_position)
	if dist > magnet_distance:
		return

	var dir = (player_ship as PlayerShip).global_position - global_position
	var col = move_and_collide(dir.normalized() * (magnet_speed * delta + dist/100))
	if col && col.get_collider() == player_ship:
		collect()


func collect():
	player_ship.CollectCargo(cargo)
	queue_free()
