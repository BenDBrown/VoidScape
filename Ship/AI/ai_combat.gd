extends Node2D
class_name Combat

@export var detection_area_scene: PackedScene = preload("res://Ship/AI/detection_area.tscn")
@export_range(0, 300) var spreading: int = 300
@export_range(0, 300) var length: int = 300
@export var angle_cone_vision = deg_to_rad(30.0)
@export var max_view_distance = 800.0
@export var angle_between_rays = deg_to_rad(5.0)

var raycast_scene = preload("res://Ship/AI/ai_raycast.tscn")
var ray: RayCast2D
var cast_vect
var detection_cone: Area2D
var in_area = false
var exited = false
var player
var ship: Ship

func enter(parent):
	print("Combat_Entered")
	ship = parent
	ship.add_child(self)
	if !detection_cone:
		detection_cone = create_detection_cone()
	if !cast_vect:
		cast_vect = create_sweeping_range()
	if !ray:
		ray = create_ray()
		

func exit():
	pass

func physics_update(target):
	player = target
	if !in_area:
		ray.set_target_position(Vector2.ZERO)
		return

	if is_in_detection_cone(player):
		attack(player)

func is_in_detection_cone(target):
	for index in cast_vect:
		ray.set_target_position(index)
		ray.force_raycast_update()
		if target != null && ray.is_colliding() && ray.get_collider() == target:
			return true
	return false

func create_sweeping_range():
	var output = []
	var coun_rays = int(angle_cone_vision / angle_between_rays) + 1
	for index in coun_rays:
		var vects = (max_view_distance * Vector2.UP.rotated(angle_between_rays * (index - coun_rays / 2.0)))
		output.append(vects)
	return output

func create_ray():
	var r = raycast_scene.instantiate()
	add_child(r)
	r.add_exception(ship)
	for c in ship.get_children():
		if c is CollisionObject2D:
			r.add_exception(c)
	return r

func attack(target):
	if ray.is_colliding() and ray.get_collider() == target:
		ship.StartShooting()

func create_detection_cone():
	var cone = detection_area_scene.instantiate() as Area2D
	add_child(cone)
	cone.global_position = ship.global_position
	cone.name = "eyes_for_guns"
	cone.area_entered.connect(on_area_entered)
	cone.area_exited.connect(on_area_exited)
	cone.monitorable = false
	return cone

func on_area_exited(target: Area2D):
	if !is_in_detection_cone(target.get_parent()):
		ship.StopShooting()
		exited = true
		
		
#creating a method that has the ability to attack the player on the detected location from the Area2D's that are part of the ship
func on_area_entered(target: Area2D):
	var par = target.get_parent()
	if par is ShipComponent:
		if (par.get_parent() == player):
			in_area = true

func create_detection_collider():
	var origin = Vector2(0, 0)
	var x = spreading * -1
	var y = length * -1
	var top2
	var top
	top2 = Vector2(x * -1, y)
	top = Vector2(x, y)
	var collider = CollisionPolygon2D.new()
	var unpackedVectors = PackedVector2Array([origin, top, top2])
	var convexPolygon = ConvexPolygonShape2D.new()
	convexPolygon.set_point_cloud(unpackedVectors)
	collider.collider = convexPolygon.points
	return collider
