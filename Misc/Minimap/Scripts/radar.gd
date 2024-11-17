extends Area2D

signal enemy_detected(relative_position: Vector2)
signal radar_activated(radius:float, rotation_speed: float)
signal radar_sweeper_moved(rotation: float)

@onready var sweeper: CollisionShape2D = $Sweeper

@export var sweeper_rotation_speed = 180;
@export var radar_radius =  1000.0

func _ready():
	set_radar_radius(radar_radius)	# set new radar radius
	radar_activated.emit(get_radar_radius(), get_rotation_speed())
	
	position = Vector2(0,0)

func _process(delta: float):
	rotation = -get_parent().global_rotation # Inverse the ships rotation
	
	sweeper.rotation_degrees += sweeper_rotation_speed * delta
		
	if(sweeper.rotation_degrees >= 360):
		sweeper.rotation = 0
		
	#TODO: ONLY EMIT SIGNAL AFTER A SET AMOUNT OF FRAMES PASSED
	radar_sweeper_moved.emit(sweeper.rotation)

func _on_area_entered(area: Area2D):
	if area.is_in_group("enemy"):
		
		var relativePosition = area.global_position - global_position 
		emit_signal("enemy_detected", relativePosition)
	
func set_radar_radius(radius: float):
	var newRadius = Vector2(radius, 0)
	sweeper.shape.set_b(newRadius)
	
func get_radar_radius() -> float:
	return sweeper.shape.get_b().x
	
func get_rotation_speed() -> float:
	return sweeper_rotation_speed
