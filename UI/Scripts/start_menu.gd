extends Node2D

@export var start_scene: PackedScene = preload("res://Scenes/start_scene.tscn")
@export var continue_scene: PackedScene = preload("res://Scenes/continue.tscn")
@export var ship_builder_scene: PackedScene = preload("res://Ship/ShipBuilder/ship_builder.tscn")
@export var test_scene: PackedScene = preload("res://Scenes/playtest.tscn")
@export var worm_scene: PackedScene = preload("res://Hazards/Worm/WormDemo.tscn")
@export var cave_scene: PackedScene = preload("res://Scenes/cave_demo.tscn")

@onready var mainScreen = $"Main Screen"
@onready var settingsMenu = $"SettingsMenu"
@onready var continueButton = $"Main Screen/CanvasLayer/Continue"

func _ready():
	var hasSaveData = Game.LoadGame()
	if !hasSaveData:
		continueButton.disabled = true
		continueButton.focus_mode = Button.FOCUS_NONE


func _on_new_game_pressed():
	get_tree().change_scene_to_file(start_scene.resource_path)


func _on_continue_pressed():
	get_tree().change_scene_to_file(continue_scene.resource_path)


func _on_settings_pressed():
	settingsMenu.show()
	mainScreen.hide()


func _on_quit_pressed():
	get_tree().quit()


func _on_settings_menu_menu_closed():
	mainScreen.show()


func _on_ship_builder_pressed() -> void:
	get_tree().change_scene_to_file(ship_builder_scene.resource_path)

func _on_worm_pressed() -> void:
	get_tree().change_scene_to_file(worm_scene.resource_path)
	
func _on_ismet_pressed() -> void:
	get_tree().change_scene_to_file(cave_scene.resource_path)

func _on_test_menu_pressed() -> void:
	get_tree().change_scene_to_file(test_scene.resource_path)

func _on_remove_save_pressed() -> void:
	var save = PlayerShipSave.new()
	var result = save.delete_save()
