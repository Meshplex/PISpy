using System.Device.Gpio;
using System.Diagnostics;
using PiSpyBackend.Domain.Interfaces;

namespace PiSpyBackend.Application
{
    public class ButtonService : ISensorService, IDisposable
    {
        private const int SensorPin = 17;
        private readonly GpioController _controller;

        public ButtonService(GpioController controller)
        {
            _controller = controller;
            _controller.OpenPin(SensorPin, PinMode.InputPullUp);
        }
        public object Run()
        {   
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < TimeSpan.FromMilliseconds(250))
            {
                if (_controller.Read(SensorPin) == PinValue.Low)
                {
                    return true; // Button Gedrückt
                }
                Thread.Sleep(10);
            }
            return false; // Button nicht gedrückt
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