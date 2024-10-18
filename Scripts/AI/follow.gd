extends AIState

class_name Follow
@export var min_distance: float = 100
@export var max_distance: float = 150
@export var out_of_detection_distance: float = 300
@export_range(0, 300) var spreading:int 
@export_range(0, 300) var length:int 
@export var ray :=RayCast2D.new()
@export var angle_cone_vision = deg_to_rad(30.0)
@export var max_view_distance = 800.0
@export var angle_between_rays = deg_to_rad(5.0)
var area:Area2D
var in_area = false
var shot = true
var pleyer: PlayerShip = player
var cast_vect := []
var ammo = 5
var is_shooting = false
var exited = false

func enter():
	super.enter()
	addDetectionArea()
	generateSweepingRange()

func exit():
	super.exit()
	parent.StopTurning()
	parent.StopThrustingForward()
	transitioned.emit(self, "idle")
	parent.remove_child(area)

func physics_update(_delta):
	var dist = parent.global_position.distance_to(player.global_position)
	if dist > out_of_detection_distance:
		exit()
	elif dist > max_distance:
		parent.StartThrustingForward()
		rotate(player.global_position)
	elif dist < min_distance:
		retreat()
	else:
		parent.StopThrustingForward()
		parent.StopTurning()
	if !in_area:
		resetRayCast(ray)
	elif in_area:
		var can_shoot = sweepingRaycast()
		if can_shoot:
			shootPlayerInRacastRange()


func rotate(globalPos: Vector2):
	var angle:float = Utils.get_angle(parent.global_position, globalPos, parent.global_rotation)
	if angle > 0.1:
		parent.StartTurningClockwise()
		pass
	elif angle < -0.1:
		pass
		parent.StartTurningCounterClockwise()
	else:
		parent.StopTurning()

func retreat():
	parent.StopTurning()
	parent.StopThrustingBackward()
	
func addDetectionArea():
#This Func is to create the amount area's for detection we need to have the ship enough "eyes" to be able detect the player
	
	area = createArea2DWithConnections(area)
	var polygon = createPolygonCollsion2D(area)
	area.add_child(polygon)
	parent.add_child(area)

func createArea2DWithPosition(area,x,y):
#This func is to create and add it the parent so it is added the the NPC node with a position given in the parameters
	area = Area2D.new()
	var shape = CircleShape2D.new()
	shape.set_radius(50)
	var collision = CollisionShape2D.new()
	collision.set_shape(shape)
	area.add_child(collision)
	parent.add_child(area)
	area.position +=Vector2(x,y)
	return	area

#func convertSpread():
	#pass
	#var dot:int
	#dot = spreading
	#dot *= -1
	#return dot
func createArea2DWithConnections(area):
#This func is to create and add it the parent so it is added the the NPC node with a position given in the parameters
	area = Area2D.new()
	area.area_entered.connect(detectingPlayerInArea)
	area.area_exited.connect(AreaExit)
	return area

func AreaExit(target:Area2D):
	pass
	if target.get_parent() == player:
		exited = true
		in_area = false
	else:
		exited= false
		parent.StopShooting()
		is_shooting = false

func detectingPlayerInArea(target:Area2D):
	#reating a method that has the ability to shoot the player on the detected location from the Area2D's that are part of the ship
	if target.get_parent() == player:
		in_area = true

func createPolygonCollsion2D(area:Area2D):
	var origin = Vector2(0,0)
	var x = spreading *-1
	var y= length *-1
	var top2
	var top
	top2 = Vector2(x * -1, y)
	top =Vector2(x,y)
	var polygon = CollisionPolygon2D.new()
	var unpackedVectors =PackedVector2Array([origin,top,top2])
	var convexPolygon = ConvexPolygonShape2D.new()
	convexPolygon.set_point_cloud(unpackedVectors)
	polygon.polygon = convexPolygon.points
	return polygon




func rotatingInTheDirectionOfDetectedPlayer():
	pass
	#TODO
	#This func should rotate the ship to the direction the player is when they were detected
func moveAroundPlayerWhenShooting():
	pass
	#This Func should allow the NPC to move around the player while they are shooting the player
	#TODO

func sweepingRaycast():
	for index in cast_vect:
		ray.set_target_position(index)
		ray.force_raycast_update()
		if ray.is_colliding() and ray.get_collider() is PlayerShip:
			return true
	return false
		

func generateSweepingRange():
	var calc_vects := []
	var coun_rays  := int(angle_cone_vision / angle_between_rays) + 1
	for index in coun_rays:
		var vects =( max_view_distance * Vector2.UP.rotated(angle_between_rays*(index -coun_rays/2.0)))
		cast_vect.append(vects)
	
func shootPlayerInRacastRange():
	if ray.is_colliding() and ray.get_collider() is PlayerShip:
		if(is_shooting):
			return
		parent.StartShooting()
		is_shooting = true
	
	



func resetRayCast(reycast:RayCast2D):
	reycast.set_target_position(Vector2(0,0))
