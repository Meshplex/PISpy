import React, { useEffect, useState } from "react";
import { getPictures, PictureDTO } from "../api/pictureService";

const PictureModule: React.FC = () => {
  const [pictures, setPictures] = useState<PictureDTO[]>([]);

  useEffect(() => {
    getPictures().then((data: PictureDTO[]) => {
      setPictures(data);
    });
  }, []);

  return (
    <div className="bg-gray-800 text-blue-500 p-4 rounded-md shadow-md">
      <section className="px-4 py-2">
        <h2 className="text-lg font-semibold mb-4">📸 Bewegungen</h2>
        <div className="overflow-x-auto">
          <div className="grid grid-flow-col auto-cols-max gap-4">
            <img
              src="/Users/tercanthbu1/Desktop/dev/PISpy/img/cat.jpeg"
              alt=""
            />
            {pictures.map((picture) => {
              console.log("Picture:", picture);
              return (
                <a
                  key={picture.id}
                  href={picture.path}
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  <img
                    src={picture.path}
                    alt={`Bild ${picture.id}`}
                    className="w-48 h-48 object-cover rounded cursor-pointer"
                  />
                  <small>
                    {new Date(picture.createdAt).toLocaleString("de-DE")}
                  </small>
                </a>
              );
            })}
          </div>
        </div>
      </section>
    </div>
  );
};

export default PictureModule;
