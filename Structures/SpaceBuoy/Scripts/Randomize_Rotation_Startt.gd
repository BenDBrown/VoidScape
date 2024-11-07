extends AnimationPlayer

const ANIMATION_NAME = "Rotate"

func _ready() -> void:
	
	var anim_length = self.get_animation(ANIMATION_NAME).length
	
	var sum_interval = int(anim_length/0.5)
	var random_interval = randi() % (sum_interval + 1) * 0.5
	
	self.play(ANIMATION_NAME, true)
	self.seek(random_interval)
	self.play()
	
	
