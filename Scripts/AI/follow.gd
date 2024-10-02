extends AIState

class_name Follow
@export var min_distance: float = 100
@export var max_distance: float = 150
@export var out_of_detection_distance: float = 300
@export var vector_y:float = -50
@export var vector_x:float = 0

func enter():
	super.enter()
	addDetectionArea()
	
func exit():
	super.exit()
	parent.StopShooting()
	parent.StopTurning()
	parent.StopThrusting()
	transitioned.emit(self, "idle")

func physics_update(_delta):
	var dist = parent.global_position.distance_to(player.global_position)
	if dist > out_of_detection_distance:
		exit()
	elif dist > max_distance:
		parent.ForwardThrust()
		rotate(player.global_position)
	elif dist < min_distance:
		retreat()
	else:
		parent.StopThrusting()
		parent.StopTurning()

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
	parent.BackThrust()
	
func addDetectionArea():
#This Func is to create the amount area's for detection we need to have the ship enough "eyes" to be able detect the player
	var area
	createArea2D(area,vector_x,vector_y)
	var arealeft 
	createArea2D(arealeft,vector_y,vector_x)
	var arearight
	createArea2D(arearight,50,0)
	print("Area Added")

func createArea2D(area,x,y):
#This func is to create and add it the parent so it is added the the NPC node with a position given in the parameters
	area = Area2D.new()
	var shape = CircleShape2D.new()
	shape.set_radius(50)
	var collision = CollisionShape2D.new()
	collision.set_shape(shape)
	area.add_child(collision)
	parent.add_child(area)
	area.position +=Vector2(x,y)
	
func shootingWhenPlayerEntersTheArea2D():
	pass
	#TODO
	#Creating a method that has the ability to shoot the player on the detected location from the Area2D's that are part of the ship

func rotatingInTheDirectionOfDetectedPlayer():
	pass
	#TODO
	#This func should rotate the ship to the direction the player is when they were detected
func moveAroundPlayerWhenShooting():
	pass
	#This Func should allow the NPC to move around the player while they are shooting the player
	#TODO
