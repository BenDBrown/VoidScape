extends Area2D
# Gravitional pull simulated using Newtons Law of Gravity

# Gravitional Constant - Strength of Gravity
const GRAVITY_CONSTANT: float = 500.0

# Mass of Star
@export var mass: float = 1000.0

# TEMPORARY Mass of objects being pulled
@export var objectMass: float = 10.0

@export var gravity_on: bool = false

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float):	
	
	if gravity_on:
		for body in get_overlapping_bodies():
			if body is RigidBody2D:
				applyGravity(body)

		
		#if body is ShipComponent:
			#applyGravity(body)	
	
func apply_gravity(body: Node2D) -> void:
	
	# Direction vector: object to star
	var direction = global_position - body.global_position
	var distance = direction.length()

	# Can't divide by 0	
	if distance == 0:
		return
		
	direction = direction.normalized()
	
	#var force = (G * mass * objectMass) / (distance * distance)
	var force = (GRAVITY_CONSTANT * mass * body.mass) / (distance * distance)
	
	# Apply the force as an impulse (which makes sense for a gravitational force)
	body.apply_central_impulse(force)
