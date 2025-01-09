// src/components/Modal.tsx

import React from "react";

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  zIndex?: string; // Falls du mehrere Popups stacken möchtest
  children: React.ReactNode;
}

const Modal: React.FC<ModalProps> = ({
  isOpen,
  onClose,
  title,
  zIndex = "z-50",
  children,
}) => {
  if (!isOpen) return null;

  return (
    <>
      {/* Overlay (schwarzer, halbtransparenter Hintergrund) */}
      <div
        className={`fixed inset-0 bg-black bg-opacity-50 ${zIndex}`}
        onClick={onClose}
      />
      {/* Modal-Container zentriert */}
      <div
        className={`fixed inset-0 flex items-center justify-center ${zIndex}`}
        // stopPropagation, damit Klick aufs Modal selbst den Dialog NICHT schließt
        onClick={(e) => e.stopPropagation()}
      >
        {/* Modal selbst */}
        <div className="relative bg-[#1A1A1A] text-[#E2E8F0] rounded p-6 w-full max-w-md mx-auto">
          {/* Titelzeile mit X-Button */}
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-xl font-bold text-[#007ACC]">{title}</h2>
            <button
              onClick={onClose}
              className="text-gray-400 hover:text-gray-200 transition-colors"
              aria-label="Close"
            >
              ✕
            </button>
          </div>

          {children}
        </div>
      </div>
    </>
  );
};

export default Modal;
