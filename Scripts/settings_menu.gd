extends Node2D

signal menu_closed()
@onready var volume_slider: HSlider = $"Volume Component/Slider"
@onready var volume_value_label: Label = $"Volume Component/Value"
@onready var screen_sizes: OptionButton = $"Window Mode/Options"
@onready var audio_player: AudioStreamPlayer2D = $AudioStreamPlayer2D
var settings_manager: SettingsManager;
var init_volume
var init_window_mode

func _ready():
    settings_manager = SettingsManager.load_settings()
    init_volume = settings_manager.volume
    init_window_mode = settings_manager.window_mode

    volume_slider.min_value = 0
    volume_slider.max_value = 100
    volume_slider.value = init_volume
    volume_value_label.text = str(init_volume)
    screen_sizes.select(screen_sizes.get_item_index(settings_manager.window_mode))
    settings_manager.set_window_mode(init_window_mode)

func _physics_process(delta):
    settings_manager.set_master_volume(volume_slider.value)
    volume_value_label.text = str(volume_slider.value)

func reset_settings():
    volume_slider.value = init_volume
    settings_manager.set_master_volume(init_volume)
    settings_manager.set_window_mode(init_window_mode)
    screen_sizes.select(screen_sizes.get_item_index(init_window_mode))

func _on_audio_stream_player_2d_finished():
    audio_player.play()

func _on_return_pressed():
    reset_settings()
    menu_closed.emit()
    self.hide()

func _on_save_pressed():
    settings_manager.save_settings()
    init_volume = settings_manager.volume
    init_window_mode = settings_manager.window_mode
    _on_return_pressed()


func _on_screen_sizes_item_selected(index):
    var id = screen_sizes.get_item_id(index)
    settings_manager.set_window_mode(id)
