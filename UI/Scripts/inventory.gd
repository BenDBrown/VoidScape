extends CanvasLayer

@onready var list: ItemList = $ItemList
var player

func _ready() -> void:
	player = Game.PlayerShip

func _process(delta: float) -> void:
	if Input.is_action_just_pressed("inventory"):
		populate_list()
		if visible:
			hide()
		else:
			show()

func populate_list():
	list.clear()
	var cargos = player.GetCargos() as Dictionary
	for key in cargos.keys():
		list.add_item(str(cargos[key])+"x"+" "+key.resource_path.get_file().trim_suffix('.tres') )
