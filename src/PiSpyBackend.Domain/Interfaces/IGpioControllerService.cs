using System.Device.Gpio;

namespace PiSpyBackend.Domain.Interfaces
{
  public interface IGpioControllerService : IDisposable
  {
    void OpenPin(int pin, PinMode mode);
    void ClosePin(int pin);
    void Write(int pin, PinValue value);
    PinValue Read(int pin);
    void SetPinMode(int pin, PinMode mode);
    bool IsPinOpen(int pin);
  }
}