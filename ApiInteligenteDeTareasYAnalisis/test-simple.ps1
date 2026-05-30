#!/usr/bin/env pwsh

$baseUrl = "http://localhost:5157/api/tareas"

Write-Host "=== PRUEBAS DE LA API ===" -ForegroundColor Green

# 1. Obtener todas las tareas
Write-Host "`n1. GET /api/tareas" -ForegroundColor Cyan
(Invoke-WebRequest -Uri $baseUrl -Method Get -UseBasicParsing).Content | ConvertFrom-Json | Format-Table -AutoSize

# 2. Crear una nueva tarea
Write-Host "`n2. POST /api/tareas" -ForegroundColor Cyan
$newTask = @{
    titulo = "Escribir tests"
    descripcion = "Unit tests para la API"
    estado = "Pendiente"
    prioridad = "Media"
    fechaVencimiento = "2026-06-30T00:00:00Z"
} | ConvertTo-Json

$created = (Invoke-WebRequest -Uri $baseUrl -Method Post -ContentType "application/json" -Body $newTask -UseBasicParsing).Content | ConvertFrom-Json
Write-Host "Tarea creada: ID=$($created.id), Título=$($created.titulo)"

# 3. Obtener tarea por ID
Write-Host "`n3. GET /api/tareas/1" -ForegroundColor Cyan
(Invoke-WebRequest -Uri "$baseUrl/1" -Method Get -UseBasicParsing).Content | ConvertFrom-Json | Format-List

# 4. Actualizar tarea
Write-Host "`n4. PUT /api/tareas/1" -ForegroundColor Cyan
$updateTask = @{
    titulo = "Implementar autenticación (Actualizada)"
    descripcion = "Agregar JWT al proyecto"
    estado = "EnProceso"
    prioridad = "Alta"
    fechaVencimiento = "2026-06-15T00:00:00Z"
} | ConvertTo-Json

$updated = (Invoke-WebRequest -Uri "$baseUrl/1" -Method Put -ContentType "application/json" -Body $updateTask -UseBasicParsing).Content | ConvertFrom-Json
Write-Host "Tarea actualizada: Estado=$($updated.estado)"

# 5. Listar todas las tareas de nuevo
Write-Host "`n5. GET /api/tareas (Final)" -ForegroundColor Cyan
(Invoke-WebRequest -Uri $baseUrl -Method Get -UseBasicParsing).Content | ConvertFrom-Json | Format-Table -AutoSize

Write-Host "`n[OK] Pruebas completadas" -ForegroundColor Green
