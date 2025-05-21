import { HubConnectionBuilder, HubConnection, HttpTransportType, LogLevel } from '@microsoft/signalr';
import { EventType } from '../types/Eventtype';

class EventService {
  private connection: HubConnection;
  private handlers = {
    all: new Set<(events: EventType[]) => void>(),
    new: new Set<(evt: EventType) => void>()
  };

  constructor() {
    const hubUrl = 'http://localhost:5272/alarmHub';
    this.connection = new HubConnectionBuilder().withUrl(hubUrl, {
    skipNegotiation: true,
    transport: HttpTransportType.WebSockets
  }).withAutomaticReconnect().build();

    // Log connection lifecycle events
    this.connection.onclose(error => console.debug('SignalR connection closed', error));
    this.connection.onreconnecting(error => console.debug('SignalR reconnecting', error));
    this.connection.onreconnected(connectionId => console.debug('SignalR reconnected, connectionId:', connectionId));

    // Log incoming messages
    this.connection.on('ReceiveAllEvents', (events: EventType[]) => {
      console.debug('Received all events:', events);
      this.handlers.all.forEach(fn => fn(events));
    });
    this.connection.on('ReceiveEvent', (evt: EventType) => {
      console.debug('Received new event:', evt);
      this.handlers.new.forEach(fn => fn(evt));
    });
  }

  async start() {
    console.debug('SignalR start called, current state:', this.connection.state);
    if (this.connection.state === 'Disconnected') {
      try {
        await this.connection.start();
        console.debug('SignalR connected, connection state:', this.connection.state);
      } catch (err) {
        console.error('SignalR-Verbindungsfehler:', err);
        throw err;
      }
    }
  }

  stop() {
    console.debug('SignalR stop called');
    return this.connection.stop();
  }

  /**
   * Subscribes to the initial event list. Returns an unsubscribe function.
   */
  subscribeAll(handler: (events: EventType[]) => void): () => void {
    this.handlers.all.add(handler);
    return () => {
      this.handlers.all.delete(handler);
    };
  }

  /**
   * Subscribes to new events. Returns an unsubscribe function.
   */
  subscribeNew(handler: (evt: EventType) => void): () => void {
    this.handlers.new.add(handler);
    return () => {
      this.handlers.new.delete(handler);
    };
  }
}

export const eventService = new EventService();