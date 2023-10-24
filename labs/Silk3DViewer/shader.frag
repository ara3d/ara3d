#version 330 core
in vec3 fUvw;

out vec4 FragColor;

void main()
{
    FragColor = vec4(fUvw.x, fUvw.y, fUvw.z, 1.0);
}
