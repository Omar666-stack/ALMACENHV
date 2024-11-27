$controllersPath = "ALMACENHV/Controllers"
$controllers = @(
    "ApuntesController",
    "CargosController",
    "EventosController",
    "ProductosController",
    "ProveedoresController",
    "RegistroEntradasController",
    "RegistroIngresoDetallesController",
    "RegistroIngresoFotosController",
    "RegistroSalidaDetallesController",
    "RegistroSalidasController",
    "RolesController",
    "SeccionesController",
    "UbicacionesController",
    "UsuariosController"
)

foreach ($controller in $controllers) {
    $templateContent = @"
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Controllers
{
    public class $controller : BaseController
    {
        private readonly TuDbContext _context;
        private readonly ILogger<$controller> _logger;

        public $controller(TuDbContext context, ILogger<$controller> logger)
            : base(logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/$($controller.Replace("Controller", ""))
        [HttpGet]
        public async Task<ActionResult<IEnumerable<${controller.Replace("Controller", "").TrimEnd('s')}>>> Get$($controller.Replace("Controller", ""))()
        {
            return await HandleDbOperation(async () =>
            {
                var items = await _context.$($controller.Replace("Controller", "")).ToListAsync();
                if (!items.Any())
                {
                    _logger.LogInformation("No se encontraron registros");
                    return new List<${controller.Replace("Controller", "").TrimEnd('s')}>();
                }
                return items;
            });
        }

        // GET: api/$($controller.Replace("Controller", ""))/5
        [HttpGet("{id}")]
        public async Task<ActionResult<${controller.Replace("Controller", "").TrimEnd('s')}>> Get${controller.Replace("Controller", "").TrimEnd('s')}(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.$($controller.Replace("Controller", "")).FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Registro no encontrado: {Id}", id);
                    return null;
                }
                return item;
            });
        }

        // PUT: api/$($controller.Replace("Controller", ""))/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put${controller.Replace("Controller", "").TrimEnd('s')}(int id, ${controller.Replace("Controller", "").TrimEnd('s')} item)
        {
            if (id != item.${controller.Replace("Controller", "").TrimEnd('s')}ID)
            {
                return BadRequest("El ID no coincide con el registro a actualizar");
            }

            return await HandleDbOperation(async () =>
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // POST: api/$($controller.Replace("Controller", ""))
        [HttpPost]
        public async Task<ActionResult<${controller.Replace("Controller", "").TrimEnd('s')}>> Post${controller.Replace("Controller", "").TrimEnd('s')}(${controller.Replace("Controller", "").TrimEnd('s')} item)
        {
            return await HandleDbOperation(async () =>
            {
                _context.$($controller.Replace("Controller", "")).Add(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // DELETE: api/$($controller.Replace("Controller", ""))/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete${controller.Replace("Controller", "").TrimEnd('s')}(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.$($controller.Replace("Controller", "")).FindAsync(id);
                if (item == null)
                {
                    return null;
                }

                _context.$($controller.Replace("Controller", "")).Remove(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }
    }
}
"@

    $controllerPath = Join-Path $controllersPath "$controller.cs"
    $templateContent | Out-File -FilePath $controllerPath -Encoding UTF8
    Write-Host "Updated $controller"
}

Write-Host "All controllers have been updated!"
