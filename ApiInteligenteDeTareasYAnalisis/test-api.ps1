$ErrorActionPreference = 'Continue'
$ProgressPreference = 'SilentlyContinue'

# Crear una tarea
$body = @{
    titulo = "Implementar autenticación"
    descripcion = "Agregar JWT al proyecto"
    estado = "Pendiente"
    prioridad = "Alta"
    fechaVencimiento = "2026-06-15T00:00:00Z"
} | ConvertTo-Json

Write-Host "Enviando POST para crear tarea..."
Write-Host "Body: $body"

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5157/api/tareas" `
        -Method Post `
        -ContentType "application/json" `
        -Body $body `
        -UseBasicParsing
    Write-Host "Status: $($response.StatusCode)"
    Write-Host "Response: $($response.Content)"
} catch {
    Write-Host "Error: $($_.Exception.Message)"
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response Body: $responseBody"
    }
}
