import React, { useEffect, useState } from "react";
import { getPictures, PictureDTO } from "../api/pictureService";

const PictureModule: React.FC = () => {
  const [pictures, setPictures] = useState<PictureDTO[]>([]);

  useEffect(() => {
    let isMounted = true;

    const fetchAndSet = async () => {
      try {
        const data = await getPictures();
        if (isMounted) {
          setPictures(data);
        }
      } catch (err) {
        console.error("Fehler beim Laden der Bilder:", err);
      }
    };

    // Direkt initial laden
    fetchAndSet();

    // Alle 1000 ms wiederholen
    const intervalId = window.setInterval(fetchAndSet, 1000);

    return () => {
      // Cleanup beim Unmount
      isMounted = false;
      clearInterval(intervalId);
    };
  }, []);

  return (
    <div className="bg-gray-800 text-blue-500 p-4 rounded-md shadow-md">
      <section className="px-4 py-2">
        <h2 className="text-lg font-semibold mb-4">📸 Bewegungen</h2>
        <div className="overflow-x-auto">
          <div className="grid grid-flow-col auto-cols-max gap-4">
            {/* Dieses statische Cat-Bild bleibt erhalten */}
            <img
              src="/Users/tercanthbu1/Desktop/dev/PISpy/img/cat.jpeg"
              alt="Cat"
              className="w-48 h-48 object-cover rounded"
            />

            {/* Gefetchte Bilder */}
            {pictures.map((picture) => (
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
                <small className="block text-sm text-gray-400 mt-1">
                  {new Date(picture.createdAt).toLocaleString("de-DE")}
                </small>
              </a>
            ))}
          </div>
        </div>
      </section>
    </div>
  );
};

export default PictureModule;
