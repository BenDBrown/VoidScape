extends Area2D

func _on_body_entered(body: Node2D):
	DestroyCollidingObject(body)

func _on_area_entered(area: Area2D) -> void:
	DestroyCollidingObject(area)

func DestroyCollidingObject(object: Node2D):
	print(object.name)
	object.get_parent().queue_free()
