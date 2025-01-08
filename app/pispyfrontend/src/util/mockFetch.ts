// utils/mockFetch.ts

export async function mockFetch(url: string, options: RequestInit) {
    // Wir parsen den Body, falls vorhanden.
    const body = options.body ? JSON.parse(options.body as string) : {};
  
    // Beispiel: Mock-Endpoint /api/login
    if (url === "/api/login" && options.method === "POST") {
      const { username, password } = body;
  
      // Hier simulieren wir eine feste Demo-Anmeldung:
      if (username === "test" && password === "1234") {
        // "Erfolgreicher Login" -> JWT zurückgeben
        const mockResponseData = {
          token: "MOCK_JWT_TOKEN_ABC123", // könnte ein beliebiger String sein
        };
  
        // Wir geben eine "Response" zurück, so wie fetch es machen würde
        return new Response(JSON.stringify(mockResponseData), {
          status: 200,
          statusText: "OK",
        });
      } else {
        // "Fehlgeschlagen" -> 401 Unauthorized
        return new Response(JSON.stringify({ error: "Invalid credentials" }), {
          status: 401,
          statusText: "Unauthorized",
        });
      }
    }
  
    // Optional: Für alle anderen "Routen" kannst du ein Default-Response zurückgeben
    return new Response(JSON.stringify({ error: "Route not found" }), {
      status: 404,
      statusText: "Not Found",
    });
  }
  