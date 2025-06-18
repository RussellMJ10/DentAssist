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
using DentAssistProyect.Models.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;

namespace DentAssistProyect.Controllers
{
    [Authorize(Roles = "Administrador,Odontologo")]
    public class PasoPlanesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PasoPlanesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PasoPlanes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PasosPlan.Include(p => p.PlanTratamiento).Include(p => p.Tratamiento);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PasoPlanes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasoPlan = await _context.PasosPlan
                .Include(p => p.PlanTratamiento)
                    .ThenInclude(pt => pt.Paciente)
                .Include(p => p.PlanTratamiento)
                    .ThenInclude(pt => pt.Odontologo)
                .Include(p => p.Tratamiento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pasoPlan == null)
            {
                return NotFound();
            }

            return View(pasoPlan);
        }


        // GET: PasoPlanes/Create
        public IActionResult Create()
        {
            ViewData["PlanTratamientoId"] = new SelectList(_context.PlanesTratamiento, "Id", "Id");
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre");
            ViewData["Estado"] = new SelectList(Enum.GetValues(typeof(EstadoPaso)));
            return View();
        }

        // POST: PasoPlanes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Orden,Descripcion,FechaEstimada,Estado,PlanTratamientoId,TratamientoId")] PasoPlan pasoPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pasoPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanTratamientoId"] = new SelectList(_context.PlanesTratamiento, "Id", "Id", pasoPlan.PlanTratamientoId);
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre", pasoPlan.TratamientoId);
            ViewData["Estado"] = new SelectList(Enum.GetValues(typeof(EstadoPaso)));
            return View(pasoPlan);
        }

        // GET: PasoPlanes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasoPlan = await _context.PasosPlan.FindAsync(id);
            if (pasoPlan == null)
            {
                return NotFound();
            }
            ViewData["PlanTratamientoId"] = new SelectList(_context.PlanesTratamiento, "Id", "Id", pasoPlan.PlanTratamientoId);
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre", pasoPlan.TratamientoId);
            ViewData["Estado"] = new SelectList(Enum.GetValues(typeof(EstadoPaso)));
            return View(pasoPlan);
        }

        // POST: PasoPlanes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Orden,Descripcion,FechaEstimada,Estado,PlanTratamientoId,TratamientoId")] PasoPlan pasoPlan)
        {
            if (id != pasoPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pasoPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasoPlanExists(pasoPlan.Id))
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
            ViewData["PlanTratamientoId"] = new SelectList(_context.PlanesTratamiento, "Id", "Id", pasoPlan.PlanTratamientoId);
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre", pasoPlan.TratamientoId);
            return View(pasoPlan);
        }

        // GET: PasoPlanes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasoPlan = await _context.PasosPlan
                .Include(p => p.PlanTratamiento)
                .Include(p => p.Tratamiento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pasoPlan == null)
            {
                return NotFound();
            }

            return View(pasoPlan);
        }

        // POST: PasoPlanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pasoPlan = await _context.PasosPlan.FindAsync(id);
            if (pasoPlan != null)
            {
                _context.PasosPlan.Remove(pasoPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasoPlanExists(int id)
        {
            return _context.PasosPlan.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> ExportarPdf(int id)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var pasoPlan = await _context.PasosPlan
                .Include(p => p.PlanTratamiento)
                    .ThenInclude(p => p.Paciente)
                .Include(p => p.PlanTratamiento)
                    .ThenInclude(p => p.Odontologo)
                .Include(p => p.Tratamiento)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pasoPlan == null)
                return NotFound();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Header()
                        .Text($"Detalle Paso del Plan de Tratamiento - Numero #{pasoPlan.Orden}")
                        .FontSize(18)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .PaddingVertical(10)
                        .Column(column =>
                        {
                            column.Item().Text($"Tratamiento: {pasoPlan.Tratamiento?.Nombre ?? "N/A"}");
                            column.Item().Text($"Descripción: {pasoPlan.Descripcion ?? "Sin descripción"}");
                            column.Item().Text($"Fecha estimada: {(pasoPlan.FechaEstimada.HasValue ? pasoPlan.FechaEstimada.Value.ToString("dd/MM/yyyy") : "No definida")}");
                            column.Item().Text($"Estado: {pasoPlan.Estado}");

                            column.Item().PaddingVertical(5).LineHorizontal(1);

                            column.Item().Text("Información del Plan de Tratamiento").Bold().FontSize(14);
                            column.Item().Text($"Fecha de creación: {pasoPlan.PlanTratamiento.FechaCreacion:dd/MM/yyyy}");
                            column.Item().Text($"Observaciones: {pasoPlan.PlanTratamiento.Observaciones ?? "Sin observaciones"}");

                            column.Item().PaddingVertical(5).LineHorizontal(1);

                            column.Item().Text("Paciente").Bold();
                            column.Item().Text($"Nombre: {pasoPlan.PlanTratamiento.Paciente.Nombre}");
                            column.Item().Text($"RUT: {pasoPlan.PlanTratamiento.Paciente.Rut}");
                            column.Item().Text($"Teléfono: {pasoPlan.PlanTratamiento.Paciente.Telefono}");
                            column.Item().Text($"Email: {pasoPlan.PlanTratamiento.Paciente.Email}");
                            column.Item().Text($"Dirección: {pasoPlan.PlanTratamiento.Paciente.Direccion}");

                            column.Item().PaddingVertical(5).LineHorizontal(1);

                            column.Item().Text("Odontólogo").Bold();
                            column.Item().Text($"Matrícula: {pasoPlan.PlanTratamiento.Odontologo.Matricula}");
                            column.Item().Text($"Nombre Completo: {pasoPlan.PlanTratamiento.Odontologo.Nombre}");
                            column.Item().Text($"Especialidad: {pasoPlan.PlanTratamiento.Odontologo.Especialidad}");
                            column.Item().Text($"Email: {pasoPlan.PlanTratamiento.Odontologo.Email}");
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"PasoPlan_{pasoPlan.Id}.pdf");
        }
    }
}