extends Node2D
var credit
var interact_ui
var message_box
var format_string = "You have recieved %s credits" 
var text_label:RichTextLabel
var timer
@export var message_box_scene:PackedScene

func _ready() -> void:
	credit = get_parent().credit_reward
	dialog()
	give_credits()

func dialog():
	text_label = message_box_scene.instantiate()
	text_label.text= format_string % credit
	Game.Hud.add_child(text_label)
	death_timer()

func give_credits():
	Game.PlayerShip.AddCredits(credit)

func death_timer():
	if !timer:
		timer = Timer.new()
		timer.timeout.connect(queue_free)
		timer.timeout.connect(text_label.queue_free)
		add_child(timer)
	timer.start(5)
