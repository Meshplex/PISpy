using System.Device.Gpio;
using PiSpyBackend.Domain.Interfaces;

namespace PiSpyBackend.Application.Services
{
  public class MockGpioControllerService : IGpioControllerService
  {
    private readonly Dictionary<int, PinValue> _pinStates = new();

    public void OpenPin(int pin, PinMode mode)
    {
      Console.WriteLine($"[Fake] OpenPin: {pin}, Mode: {mode}");
      _pinStates[pin] = PinValue.Low;
    }

    public void ClosePin(int pin)
    {
      Console.WriteLine($"[Fake] ClosePin: {pin}");
      _pinStates.Remove(pin);
    }

    public void Write(int pin, PinValue value)
    {
      Console.WriteLine($"[Fake] Write: {pin} = {value}");
      _pinStates[pin] = value;
    }

    public PinValue Read(int pin)
    {
      _pinStates.TryGetValue(pin, out var value);
      Console.WriteLine($"[Fake] Read: {pin} => {value}");
      return value;
    }

    public void SetPinMode(int pin, PinMode mode)
    {
      Console.WriteLine($"[Fake] SetPinMode: {pin} = {mode}");
    }

    public bool IsPinOpen(int pin) => _pinStates.ContainsKey(pin);
    public void Dispose() { }
  }
}