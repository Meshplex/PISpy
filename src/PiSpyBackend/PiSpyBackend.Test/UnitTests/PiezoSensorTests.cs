using System;
using System.Device.Pwm;
using System.Threading;
using Xunit;

namespace SensorTests
{
    public class PiezoSensorTests
    {
        [Fact]
        public void Buzzer_Should_Beep_Shortly()
        {
            int buzzerPin = 18;

            using (var buzzer = new Buzzer(buzzerPin))
            {
                buzzer.Beep(frequency: 440, duration: 500);
            }
        }
    }
}
