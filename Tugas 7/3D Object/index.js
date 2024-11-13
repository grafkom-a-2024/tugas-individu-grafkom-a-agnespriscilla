import * as THREE from "three";
import { OrbitControls } from "jsm/controls/OrbitControls.js";

const w = window.innerWidth;
const h = window.innerHeight;
const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(w, h);
document.body.appendChild(renderer.domElement);

const fov = 75;
const aspect = w / h;
const near = 0.1;
const far = 10;
const camera = new THREE.PerspectiveCamera(fov, aspect, near, far);
camera.position.z = 2;
const scene = new THREE.Scene();

const controls = new OrbitControls(camera, renderer.domElement);
controls.enableDamping = true;
controls.dampingFactor = 0.03;

// Membuat shader material untuk gradasi warna
const gradMaterial = new THREE.ShaderMaterial({
  vertexShader: `
    varying vec3 vPosition;
    void main() {
      vPosition = position;
      gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
    }
  `,
  fragmentShader: `
    varying vec3 vPosition;
    void main() {
      // Buat gradien dari biru ke ungu berdasarkan posisi y
      float mixValue = (vPosition.y + 1.0) / 2.0;
      vec3 color = mix(vec3(0.0, 0.0, 1.0), vec3(1.0, 0.0, 1.0), mixValue);
      gl_FragColor = vec4(color, 1.0);
    }
  `,
  flatShading: true,
});

const geo = new THREE.IcosahedronGeometry(1.0, 2);
const mesh = new THREE.Mesh(geo, gradMaterial);
scene.add(mesh);

// Tambahkan wireframe sebagai lapisan atas
const wireMat = new THREE.MeshBasicMaterial({ 
    color: 0xffffff,
    wireframe: true
});
const wireMesh = new THREE.Mesh(geo, wireMat);
wireMesh.scale.setScalar(1.001);
mesh.add(wireMesh);

const hemilight = new THREE.HemisphereLight(0xffffff, 0xaa5500);
scene.add(hemilight);

function animate(t = 0) {
    requestAnimationFrame(animate);
    renderer.render(scene, camera);
    controls.update();
}
animate();
