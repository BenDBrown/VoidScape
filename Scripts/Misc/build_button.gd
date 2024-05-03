extends Button

@onready var builder:ShipBuilder = $"../Builder"
func _on_pressed():
	builder.ShipBuildAttempt.connect(attempted)
	builder.BuildShip()

func attempted(result: bool):
	if result:
		var shipSaver = ShipSaver.new()
		var gridDict = builder.GetDict()
		for pos in gridDict.keys():
			shipSaver.add_component(pos,gridDict[pos])
		shipSaver.save()
