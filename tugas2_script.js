// Setup WebGL context
const canvas = document.getElementById('glCanvas');
const gl = canvas.getContext('webgl');

// Shader source
const vertexShaderSource = `
attribute vec2 a_position;
uniform mat3 u_matrix;
void main() {
    // Apply the transformation matrix
    vec2 position = (u_matrix * vec3(a_position, 1)).xy;
    gl_Position = vec4(position, 0, 1);
}`;

const fragmentShaderSource = `
precision mediump float;
uniform vec4 u_color;
void main() {
    gl_FragColor = u_color;
}`;

// Compile and link shaders
function createShader(gl, type, source) {
    const shader = gl.createShader(type);
    gl.shaderSource(shader, source);
    gl.compileShader(shader);
    if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) {
        console.error('Shader compile failed:', gl.getShaderInfoLog(shader));
        gl.deleteShader(shader);
        return null;
    }
    return shader;
}

function createProgram(gl, vertexShader, fragmentShader) {
    const program = gl.createProgram();
    gl.attachShader(program, vertexShader);
    gl.attachShader(program, fragmentShader);
    gl.linkProgram(program);
    if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
        console.error('Program link failed:', gl.getProgramInfoLog(program));
        gl.deleteProgram(program);
        return null;
    }
    return program;
}

// Initialize shaders and program
const vertexShader = createShader(gl, gl.VERTEX_SHADER, vertexShaderSource);
const fragmentShader = createShader(gl, gl.FRAGMENT_SHADER, fragmentShaderSource);
const program = createProgram(gl, vertexShader, fragmentShader);
gl.useProgram(program);

// Look up attribute and uniform locations
const positionLocation = gl.getAttribLocation(program, 'a_position');
const matrixLocation = gl.getUniformLocation(program, 'u_matrix');
const colorLocation = gl.getUniformLocation(program, 'u_color');

// Create a buffer to hold the square's vertices
const positionBuffer = gl.createBuffer();
gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
const positions = [
    -0.5, -0.5,
     0.5, -0.5,
    -0.5,  0.5,
     0.5,  0.5,
];
gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(positions), gl.STATIC_DRAW);

// Set up vertex attributes
gl.enableVertexAttribArray(positionLocation);
gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
gl.vertexAttribPointer(positionLocation, 2, gl.FLOAT, false, 0, 0);

// Set the color
gl.uniform4f(colorLocation, 0.2, 0.7, 0.8, 1);

// Helper function for creating 3x3 matrices
function makeTranslation(tx, ty) {
    return [
        1, 0, 0,
        0, 1, 0,
        tx, ty, 1
    ];
}

function makeRotation(angleInRadians) {
    const c = Math.cos(angleInRadians);
    const s = Math.sin(angleInRadians);
    return [
        c, -s, 0,
        s,  c, 0,
        0,  0, 1
    ];
}

function makeScale(sx, sy) {
    return [
        sx, 0, 0,
        0, sy, 0,
        0,  0, 1
    ];
}

// Matrix multiplication function
function multiplyMatrices(a, b) {
    const a00 = a[0 * 3 + 0];
    const a01 = a[0 * 3 + 1];
    const a02 = a[0 * 3 + 2];
    const a10 = a[1 * 3 + 0];
    const a11 = a[1 * 3 + 1];
    const a12 = a[1 * 3 + 2];
    const a20 = a[2 * 3 + 0];
    const a21 = a[2 * 3 + 1];
    const a22 = a[2 * 3 + 2];

    const b00 = b[0 * 3 + 0];
    const b01 = b[0 * 3 + 1];
    const b02 = b[0 * 3 + 2];
    const b10 = b[1 * 3 + 0];
    const b11 = b[1 * 3 + 1];
    const b12 = b[1 * 3 + 2];
    const b20 = b[2 * 3 + 0];
    const b21 = b[2 * 3 + 1];
    const b22 = b[2 * 3 + 2];

    return [
        a00 * b00 + a01 * b10 + a02 * b20,
        a00 * b01 + a01 * b11 + a02 * b21,
        a00 * b02 + a01 * b12 + a02 * b22,
        a10 * b00 + a11 * b10 + a12 * b20,
        a10 * b01 + a11 * b11 + a12 * b21,
        a10 * b02 + a11 * b12 + a12 * b22,
        a20 * b00 + a21 * b10 + a22 * b20,
        a20 * b01 + a21 * b11 + a22 * b21,
        a20 * b02 + a21 * b12 + a22 * b22
    ];
}

// Animation variables
let translation = [0, 0];
let rotationInRadians = 0;
let scale = [1, 1];

function drawScene() {
    // Clear the canvas
    gl.clearColor(0, 0, 0, 0);
    gl.clear(gl.COLOR_BUFFER_BIT);

    // Compute transformations
    let matrix = makeTranslation(translation[0], translation[1]);
    matrix = multiplyMatrices(matrix, makeRotation(rotationInRadians));
    matrix = multiplyMatrices(matrix, makeScale(scale[0], scale[1]));

    // Set the matrix
    gl.uniformMatrix3fv(matrixLocation, false, matrix);

    // Draw the square
    gl.drawArrays(gl.TRIANGLE_STRIP, 0, 4);
}

// Animate the transformations
function animate() {
    translation = [Math.sin(Date.now() * 0.001), Math.cos(Date.now() * 0.001)];
    rotationInRadians += 0.01;
    scale = [Math.sin(Date.now() * 0.001) + 1.5, Math.cos(Date.now() * 0.001) + 1.5];

    drawScene();
    requestAnimationFrame(animate);
}

// Start the animation
animate();
