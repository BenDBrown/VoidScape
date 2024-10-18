extends AudioStreamPlayer2D
class_name DeathAudioComponent

@export var from_position: float = 0
@export var play_on_exit_tree = true
@export var shift_pitch: bool = true
@export_range(0, 0.5) var pitch_range: float = 0.2


func _on_tree_exiting() -> void:
	if play_on_exit_tree:
		play()

func play_death_audio():
	reparent(get_tree().current_scene)
	finished.connect(queue_free)
	if shift_pitch:
		pitch_scale += randf_range(-pitch_range, pitch_range)
	play(from_position)
