using System.Device.Spi;
using System.Diagnostics;
using System.Drawing;
using rpi_ws281x;

namespace PiSpyBackend.Test.UnitTests
{
    public class NeoPixelLedRingTests
    {

        [Fact]
        public void SetAllLedsToRedTest()
        {
            Process process = new Process();
                
                // Wir setzen "sudo" als auszuführendes Programm
                process.StartInfo.FileName = "sudo";
                // Übergabe der Argumente: Pfad zu Python in der virtuellen Umgebung und das Skript
                process.StartInfo.Arguments = "/home/admin/neo_pixel_project/venv/bin/python led_ring_rot.py";
                
                // Damit wir die Ausgaben einsehen können:
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                // Start des Prozesses
                process.Start();
        }
    }
}
