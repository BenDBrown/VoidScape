extends Node2D

@onready var mainScreen = get_node("Main Screen");
@onready var settingsMenu = get_node("SettingsMenu");

func _ready():
    var hasSaveData = Utils.load_game();
    if(!hasSaveData):
        var continueButton = get_node("Main Screen/Continue");
        continueButton.disabled = true;
        continueButton.focus_mode = Button.FOCUS_NONE;

func _on_new_game_pressed():
    get_tree().change_scene_to_file("res://Scenes/start_scene.tscn")

func _on_continue_pressed():
    get_tree().change_scene_to_file("res://Scenes/continue.tscn")

func _on_settings_pressed():
    settingsMenu.show();
    mainScreen.hide();

func _on_quit_pressed():
    get_tree().quit();


func _on_settings_menu_menu_closed():
    mainScreen.show();
