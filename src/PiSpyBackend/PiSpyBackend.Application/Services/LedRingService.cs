using PiSpyBackend.Domain.Interfaces;

namespace PiSpyBackend.Application
{
    class LedRingService : ISensorService
    {
        public void ActiveAlarmLed()
        {
            // Led Soll Rot leuchten wenn die Alarmanlage scharf ist
        }

        public void DeactiveAlarmLed()
        {
            // Led Soll Grün leuchten wenn die Alarmanlage aus ist
        }

        public void AlarmLedBlink()
        {
            // Led Soll Rot Aufleuchten sync zum pieper wenn der Alarm an ist 
        }

        public object Run()
        {
            throw new NotImplementedException();
        }
    }
}