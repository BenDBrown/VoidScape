extends Node2D
@export var death_anim:SingleRunAnimation
@export var grunt_scene:PackedScene
@export var respawn_time:int
@export var core:Node2D
@onready var health_comp = $HealthComponent


var grunts_alive:int = 0
var grunt
var allowed_to_spawn = true
var respawn_timer:Timer

func _ready() -> void:
	#for now the entire structure dies when the core is destroyed. 
	if core:
		core.connect("core_destroyed", _on_health_component_died)
	else: 
		print("No core connected")
	first_enemy_wave()

func _on_health_component_died() -> void:
	allowed_to_spawn = false
	death_anim.visible = true
	death_anim.play()
	respawn_timer.queue_free()

func fucking_die_you_cunt():
	grunt = grunt_scene.instantiate()
	grunts_alive += 1
	grunt.connect("OnDestroyed",on_enemy_death)
	add_child(grunt)

func on_enemy_death(enemy:Ship):
	grunts_alive -=1
	start_respawn_timer_enemy()

func start_respawn_timer_enemy():
	if allowed_to_spawn:
		if respawn_timer:
			if grunts_alive < 2:
				if respawn_timer.time_left == 0:
					print("why are toy here")
					respawn_timer.start(respawn_time)
					

func first_enemy_wave():
	respawn_timer = Timer.new()
	respawn_timer.timeout.connect(fucking_die_you_cunt)
	respawn_timer.name = "respawn_timer"
	respawn_timer.one_shot = true
	add_child(respawn_timer)

func _process(delta: float) -> void:
	start_respawn_timer_enemy()
