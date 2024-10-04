extends Node2D

class_name RadarUI

@onready var sweeper: Sprite2D = $Sweeper

@export var rotation_speed = 180;
@export var radar_distance = 150;

@export var raycast_distance_x = 200;
@export var raycast_distance_y = 200;


@onready var ray_cast_2d: RayCast2D = $Sweeper/RayCast2D

var collider_list: Array[Area2D] = []

# Called when the node enters the scene tree for the first time.
func _ready():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float):
	sweeper.rotation_degrees += rotation_speed * delta
	
	if(sweeper.rotation_degrees >= 360):
		sweeper.rotation = 0
		collider_list.clear() # clear list to get new 
		
func _physics_process(delta: float):
	
	if ray_cast_2d.is_colliding():
		var collider = ray_cast_2d.get_collider()
		
		var parent_node = collider.get_parent()
		if(collider not in collider_list && parent_node is Detectable):
			print("The simple one is colliding and collider: ", collider, " and added")
			collider_list.append(collider)
	
