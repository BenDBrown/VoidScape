extends Node

var player_ship_scene = preload("res://Prefabs/PlayerShip.tscn")

func _ready():
    var playerShip = player_ship_scene.instantiate() as PlayerShip
    var saver = ShipSaver.load_save();
    get_parent().add_child.call_deferred(playerShip)
    saver.build_ship(playerShip)
    playerShip.TryBuildShip()
