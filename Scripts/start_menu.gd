extends Node

@onready var mainScreen = get_node("Main Screen");
@onready var settingsMenu = get_node("SettingsMenu");

func _on_new_game_pressed():
	pass

func _on_continue_pressed():
	pass

func _on_settings_pressed():
	settingsMenu.show();
	mainScreen.hide();

func _on_quit_pressed():
	get_tree().quit();


func _on_settings_menu_menu_closed():
	mainScreen.show();
