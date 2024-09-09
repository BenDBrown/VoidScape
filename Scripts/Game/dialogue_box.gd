extends Control

@export var file_path: String
var dialog: Dialog = Dialog.new()

@onready var speaker_panel = $Speaker
@onready var dialog_panel = $Dialogue
@onready var speaker_label = $Speaker/Label
@onready var dialog_label = $Dialogue/Label


func _ready():
	if file_path:
		set_dialog_by_path(file_path)
		return


func _process(_delta):
	if Input.is_action_just_pressed("click") && speaker_panel.is_visible_in_tree():
		click()
	if Input.is_action_just_pressed("forward"):
		show_dialog_box()


func click():
	var nextLine = dialog.next_line()
	if nextLine is int && nextLine == dialog.EXIT_CODE:
		hide_dialog_box()
		return
	dialog_label.text = nextLine
	speaker_label.text = dialog.get_speaker()


func show_dialog_box():
	click()
	speaker_panel.show()
	dialog_panel.show()


func hide_dialog_box():
	speaker_panel.hide()
	dialog_panel.hide()


func set_dialog(new_dialog: Dialog):
	dialog = new_dialog


func set_dialog_by_path(path: String):
	if !FileAccess.file_exists(path):
		printerr("Error occured when trying to locate file: " + path)
		return
	var file = FileAccess.open(path, FileAccess.READ)
	dialog.parse_to_dialog(file)
