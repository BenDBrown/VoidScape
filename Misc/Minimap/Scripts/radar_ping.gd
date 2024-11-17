extends Sprite2D

var disappear_timer = 0.0 # in seconds
var disappear_timer_max = 1.0 # in seconds
var color = Color(1,1,1,1)

func _process(delta: float):
	disappear_timer += delta
	
	color.a = lerp(1.0, 0.0 , disappear_timer/disappear_timer_max)
	modulate = color # Applying color to sprite
	
	# Destroy ping
	if disappear_timer >= disappear_timer_max:
		queue_free()
	
# Set ping color
func set_color(new_color: Color):
	color = new_color	
	modulate = color # Applying color to sprite
	
# Reset timer and set new max value
func set_disappear_timer(value: float):
	disappear_timer_max = value
	disappear_timer = 0.0
