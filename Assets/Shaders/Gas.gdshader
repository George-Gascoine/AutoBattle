shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

uniform vec2 resolution;
uniform float time;

void fragment() {
    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = SCREEN_UV;

    // Time-dependent 2D Perlin noise
    float noise = textureLod(SCREEN_TEXTURE, uv + vec2(time), 0.0).r;

    // Output to screen
    COLOR = vec4(vec3(0, noise, 0), 1.0);
}