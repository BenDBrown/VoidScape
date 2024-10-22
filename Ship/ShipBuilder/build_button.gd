extends Button

var builder: ShipBuilder

func _ready() -> void:
	if get_parent() is ShipBuilder:
		builder = get_parent() as ShipBuilder
	else:
		printerr("Parent is not shipbuilder")

func _on_pressed():
	builder.ShipBuildAttempt.connect(on_build_attempted)
	builder.BuildShip()

func on_build_attempted(result: bool):
	if result:
		var shipSaver = PlayerShipSave.new()
		var gridDict = builder.GetDict()
		for pos in gridDict.keys():
			shipSaver.add_component(pos, gridDict[pos])
		shipSaver.save()
