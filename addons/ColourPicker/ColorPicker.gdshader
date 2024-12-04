shader_type canvas_item;

uniform vec4 color : source_color;
uniform vec4 shades[10] : source_color;
uniform float tolerance = 0.001;

void fragment() {
	vec4 pixelColor = texture(TEXTURE, UV);
	int shadeCount = shades.length();
	for(int i = 0; i < shadeCount; i++)
	{
		if(distance(shades[i], pixelColor) < tolerance)
		{
			pixelColor *= color;
			break;
		}
	}
	COLOR = pixelColor;
}