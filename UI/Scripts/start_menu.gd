extends Node2D

@export var open_space: PackedScene = preload("res://Scenes/open_space.tscn")
@export var start_cinematic: PackedScene = preload("res://Scenes/start_scene_story.tscn")

@onready var mainScreen = $"Main Screen/CanvasLayer"
@onready var settingsMenu = $"SettingsMenu"
@onready var continueButton = $"Main Screen/CanvasLayer/Continue"

func _ready():
	var hasSaveData = Game.LoadGame()
	if !hasSaveData:
		continueButton.disabled = true
		continueButton.focus_mode = Button.FOCUS_NONE


func _on_new_game_pressed():
	var spawnPointSave = SpawnPointSave.new()
	spawnPointSave.delete_save()
	get_tree().change_scene_to_file(start_cinematic.resource_path)


func _on_continue_pressed():
	get_tree().change_scene_to_file(open_space.resource_path)


func _on_settings_pressed():
	settingsMenu.show()
	mainScreen.hide()


func _on_quit_pressed():
	get_tree().quit()


func _on_settings_menu_menu_closed():
	mainScreen.show()

func _on_remove_save_pressed() -> void:
	var save = PlayerShipSave.new()
	save.delete_save()
