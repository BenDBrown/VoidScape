extends Node
class_name Combat
var raycast_scene = preload("res://Ship/AI/ai_raycast.tscn")
var ray:RayCast2D
var cast_vect := []
var area:Area2D
var in_area = false
var is_shooting = false
var exited = false
@export var detection_area_scene:PackedScene
@export_range(0, 300) var spreading:int 
@export_range(0, 300) var length:int 
@export var angle_cone_vision = deg_to_rad(30.0)
@export var max_view_distance = 800.0
@export var angle_between_rays = deg_to_rad(5.0)
var ship:Ship:
	set(value):
		value.add_child(self)
		ship = value
	get:
		return ship
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	add_detection_area()
	generate_sweeping_range()
	ray = raycast_scene.instantiate()
	add_child(ray)
	print("Combat Ready")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func is_in_raycast_sweep(target):
	for index in cast_vect:
		ray.set_target_position(index)
		ray.force_raycast_update()
		if ray.is_colliding() and ray.get_collider().get_parent() == target:
			return true
	return false
		

func combat_enganged(player):
	if!in_area:
		reset_raycast(ray)
	elif(in_area):
		var can_shoot = is_in_raycast_sweep(player)
		if can_shoot:
			shoot_target_in_range(player)

func generate_sweeping_range():
	ray = raycast_scene.instantiate()
	var coun_rays  := int(angle_cone_vision / angle_between_rays) + 1
	for index in coun_rays:
		var vects =( max_view_distance * Vector2.UP.rotated(angle_between_rays*(index -coun_rays/2.0)))
		cast_vect.append(vects)
	
func shoot_target_in_range(target): 
	if ray.is_colliding() and ray.get_collider().get_parent() == target:
		if(is_shooting):
			return false
		ship.StartShooting()
		is_shooting = true
		
	
	

func add_detection_area():
#This Func is to create the amount area's for detection we need to have the ship enough "eyes" to be able detect the player
	area = detection_area_scene.instantiate()
	area.name ="SHOOTING"
	area = create_area2D_with_signal_connections(area)
	add_child(area)

	

func create_area2D_with_signal_connections(_area):
#This func is to create and add it the parent so it is added the the NPC node with a position given in the parameters
	
	_area.area_entered.connect(on_area_entered)
	_area.area_exited.connect(on_area_exited)
	_area.monitorable = false
	return area

func on_area_exited(target:Area2D):
	if target.get_parent().get_parent() == ship:
		exited = true
		in_area = false
	else:
		exited= false
		ship.StopShooting()
		is_shooting = false

func on_area_entered(target:Area2D):
	#reating a method that has the ability to shoot the player on the detected location from the Area2D's that are part of the ship
	var par = target.get_parent()
	if par is ShipComponent:
		if(par == ship):
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


func reset_raycast(reycast:RayCast2D):
	reycast.set_target_position(Vector2.ZERO)
