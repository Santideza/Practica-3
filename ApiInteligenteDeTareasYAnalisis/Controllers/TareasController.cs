using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiInteligenteDeTareasYAnalisis.Data;
using ApiInteligenteDeTareasYAnalisis.Models;

namespace ApiInteligenteDeTareasYAnalisis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TareasController> _logger;

        public TareasController(AppDbContext context, ILogger<TareasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de todas las tareas
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
        {
            try
            {
                var tareas = await _context.Tareas.OrderByDescending(t => t.FechaCreacion).ToListAsync();
                return Ok(tareas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener las tareas");
            }
        }

        /// <summary>
        /// Obtiene una tarea específica por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);

                if (tarea == null)
                {
                    return NotFound(new { mensaje = $"La tarea con ID {id} no existe" });
                }

                return Ok(tarea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la tarea con ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener la tarea");
            }
        }

        /// <summary>
        /// Crea una nueva tarea
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Tarea>> PostTarea([FromBody] Tarea tarea)
        {
            try
            {
                // Validaciones personalizadas
                var erroresValidacion = ValidarTarea(tarea);
                if (erroresValidacion.Count > 0)
                {
                    return BadRequest(new { errores = erroresValidacion });
                }

                // Asignar fecha de creación
                tarea.FechaCreacion = DateTime.UtcNow;

                _context.Tareas.Add(tarea);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la tarea");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la tarea");
            }
        }

        /// <summary>
        /// Actualiza una tarea existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tarea>> PutTarea(int id, [FromBody] Tarea tareaActualizada)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);

                if (tarea == null)
                {
                    return NotFound(new { mensaje = $"La tarea con ID {id} no existe" });
                }

                // Validaciones personalizadas
                var erroresValidacion = ValidarTarea(tareaActualizada);
                if (erroresValidacion.Count > 0)
                {
                    return BadRequest(new { errores = erroresValidacion });
                }

                // Actualizar solo los campos permitidos
                tarea.Titulo = tareaActualizada.Titulo;
                tarea.Descripcion = tareaActualizada.Descripcion;
                tarea.Estado = tareaActualizada.Estado;
                tarea.Prioridad = tareaActualizada.Prioridad;
                tarea.FechaVencimiento = tareaActualizada.FechaVencimiento;

                _context.Tareas.Update(tarea);
                await _context.SaveChangesAsync();

                return Ok(tarea);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error al actualizar la tarea con ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar la tarea");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar la tarea con ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar la tarea");
            }
        }

        /// <summary>
        /// Elimina una tarea
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTarea(int id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);

                if (tarea == null)
                {
                    return NotFound(new { mensaje = $"La tarea con ID {id} no existe" });
                }

                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Tarea eliminada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la tarea con ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar la tarea");
            }
        }

        /// <summary>
        /// Valida los datos de una tarea
        /// </summary>
        private List<string> ValidarTarea(Tarea tarea)
        {
            var errores = new List<string>();

            // Validar Titulo
            if (string.IsNullOrWhiteSpace(tarea.Titulo))
            {
                errores.Add("El título es obligatorio y no puede estar vacío");
            }
            else if (tarea.Titulo.Length > 500)
            {
                errores.Add("El título no puede exceder 500 caracteres");
            }

            // Validar Estado
            var estadosValidos = new[] { "Pendiente", "EnProceso", "Completada" };
            if (!estadosValidos.Contains(tarea.Estado))
            {
                errores.Add("El estado debe ser uno de: Pendiente, EnProceso, Completada");
            }

            // Validar Prioridad
            var prioridadesValidas = new[] { "Baja", "Media", "Alta" };
            if (!prioridadesValidas.Contains(tarea.Prioridad))
            {
                errores.Add("La prioridad debe ser una de: Baja, Media, Alta");
            }

            // Validar FechaVencimiento
            if (tarea.FechaVencimiento < DateTime.UtcNow.Date)
            {
                errores.Add("La fecha de vencimiento no puede ser una fecha pasada");
            }

            return errores;
        }
    }
}
