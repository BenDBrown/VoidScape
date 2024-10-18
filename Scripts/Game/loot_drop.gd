extends CharacterBody2D
class_name LootDrop

@onready var sprite = $Sprite2D
@onready var player_ship = Game.player_ship
@export var magnet_distance: float = 350
@export var magnet_speed: float = 100
var found_player = false

var amount: int = 1:
	set(value):
		amount = value
	get:
		return amount

var cargo: Cargo:
	set(value):
		cargo = value
	get:
		return cargo

func set_sprite(newSprite):
	sprite.texture = newSprite

func _physics_process(delta: float) -> void:
	if player_ship is String:
		return
	var dist = global_position.distance_to(player_ship.global_position)
	if !found_player && dist > magnet_distance:
		return
	found_player = true
	var dir =(player_ship as PlayerShip).global_position - global_position
	var col = move_and_collide(dir.normalized() * (magnet_speed * delta + dist/100))
	if col && col.get_collider() == player_ship:
		collect()


func collect():
	#TODO: send info to the playership about collecting a material
	queue_free()
