extends Control

@export var add_credits_amount = 200
@export var mcnuggets_price = 150

@onready var player_ship = Game.PlayerShip

@onready var credits_check_label: Label = $credits_check
@onready var nuggets_amounts_label: Label = $"nuggets amounts"

var nuggets = 0

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if(player_ship != null):
		var has_enough = player_ship.HasEnoughCredits(mcnuggets_price)
		
		if(has_enough):
			credits_check_label.text = "You have enough."
		else:
			credits_check_label.text = "You do not have enough credits."

func _on_buy_McNuggets_pressed() -> void:
	var purchaseValid = player_ship.TryTakeCredits(mcnuggets_price)
	
	if (purchaseValid):
		nuggets +=1
		nuggets_amounts_label.text = "You have " + str(nuggets) + " stars."
		

func _on_add_credits_pressed() -> void:
	player_ship.AddCredits(add_credits_amount)
