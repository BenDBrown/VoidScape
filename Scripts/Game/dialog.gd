extends Node
class_name Dialog
signal on_ended

var speaker: String = ""
var lines: Array[String] = []
var dialogs: Array = []
var line_index = 0
var dialog_index = 0
enum IndentLevel {CONVERSATION = 0, SPEAKER = 4, LINE = 8}

var currentDialog: Dialog

func parse_to_dialog(file: FileAccess):
    var content = file.get_as_text().split("\n", true)
    var currentConv : Dialog
    var currentSpeaker: Dialog
    for line in content:
        var indent_level = get_indentation_level(line)
        line = line.strip_edges()
        if line.length() == 0:
            continue
        match indent_level:
            IndentLevel.CONVERSATION:
                currentConv = Dialog.new()
                currentConv.speaker = line.replace(":","")
                dialogs.append(currentConv)
            IndentLevel.SPEAKER:
                currentSpeaker = Dialog.new()
                currentSpeaker.speaker = line.replace(":","")
                currentConv.dialogs.append(currentSpeaker)
            IndentLevel.LINE:
                currentSpeaker.lines.append(line.replace("-",""))

func get_indentation_level(line: String) -> IndentLevel:
    var trimmed_line = line.lstrip(" ")
    var indent_count = line.length() - trimmed_line.length()
    return indent_count as IndentLevel

func next_line() -> String:
    if dialogs && dialog_index < dialogs.size():
        if currentDialog != dialogs[dialog_index]:
            if currentDialog:
                currentDialog.on_ended.disconnect(onEnd)
            currentDialog = dialogs[dialog_index]
            currentDialog.on_ended.connect(onEnd)
        return currentDialog.next_line()

    if line_index < lines.size():
        line_index = line_index + 1
        return lines[line_index-1]
    else:
        on_ended.emit()
    return "NaN"

func get_speaker():
    if lines.size() > 0:
        return speaker
    return currentDialog.get_speaker()

func onEnd():
    dialog_index = dialog_index + 1
