using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
   public class MaterialService : IMaterialService
    {
        private readonly InventarioContext _context;

        public MaterialService(InventarioContext context)
        {
            _context = context;
        }

        public async Task<MaterialResponse> GetMateriales(string userRole, string localAsignado, string nombreUsuario, MaterialRequest request)
        {
            IQueryable<Material> query = _context.Materiales.AsNoTracking();

            string mensaje = "";

            // Filtrar por local si es inventariador
            if (userRole == "Inventariador" && !string.IsNullOrEmpty(localAsignado))
            {
                query = query.Where(m => m.Local == localAsignado);
                mensaje = $"Mostrando materiales del local asignado: {localAsignado}";
            }
            else if (userRole == "Administrador")
            {
                // Administrador ve todo, pero puede filtrar por local si se especifica
                if (!string.IsNullOrEmpty(request.Local))
                {
                    query = query.Where(m => m.Local == request.Local);
                    mensaje = $"Mostrando materiales del local: {request.Local}";
                }
                else
                {
                    mensaje = "Mostrando todos los materiales de todos los locales";
                }
            }

            // Obtener el total de registros
            var totalCount = await query.CountAsync();

            // Aplicar paginación - SELECCIONAR TODOS LOS CAMPOS EXPLÍCITAMENTE
            var materiales = await query
                .OrderBy(m => m.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(m => new Material // Proyección explícita para asegurar todos los campos
                {
                    Id = m.Id,
                    MaterialCod = m.MaterialCod,
                    Descripcion = m.Descripcion,
                    Almacen = m.Almacen,
                    Lote = m.Lote,
                    Cantidad = m.Cantidad,
                    Conteo = m.Conteo,
                    Reconteo = m.Reconteo,
                    FecReg = m.FecReg,
                    Obs = m.Obs,
                    Local = m.Local,
                    Umb = m.Umb,
                    Parihuela = m.Parihuela,
                    Ubicacion = m.Ubicacion,
                    Fec = m.Fec,
                    Cta = m.Cta,
                    Usuario = m.Usuario,
                    FecSys = m.FecSys,
                    Estado = m.Estado
                })
                .ToListAsync();

            return new MaterialResponse
            {
                Materiales = materiales,
                TotalCount = totalCount,
                Page = request.Page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                Usuario = nombreUsuario,
                Rol = userRole,
                LocalAsignado = localAsignado ?? "",
                Mensaje = mensaje
            };
        }

        public IQueryable<string> GetLocalesQuery()
        {
            return _context.Materiales
                .Where(m => m.Local != null && m.Local != "")
                .Select(m => m.Local)
                .Distinct();
        }
    }
}
