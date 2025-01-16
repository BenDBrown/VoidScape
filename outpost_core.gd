extends Node2D
signal core_destroyed
@onready var death_anim = $BulletDeathAnimation
@onready var hitbox =$HitboxComponent
@onready var sprite = $Sprite2D
@export var message_scene:PackedScene
var message:RichTextLabel
var death_timer:Timer
var died = false

func _on_health_component_died() -> void:
	if !died:
		died = true
		emit_signal("core_destroyed")
		sprite.visible = false
		hitbox.queue_free()
		death_anim.visible = true
		death_anim.play()
		add_destruction_message()

func add_destruction_message():
	message = message_scene.instantiate()
	message.text = "The core has been Destroyed, outpost losing power"
	Game.Hud.add_child(message)
	create_death_timer()

func create_death_timer():
	if death_timer:
		return
	death_timer = Timer.new()
	death_timer.timeout.connect(message.queue_free)
	death_timer.timeout.connect(death_timer.queue_free)
	add_child(death_timer)
	death_timer.start(3)
