using DentAssistProyect.Data;
using DentAssistProyect.Models.Entities;
using DentAssistProyect.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentAssistProyect.Controllers
{
    [Authorize(Roles = "Administrador,Odontologo,Recepcionista")]

    public class TurnoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TurnoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Turnoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Turnos.Include(t => t.Odontologo).Include(t => t.Paciente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Turnoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turnoes/Create
        public IActionResult Create()
        {
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Matricula");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre");
            ViewData["Estado"] = new SelectList(Enum.GetValues(typeof(EstadoTurno)));
            return View();
        }

        // POST: Turnoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaHora,DuracionMinutos,Estado,PacienteId,OdontologoId")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Matricula", turno.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", turno.PacienteId);
            ViewData["Estado"] = new SelectList(Enum.GetValues(typeof(EstadoTurno)));
            return View(turno);
        }

        // GET: Turnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Matricula", turno.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", turno.PacienteId);
            ViewData["Estado"] = new SelectList(Enum.GetValues(typeof(EstadoTurno)));
            return View(turno);
        }

        // POST: Turnoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaHora,DuracionMinutos,Estado,PacienteId,OdontologoId")] Turno turno)
        {
            if (id != turno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Matricula", turno.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", turno.PacienteId);
            ViewData["Estado"] = new SelectList(Enum.GetValues(typeof(EstadoTurno)));
            return View(turno);
        }

        // GET: Turnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.Id == id);
        }
    }
}
