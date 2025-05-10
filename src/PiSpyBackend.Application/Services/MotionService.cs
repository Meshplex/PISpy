using System.Device.Gpio;
using System.Diagnostics;
using PiSpyBackend.Domain.Interfaces;

namespace PiSpyBackend.Application
{
    public class MotionService : ISensorService, IDisposable
    {
        private const int SensorPin = 27;
        private readonly IGpioControllerService _controller;

        public MotionService(IGpioControllerService controller)
        {
            _controller = controller;
            _controller.OpenPin(SensorPin, PinMode.Input);
        }

        public object Run()
        {
            return CheckForMotion(TimeSpan.FromMilliseconds(1500));
        }

        public bool CheckForMotion(TimeSpan duration)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < duration)
            {
                if (_controller.Read(SensorPin) == PinValue.High)
                {
                    return true; // Bewegung erkannt
                }
                Thread.Sleep(10);
            }
            return false; // Keine Bewegung erkannt
        }

        public void Dispose()
        {
            if (_controller.IsPinOpen(SensorPin))
            {
                _controller.ClosePin(SensorPin);
            }
            _controller.Dispose();
        }
    }
}