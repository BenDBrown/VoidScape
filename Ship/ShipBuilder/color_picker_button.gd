extends ColorPickerButton

@onready var item_lister: ItemLister = $"../ItemLister"

func _on_color_changed(color: Color) -> void:
	item_lister.change_color(color)
