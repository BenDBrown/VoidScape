extends Control

@export var player_ship: PlayerShip

var menu_is_open: bool = false
var selected_index: int = 0

@export var type_gun: Texture
@export var type_laser: Texture

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if Input.is_action_just_pressed("open_weapon_menu"):
		open_weapon_menu()
		
	elif Input.is_action_just_released("open_weapon_menu"):
		close_weapon_menu()
		
	if menu_is_open:
		if Input.is_action_just_pressed("cycle_weapon_up"):
			#player_ship.CycleGunGroupUp()
			cycle_weapon_ui(-1)
			
			
		if Input.is_action_just_pressed("cycle_weapon_down"):
			#player_ship.CycleGunGroupDown()
			cycle_weapon_ui(1)
			

func open_weapon_menu() -> void:
	menu_is_open = true
	create_menu()
	self.visible = true
	
func close_weapon_menu() -> void:
	menu_is_open = false
	self.visible = false
	
func create_menu(): # Create the menu in UI
	
	var gun_types = player_ship.GetAvailableGunTypes()
	var index = 0
	var active_index = player_ship.GetActiveWeaponIndex()
	
	for child in get_children():

		if child is weapon_option_ui:
			
			if index <= (gun_types.size() - 1):
				
				print(gun_types[index])
				match gun_types[index]:
					"Gun":
						child.set_type_icon(type_gun)
						print("Its a gun")
					"Laser":
						child.set_type_icon(type_laser)
						print("Its a laser")
						
				if index == active_index:
					child.set_selected()
				else:
					child.set_unselected()
					
			else:
				print("No gun at slot: ", index)
				child.clear_type_icon()
				child.set_unavailable()
		index += 1
	
	
var gun_index = 0 

func cycle_weapon_ui(cycleNum: int):
	
	var gunType_list = player_ship.GetAvailableGunTypes	()
	var active_index = player_ship.GetActiveWeaponIndex()
	
	active_index = gun_index

	print(gunType_list)
	print("Cycling through weapons: ", cycleNum)
	var selected_child_ui: weapon_option_ui = get_child(active_index)
	
	var new_selected_gun_index = active_index + cycleNum
	if new_selected_gun_index < 0:
		print("Go to last")
		var new_child_ui: weapon_option_ui = get_child(get_child_count() - 1)
		new_child_ui.set_selected()
		selected_child_ui.set_unselected()
		
		gun_index = get_child_count() - 1
		
	#elif new_selected_gun_index <= (gunType_list.size() - 1):
	elif new_selected_gun_index <= 3:
		print("Go to new one")
		var new_child_ui: weapon_option_ui = get_child(new_selected_gun_index)
		new_child_ui.set_selected()
		selected_child_ui.set_unselected()
		
		gun_index = new_selected_gun_index
	elif new_selected_gun_index >= gunType_list.size():
		print("Go to first one")
		var new_child_ui: weapon_option_ui = get_child(0)
		new_child_ui.set_selected()
		selected_child_ui.set_unselected()
	
		gun_index = 0
	
