extends Area2D

func _on_body_entered(body: Node2D):
	destroy_colliding_object(body)

func _on_area_entered(area: Area2D) -> void:
	destroy_colliding_object(area)

func destroy_colliding_object(object: Node2D):
	print(object.name)
	object.get_parent().queue_free()
