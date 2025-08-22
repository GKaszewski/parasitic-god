shader_type canvas_item;

// The color of the healthy terrain you want to target.
uniform vec4 terrain_color : source_color = vec4(0.0, 1.0, 0.0, 1.0);
// The color the terrain will turn into as it gets corrupted.
uniform vec4 corrupted_color : source_color = vec4(0.3, 0.1, 0.2, 1.0);
// This value will be controlled by your C# script (from 0.0 to 1.0).
uniform float corruption_level : hint_range(0.0, 1.0);
// A small tolerance to catch pixels that aren't *exactly* the terrain color.
uniform float tolerance : hint_range(0.0, 0.5) = 0.1;

void fragment() {
    // Get the original color of the pixel from the sprite's texture.
    vec4 original_color = texture(TEXTURE, UV);

    // Calculate the difference between this pixel's color and the healthy terrain color.
    // The distance() function is great for comparing colors.
    float color_difference = distance(original_color.rgb, terrain_color.rgb);

    // Check if the difference is within our tolerance.
    if (color_difference < tolerance) {
        // If it's a terrain pixel, mix the original terrain color with the
        // corrupted color based on the corruption_level.
        vec3 final_color = mix(terrain_color.rgb, corrupted_color.rgb, corruption_level);
        COLOR = vec4(final_color, original_color.a);
    } else {
        // If it's not a terrain pixel (i.e., it's ocean), leave it unchanged.
        COLOR = original_color;
    }
}
