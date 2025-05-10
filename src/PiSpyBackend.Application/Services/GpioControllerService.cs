using System.Device.Gpio;
using PiSpyBackend.Domain.Interfaces;

namespace PiSpyBackend.Application.Services
{
  public class GpioControllerService : IGpioControllerService
  {
    private readonly GpioController _controller;

    public GpioControllerService(PinNumberingScheme scheme = PinNumberingScheme.Logical)
    {
      _controller = new GpioController(scheme);
    }

    public void OpenPin(int pin, PinMode mode) => _controller.OpenPin(pin, mode);
    public void ClosePin(int pin) => _controller.ClosePin(pin);
    public void Write(int pin, PinValue value) => _controller.Write(pin, value);
    public PinValue Read(int pin) => _controller.Read(pin);
    public void SetPinMode(int pin, PinMode mode) => _controller.SetPinMode(pin, mode);
    public bool IsPinOpen(int pin) => _controller.IsPinOpen(pin);
    public void Dispose() => _controller.Dispose();
  }
}