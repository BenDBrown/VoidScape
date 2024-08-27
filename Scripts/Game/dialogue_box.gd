extends Control

@export var file_path: String
@onready var box = $Dialogue/Label
@onready var speaker = $Speaker/Label
var dialog : Dialog = Dialog.new()
func _ready():
    if !FileAccess.file_exists(file_path):
        printerr("Error occured when trying to locate file: "+ file_path)
        return
    var file = FileAccess.open(file_path, FileAccess.READ)
    dialog.parse_to_dialog(file)

func _process(_delta):
    if Input.is_action_just_pressed("click"):
        box.text = dialog.next_line()
        speaker.text = dialog.get_speaker()
