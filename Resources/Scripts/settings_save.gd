extends Saveable
class_name SettingsSave

const NAME = "Settings.tres"
const MIN_DB = -30
const MAX_DB = 0
@export var volume: int = 12
@export var mute: bool = false
@export var window_mode = DisplayServer.WINDOW_MODE_WINDOWED


func set_master_volume(newVolume):
	volume = newVolume
	mute = newVolume == 0
	AudioServer.set_bus_mute(0, mute)
	var dbVolume = remap(newVolume, 0, 100, MIN_DB, MAX_DB)
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Master"), dbVolume)

func set_window_mode(id):
	if id == DisplayServer.WINDOW_MODE_WINDOWED:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
	elif id == DisplayServer.WINDOW_MODE_FULLSCREEN:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
	else: return
	window_mode = id

func get_save_name():
	return NAME
