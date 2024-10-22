extends AIState

class_name Follow
@export var min_distance: float = 150
@export var max_distance: float = 300
@export var out_of_detection_distance: float = 400
@export_range(0, 300) var spreading:int 
@export_range(0, 300) var length:int 
@export var ray :=RayCast2D.new()
@export var angle_cone_vision = deg_to_rad(30.0)
@export var max_view_distance = 800.0
@export var angle_between_rays = deg_to_rad(5.0)
@export var detection_area_scene:PackedScene
var area:Area2D
var in_area = false
var shot = true
var pleyer: Ship = player
var cast_vect := []
var ammo = 5
var is_shooting = false
var exited = false

func enter():
	super.enter()
	add_detection_area()
	generate_sweeping_range()
	

func exit():
	super.exit()
	parent.StopTurning()
	parent.StopThrustingForward()
	transitioned.emit(self, "idle")
	get_parent().remove_child(area)

func physics_update(_delta):
	
	rotate_towards(player.global_position)
	var dist = parent.global_position.distance_to(player.global_position)
	if dist > out_of_detection_distance:
		exit()
	elif dist > max_distance:
		parent.StartThrustingForward()
	elif dist < min_distance:
		retreat()
	else:
		parent.StopThrustingForward()
	if !in_area:
		reset_raycast(ray)
	elif in_area:
		var can_shoot = is_in_raycast_sweep(player)
		if can_shoot:
			shoot_target_in_range(player)


func rotate_towards(globalPos: Vector2):
	var angle:float = Utils.get_angle(parent.global_position, globalPos, parent.global_rotation)
	if angle > 0.3:
		parent.StartTurningClockwise()
	elif angle < -0.3:
		parent.StartTurningCounterClockwise()
	else:
		parent.StopTurning()

func retreat():
	parent.StopTurning()
	parent.StartThrustingBackward()
	
func add_detection_area():
#This Func is to create the amount area's for detection we need to have the ship enough "eyes" to be able detect the player
	area = detection_area_scene.instantiate()
	get_parent().add_child(area)
	area = create_area2D_with_signal_connections(area)
	

func create_area2D_with_signal_connections(_area):
#This func is to create and add it the parent so it is added the the NPC node with a position given in the parameters
	
	_area.area_entered.connect(on_area_entered)
	_area.area_exited.connect(on_area_exited)
	_area.monitorable = false
	return area

func on_area_exited(target:Area2D):
	if target.get_parent().get_parent() == player:
		exited = true
		in_area = false
	else:
		exited= false
		parent.StopShooting()
		is_shooting = false

func on_area_entered(target:Area2D):
	#reating a method that has the ability to shoot the player on the detected location from the Area2D's that are part of the ship
	var par = target.get_parent()
	if par is ShipComponent:
		if(par.get_parent() == player):
			in_area = true

func create_detection_collider():
	var origin = Vector2(0,0)
	var x = spreading *-1
	var y= length *-1
	var top2
	var top
	top2 = Vector2(x * -1, y)
	top =Vector2(x,y)
	var collider = CollisionPolygon2D.new()
	var unpackedVectors =PackedVector2Array([origin,top,top2])
	var convexPolygon = ConvexPolygonShape2D.new()
	convexPolygon.set_point_cloud(unpackedVectors)
	collider.collider = convexPolygon.points
	return collider

func strafe_around_target(_target):
	pass
	#This Func should allow the NPC to move around the player while they are shooting the player
	#TODO

func is_in_raycast_sweep(target):
	for index in cast_vect:
		ray.set_target_position(index)
		ray.force_raycast_update()
		if ray.is_colliding() and ray.get_collider().get_parent() == target:
			return true
	return false
		

func generate_sweeping_range():
	var coun_rays  := int(angle_cone_vision / angle_between_rays) + 1
	for index in coun_rays:
		var vects =( max_view_distance * Vector2.UP.rotated(angle_between_rays*(index -coun_rays/2.0)))
		cast_vect.append(vects)
	
func shoot_target_in_range(target):
	if ray.is_colliding() and ray.get_collider().get_parent() == target:
		if(is_shooting):
			return
		parent.StartShooting()
		print(is_shooting)
		is_shooting = true
	
	



func reset_raycast(reycast:RayCast2D):
	reycast.set_target_position(Vector2(0,0))
