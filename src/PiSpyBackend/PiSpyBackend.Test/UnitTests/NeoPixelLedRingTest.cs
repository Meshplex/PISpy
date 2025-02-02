using System.Device.Spi;
using System.Diagnostics;
using System.Drawing;
using rpi_ws281x;

namespace PiSpyBackend.Test.UnitTests
{
    public class NeoPixelLedRingTests
    {

        [Fact]
        public void SetAllLedsToGreenTest()
        {
            // Erstelle und konfiguriere den Prozess
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "sudo";
                // Hier wird der Befehl über sudo ausgeführt:
                process.StartInfo.Arguments = "/home/admin/neo_pixel_project/venv/bin/python led_ring_rot.py";
                process.StartInfo.WorkingDirectory = "/home/admin/neo_pixel_project/";
                // Damit wir Ausgaben einlesen können
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                
                try
                {
                    // Starte das Python-Skript
                    process.Start();

                    // Warte 5 Sekunden (5000 Millisekunden)
                    Thread.Sleep(5000);

                    process.Kill();

                    // Falls das Skript noch läuft, beende den Prozess
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }

                    // Optional: Lese die Ausgaben des Skripts
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // Warte, bis der Prozess endgültig beendet ist
                    process.WaitForExit();

                    // Mit einer Assertion kannst du überprüfen, dass der Prozess beendet ist
                    Assert.True(process.HasExited, "Der Prozess wurde nicht beendet.");

                    // Optional: Weitere Assertions, z.B. dass keine Fehlerausgabe vorliegt
                    Assert.True(string.IsNullOrEmpty(error), $"Fehler beim Ausführen des Skripts: {error}");
                }
                catch (Exception ex)
                {
                    // Falls ein Fehler auftritt, schlägt der Test fehl
                    Assert.False(true, $"Fehler beim Ausführen des Python-Skripts: {ex.Message}");
                }
            }
        }

        [Fact]
        public void SetAllLedsToRedTest()
        {
            // Erstelle und konfiguriere den Prozess
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "sudo";
                // Hier wird der Befehl über sudo ausgeführt:
                process.StartInfo.Arguments = "/home/admin/neo_pixel_project/venv/bin/python led_ring_gruen.py";
                process.StartInfo.WorkingDirectory = "/home/admin/neo_pixel_project/";
                // Damit wir Ausgaben einlesen können
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                
                try
                {
                    // Starte das Python-Skript
                    process.Start();

                    // Warte 5 Sekunden (5000 Millisekunden)
                    Thread.Sleep(5000);

                    process.Kill();

                    // Falls das Skript noch läuft, beende den Prozess
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }

                    // Optional: Lese die Ausgaben des Skripts
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // Warte, bis der Prozess endgültig beendet ist
                    process.WaitForExit();

                    // Mit einer Assertion kannst du überprüfen, dass der Prozess beendet ist
                    Assert.True(process.HasExited, "Der Prozess wurde nicht beendet.");

                    // Optional: Weitere Assertions, z.B. dass keine Fehlerausgabe vorliegt
                    Assert.True(string.IsNullOrEmpty(error), $"Fehler beim Ausführen des Skripts: {error}");
                }
                catch (Exception ex)
                {
                    // Falls ein Fehler auftritt, schlägt der Test fehl
                    Assert.False(true, $"Fehler beim Ausführen des Python-Skripts: {ex.Message}");
                }
            }
        }
    }
}
