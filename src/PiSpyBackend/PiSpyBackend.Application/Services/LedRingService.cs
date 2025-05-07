using System.Diagnostics;

namespace PiSpyBackend.Application
{
    class LedRingService
    {
        public void ActivateRedLed()
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "sudo";
                process.StartInfo.Arguments = "/home/admin/neo_pixel_project/venv/bin/python led_ring_gruen.py";
                process.StartInfo.WorkingDirectory = "/home/admin/neo_pixel_project/";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                
                try
                {
                    process.Start();
                    Thread.Sleep(5000);
                    process.Kill();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void ActivateGreenLed()
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "sudo";
                process.StartInfo.Arguments = "/home/admin/neo_pixel_project/venv/bin/python led_ring_rot.py";
                process.StartInfo.WorkingDirectory = "/home/admin/neo_pixel_project/";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                
                try
                {
                    process.Start();
                    Thread.Sleep(5000);
                    process.Kill();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void DeactivateLed()
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "sudo";
                // Hier wird der Befehl über sudo ausgeführt:
                process.StartInfo.Arguments = "/home/admin/neo_pixel_project/venv/bin/python turn_off.py";
                process.StartInfo.WorkingDirectory = "/home/admin/neo_pixel_project/";
                // Damit wir Ausgaben einlesen können
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                
                try
                {
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}