extends Area2D

signal enemy_detected(relative_position: Vector2)

@onready var sweeper: CollisionShape2D = $Sweeper

@export var sweeper_rotation_speed = 180;
@export var radar_radius =  1000.0

@export var use_raycast: bool = false
@onready var ray_cast_2d: RayCast2D = $RayCast2D
var collider_list: Array[Area2D] = []

func _ready():
	set_radar_radius(radar_radius)	# set new radar radius

func _process(delta: float):
	if use_raycast:
		ray_cast_2d.rotation_degrees += sweeper_rotation_speed * delta
		
		if(ray_cast_2d.rotation_degrees >= 360):
			ray_cast_2d.rotation = 0
			collider_list.clear()
	else:
		sweeper.rotation_degrees += sweeper_rotation_speed * delta
		
		# after this emit signal --> rotation
		
		if(sweeper.rotation_degrees >= 360):
			sweeper.rotation = 0

func _on_area_entered(area: Area2D):
	
	if use_raycast:
		return
	
	if area.is_in_group("enemy"): # make it a constant perhaps	
		#print("Enemy detected")
		
		# Suggestion Genelle: pascalCase for local variables and global snake_case --> Put it into manifesto
		var relativePosition = area.global_position - global_position # to local method see playership. Node2D.tolocal
		emit_signal("enemy_detected", relativePosition)

func _physics_process(delta: float):
	if not use_raycast:
		return
	
	if ray_cast_2d.is_colliding():
		
		var collider = ray_cast_2d.get_collider()
		
		if(collider not in collider_list && collider.is_in_group("enemy")):
			collider_list.append(collider)
			
			var relative_position = ray_cast_2d.get_collision_point() - global_position
			emit_signal("enemy_detected", relative_position)


func get_sweeper_rotation() -> float:
	if use_raycast:
		return ray_cast_2d.rotation
	
	return sweeper.rotation
	
func set_radar_radius(radius: float):
	
	if use_raycast:
		ray_cast_2d.target_position = Vector2(radius,0)
		return
	
	var new_radius = Vector2(radius, 0)
	sweeper.shape.set_b(new_radius)
	
func get_radar_radius() -> float:
	if use_raycast:
		return ray_cast_2d.target_position.x
	
	return sweeper.shape.get_b().x
	
func get_rotation_speed() -> float:
	return sweeper_rotation_speed
	
#func _on_body_entered(body: Node2D):
	#print("Enemy Detected: ", body.name)
