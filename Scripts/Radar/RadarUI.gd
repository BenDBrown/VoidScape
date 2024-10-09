extends Control

var area2d_radar

@onready var line: TextureRect = $Line
@onready var radar_ping = preload("res://Prefabs/Radar/CollisionShape/radar_ping.tscn")
@onready var background: TextureRect = $Background

var minimap_scale = 0.0
var radar_radius = 0
var minimap_size = 100

func _ready():
	area2d_radar = get_node("../Radar")
	
	if area2d_radar:
		radar_radius = area2d_radar.get_radar_radius()
	
	# Calculate the minimap scale relative to the radar radius on the player
	minimap_size = (background.size.x * scale.x) / 2.0   # Scale the background size to the parents node scale 
	minimap_scale = minimap_size / radar_radius
	print(minimap_scale)

func _process(delta: float):
	if area2d_radar:
		line.rotation_degrees = rad_to_deg(area2d_radar.get_sweeper_rotation())

func _on_radar_enemy_detected(relative_position: Vector2):
	#Calculate the position of the ping on the minimap
	var minimap_position = relative_position * minimap_scale
	minimap_position += global_position
	
	var radar_ping_instance = radar_ping.instantiate()
	radar_ping_instance.position = minimap_position
	radar_ping_instance.set_color(Color.RED)
	
	radar_ping_instance.scale *= minimap_scale
	
	# let the ping live for half a rotation
	radar_ping_instance.set_disappear_timer(360.0/area2d_radar.get_rotation_speed()/2)
	
	# Add to background as child
	add_child(radar_ping_instance)
