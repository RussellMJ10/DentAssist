using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentAssistProyect.Data;
using DentAssistProyect.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient; // Añade este using para SqlException

namespace DentAssistProyect.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        [Authorize(Roles = "Administrador,Recepcionista,Odontologo")]
        public async Task<IActionResult> Index()
        {
            // Mostrar mensajes temporales
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }

            return View(await _context.Pacientes.ToListAsync());
        }

        // GET: Pacientes/Details/5
        [Authorize(Roles = "Administrador,Recepcionista,Odontologo")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null) return NotFound();

            return View(paciente);
        }

        // GET: Pacientes/Create
        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Recepcionista")]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Rut,Telefono,Email,Direccion")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Paciente creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        [Authorize(Roles = "Administrador,Recepcionista")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Recepcionista")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Rut,Telefono,Email,Direccion")] Paciente paciente)
        {
            if (id != paciente.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Paciente actualizado correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        [Authorize(Roles = "Administrador,Recepcionista")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.Turnos)
                .Include(p => p.PlanesTratamiento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (paciente == null) return NotFound();

            // Verificar si tiene relaciones
            if (paciente.Turnos.Any() || paciente.PlanesTratamiento.Any())
            {
                ViewBag.HasRelations = true;
                ViewBag.RelationCount = paciente.Turnos.Count() + paciente.PlanesTratamiento.Count();
            }
            else
            {
                ViewBag.HasRelations = false;
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Recepcionista")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var paciente = await _context.Pacientes
                    .Include(p => p.Turnos)
                    .Include(p => p.PlanesTratamiento)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (paciente == null)
                {
                    TempData["ErrorMessage"] = "Paciente no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                // Verificación adicional por si acaso
                if (paciente.Turnos.Any() || paciente.PlanesTratamiento.Any())
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el paciente porque tiene turnos o planes de tratamiento asociados.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Paciente eliminado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Verifica si es un error de restricción de clave foránea
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 547 || sqlEx.Message.Contains("FK_")))
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el paciente porque está siendo utilizado en otros registros (turnos o planes de tratamiento).";
                    return RedirectToAction(nameof(Index));
                }

                // Para otros errores de base de datos
                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el paciente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Para cualquier otro error
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al eliminar el paciente.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id == id);
        }
    }
}