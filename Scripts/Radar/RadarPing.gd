extends Sprite2D

var disappear_timer = 0.0 # in seconds
var disappear_timer_max = 1.0 # in seonds
var color = Color(1,1,1,1)

# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	#disappear_timer = 0.0
	#disappear_timer_max = 1.0
	#color = modulate # Applying color to sprite

# Called every frame. 'delta' is the elapsed time since the previous frame.
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
	
# Reset timer and set new mac value
func set_disappear_timer(value: float):
	print("new timer ", value)
	disappear_timer_max = value
	print("new timer final ", disappear_timer_max)
	disappear_timer = 0.0
