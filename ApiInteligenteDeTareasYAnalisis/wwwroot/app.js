const output = document.querySelector("#forecast-output");
const refresh = document.querySelector("#refresh");
const statusText = document.querySelector("#api-status");
const detailText = document.querySelector("#api-detail");
const statusDot = document.querySelector(".status-dot");

async function loadForecast() {
  output.textContent = "Cargando datos...";
  statusDot.className = "status-dot";
  statusText.textContent = "Comprobando API...";
  detailText.textContent = "Consultando /weatherforecast";

  try {
    const response = await fetch("/weatherforecast");

    if (!response.ok) {
      throw new Error(`HTTP ${response.status}`);
    }

    const data = await response.json();
    output.textContent = JSON.stringify(data, null, 2);
    statusDot.className = "status-dot ok";
    statusText.textContent = "API activa";
    detailText.textContent = `${data.length} registros recibidos`;
  } catch (error) {
    output.textContent = `No se pudo consultar /weatherforecast\n\n${error}`;
    statusDot.className = "status-dot error";
    statusText.textContent = "API sin respuesta";
    detailText.textContent = "Revisa la consola o vuelve a ejecutar dotnet run";
  }
}

refresh.addEventListener("click", loadForecast);
loadForecast();
