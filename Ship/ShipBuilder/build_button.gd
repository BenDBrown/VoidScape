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

func on_build_attempted(isSuccessful: bool):
	if isSuccessful:
		var shipSaver = PlayerShipSave.new()
		shipSaver = shipSaver.load_save()
		shipSaver.add_components(builder.GetDict())
		shipSaver.save()
	builder.ShipBuildAttempt.disconnect(on_build_attempted)
