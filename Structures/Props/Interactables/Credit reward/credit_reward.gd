extends Node2D
var credit
var interact_ui
var message_box
var format_string = "You have recieved %s credits" 
var text_label:RichTextLabel
@export var message_box_scene:PackedScene

func _ready() -> void:
	credit = get_parent().credit_reward
	dialog()
	give_credits()

func dialog():
	find_hud()
	message_box = message_box_scene.instantiate()
	text_label = message_box.get_child(0)
	text_label.text= format_string % credit
	interact_ui.add_child(message_box)
	var timer = Timer.new()
	timer.timeout.connect(queue_free)
	timer.timeout.connect(message_box.queue_free)
	add_child(timer)
	timer.start(5)

func give_credits():
	Game.PlayerShip.AddCredits(credit)

func find_hud():
	for c in get_parent().get_parent().get_children():
		if c is CanvasLayer:
			if c.name == "HUD":
				for ui in c.get_children():
					if ui is Interactable_ui:
						interact_ui = ui
