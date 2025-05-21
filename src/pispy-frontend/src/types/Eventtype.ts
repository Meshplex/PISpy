export interface EventType {
  eventId: number;
  keyId?: string | null;
  userId: number;
  eventType: EventTypeEnum; // entsprechend deinem Enum Eventtype als string
  timestamp: string;  // ISO-String
}

export enum EventTypeEnum{
  Motion_Detected = 1,
  Alarm_Triggered = 2,
  Alarm_Activated = 3,
  Alarm_Deactivated = 4,
  Window_Opened = 5,
}