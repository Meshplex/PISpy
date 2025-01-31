using System;
using System.Device.Gpio;
using System.Threading;

public class Buzzer : IDisposable
{
    private readonly int _pin;
    private readonly GpioController _controller;
    private bool _disposed = false;

    public Buzzer(int pin)
    {
        _pin = pin;
        _controller = new GpioController();
        _controller.OpenPin(_pin, PinMode.Output);
    }

    /// <summary>
    /// Erzeugt einen Piepton mit der angegebenen Frequenz und Dauer.
    /// </summary>
    /// <param name="frequency">Frequenz in Hertz (Hz).</param>
    /// <param name="duration">Dauer des Pieptons in Millisekunden.</param>
    public void Beep(int frequency = 1000, int duration = 500)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(Buzzer));

        if (frequency <= 0)
            throw new ArgumentOutOfRangeException(nameof(frequency), "Frequenz muss größer als 0 sein.");

        if (duration <= 0)
            throw new ArgumentOutOfRangeException(nameof(duration), "Dauer muss größer als 0 sein.");

        int periodMicroseconds = 1000000 / frequency;
        int halfPeriodMicroseconds = periodMicroseconds / 2;

        DateTime endTime = DateTime.Now.AddMilliseconds(duration);

        while (DateTime.Now < endTime)
        {
            _controller.Write(_pin, PinValue.High);
            BusyWait(halfPeriodMicroseconds);
            _controller.Write(_pin, PinValue.Low);
            BusyWait(halfPeriodMicroseconds);
        }
    }

    /// <summary>
    /// Eine präzise Wartefunktion basierend auf DateTime.
    /// </summary>
    /// <param name="microseconds">Wartezeit in Mikrosekunden.</param>
    private void BusyWait(int microseconds)
    {
        var start = DateTime.Now;
        while ((DateTime.Now - start).TotalMilliseconds * 1000 < microseconds)
        {
            // Busy wait
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _controller.Write(_pin, PinValue.Low);
            _controller.ClosePin(_pin);
            _controller.Dispose();
            _disposed = true;
        }
    }
}
