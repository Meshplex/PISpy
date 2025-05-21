using System;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Interface für Kameradienste.
/// </summary>
public interface ICamService
{
  /// <summary>
  /// Nimmt ein Bild auf und speichert es, gibt den Datei-Pfad zurück.
  /// </summary>
  /// <returns>Pfad zur gespeicherten Bilddatei.</returns>
  string TakePicture();
}

/// <summary>
/// CamService-Implementierung mit libcamera-still.
/// </summary>
public class CamService : ICamService
{
  // Fester Ordnerpfad für gespeicherte Bilder
  private const string SaveDirectory = "/home/pi/captured";

  /// <summary>
  /// Nimmt ein Bild mit dem Kamera-Modul auf und speichert es in SaveDirectory.
  /// </summary>
  /// <returns>Pfad zur gespeicherten Bilddatei.</returns>
  public string TakePicture()
  {
    try
    {
      // Sicherstellen, dass das Verzeichnis existiert (erstellt es bei Bedarf)
      Directory.CreateDirectory(SaveDirectory);

      // Dateiname mit Zeitstempel, um Überschreiben zu vermeiden
      string dbFileName = $"capture_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
      string fileName = Path.Combine(SaveDirectory, dbFileName);

      // Konfiguration des ProcessStartInfo für libcamera-still
      var startInfo = new ProcessStartInfo
      {
        FileName = "libcamera-still",
        Arguments = $"--nopreview -t 200 -o \"{fileName}\"",
        RedirectStandardError = true,
        UseShellExecute = false,  // nötig, um Ausgaben umzuleiten
        CreateNoWindow = true
      };

      // Prozess starten
      using var process = Process.Start(startInfo)
          ?? throw new InvalidOperationException("libcamera-still konnte nicht gestartet werden.");

      // Fehlerausgabe lesen (falls vorhanden) und auf Prozessende warten
      string errorOutput = process.StandardError.ReadToEnd();
      process.WaitForExit();

      // Bei Fehlercode ≠ 0 eine Ausnahme werfen
      if (process.ExitCode != 0)
      {
        throw new Exception($"libcamera-still schlug fehl (Exit-Code {process.ExitCode}): {errorOutput}");
      }

      return dbFileName;
    }
    catch (Exception ex)
    {
      // Hier könnte zusätzliche Fehlerbehandlung oder Logging erfolgen
      Console.Error.WriteLine($"Fehler beim Aufnehmen des Bildes: {ex.Message}");
      throw;
    }
  }
}
