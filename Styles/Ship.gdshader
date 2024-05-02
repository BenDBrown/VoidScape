shader_type canvas_item;

uniform vec4 first:source_color;
uniform vec4 second:source_color;
uniform vec4 third:source_color;
uniform vec4 fourth:source_color;
uniform vec4 new_first:source_color;
uniform vec4 new_second:source_color;
uniform vec4 new_third:source_color;
uniform vec4 new_fourth:source_color;

void fragment() {
	vec4 current_pixel = texture(TEXTURE, UV);
	if(current_pixel == first){
		COLOR = new_first;
	}else if(current_pixel == second){
		COLOR = new_second;
	}else if(current_pixel == third){
		COLOR = new_third;
	}else if(current_pixel == fourth){
		COLOR = new_fourth;
	}
}