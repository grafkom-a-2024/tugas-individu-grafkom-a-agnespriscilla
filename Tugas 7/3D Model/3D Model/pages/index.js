import { Canvas } from "@react-three/fiber";
import React, { useState } from "react";
import Character from "../components/Character";
import { OrbitControls } from "@react-three/drei";

const index = () => {
  const [allAnimation, setAllAnimation] = useState([
    "golf",
    "idle",
    "jump",
    "pockets",
    "react",
    "rope",
    "shrug",
    "swingdance",
    "wave",
  ]);

  const [currentAnimationName, setCurrentAnimationName] = useState("golf");
  return (
    <div className="w-[100vw] h-[150vh]">
      <Canvas>
        <OrbitControls />
        <ambientLight />
        <directionalLight position={[-5, 5, 5]} />
        <Character currentAnimationName={currentAnimationName} />
      </Canvas>
      <div className="absolute top-0 right-3 flex flex-col justify-center h-[100vh]">
        {allAnimation.map((name, i) => {
          return (
            <div key={i} className="bg-purple-500 m-2 rounded-2xl text-center">
              <button
                onClick={(e) => setCurrentAnimationName(name)}
                className="p-2 w-full"
              >
                {name}
              </button>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default index;
