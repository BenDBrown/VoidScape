extends Control

@export var background: TextureRect
@export var line: TextureRect

@export_range (0.0, 5.0) var ping_fade_speed: float = 1.0 

@onready var radar_ping = preload("res://Misc/Minimap/Prefabs/radar_ping.tscn")

var radar_is_activated = false
var relative_minimap_scale = 0.0
var minimap_size = 100
var rotation_speed = 0.0

func configure_radar(radius: float, rotation_speed: float):
	self.rotation_speed = rotation_speed
	
	# Calculate the minimap scale relative to the radar radius on the player
	minimap_size = (background.size.x * scale.x) / 2.0   # Scale the background size to the parents node scale 
	relative_minimap_scale = minimap_size / radius

func create_ping(minimap_position: Vector2, ping_color: Color):
		var radarPingInstance = radar_ping.instantiate() #TODO: make it an object pool --> Look at disabling process after ping faded out.
		
		radarPingInstance.position = minimap_position
		radarPingInstance.set_color(ping_color)

		#TODO: Properly Scale the ping according to the radius of the radar
		#radarPingInstance.scale *= relative_minimap_scale
		
		# let the ping live for half a rotation
		radarPingInstance.set_disappear_timer(360.0/rotation_speed/ping_fade_speed) #TODO: make contstants
		
		return radarPingInstance

func _on_radar_enemy_detected(relative_position: Vector2, ping_color: Color = Color.WHITE): #TODO: make a switch case to detect color type
	if radar_is_activated:
		
		#Calculate the position of the ping on the minimap
		var minimapPosition = relative_position * relative_minimap_scale
		minimapPosition += global_position
		
		# Create ping
		ping_color = Color.RED # TODO: remove it later
		var radarPingInstance = create_ping(minimapPosition, ping_color)
		
		# Add to background as child
		add_child(radarPingInstance)

func _on_radar_radar_activated(radius: float, rotation_speed: float):
	configure_radar(radius, rotation_speed)
	radar_is_activated = true
	
func _on_radar_radar_sweeper_moved(rotation: float):
	line.rotation_degrees = rad_to_deg(rotation)
