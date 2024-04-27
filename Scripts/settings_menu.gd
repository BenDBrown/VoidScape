extends Node2D

signal menu_closed()



func _on_return_pressed():
	menu_closed.emit();
	self.hide();
