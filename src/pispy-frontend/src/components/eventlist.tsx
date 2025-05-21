import { useState, useEffect } from "react";
import { eventService } from "../api/evnentService";
import { EventType, EventTypeEnum } from "../types/Eventtype";

function EventList(){
    const [events, setEvents] = useState<EventType[]>([]);
  
    useEffect(() => {
      // Handler-Funktionen
      const handleAll = (all: EventType[]) => setEvents(all);
      const handleNew = (evt: EventType) => setEvents(prev => [...prev, evt]);
  
      // Abonnements aufbauen und Unsubscribe-Funktionen erhalten
      const unsubscribeAll = eventService.subscribeAll(handleAll);
      const unsubscribeNew = eventService.subscribeNew(handleNew);
  
      // Connection starten
      eventService.start().catch(console.error);
  
      return () => {
        // Nur Handlers abmelden, Connection läuft weiter
        unsubscribeAll();
        unsubscribeNew();
      };
    }, []);
  

  return (
    <div className="bg-gray-800 text-blue-500 p-4 rounded-md shadow-md">
      <h2 className="text-lg font-semibold mb-2">📋 Ereignisliste</h2>
      {events.length === 0 ? (
        <p className="text-blue-300 text-sm">Keine Ereignisse gefunden.</p>
      ) : (
        <ul
          className={`space-y-1 text-blue-300 text-sm ${
            events.length > 5 ? "max-h-64 overflow-y-auto" : ""
          }`}
        >
          {events
            .slice() // copy array
            .reverse() // flip order
            .map((event, idx) => (
              <li key={event.eventId} className="bg-gray-700 p-2 rounded-md">
                <strong>#{event.eventId}</strong> – {
                  // Show enum name instead of number
                  typeof event.eventType === "number"
                    ? EventTypeEnum[event.eventType]
                    : event.eventType
                }
                <div className="text-xs text-gray-400">
                  {new Date(event.timestamp).toLocaleString("de-DE")}
                </div>
              </li>
            ))}
        </ul>
      )}
    </div>
  );
}

export default EventList;
