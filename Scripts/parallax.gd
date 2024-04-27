extends ParallaxBackground

@export var scroll_speed = 5
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	scroll_offset.x -= scroll_speed * delta
