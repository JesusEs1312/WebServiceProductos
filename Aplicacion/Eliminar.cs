using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion
{
    public class Eliminar
    {
        public class Ejecutar : IRequest
        {
            public Guid id {get; set;}
        }

        public class Manejador : IRequestHandler<Ejecutar>
        {
            private readonly Context context;

            public Manejador(Context context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                //Encontrar producto
                var producto = await this.context.Producto.FindAsync(request.id);
                //Excepcion realizada con tu Middleware
                if(producto == null) throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el producto"});
                //Eliminar producto
                this.context.Remove(producto);
                //Guardar los cambios en la base de datos
                var valor = await this.context.SaveChangesAsync();
                if (valor > 0) return Unit.Value;
                throw new System.Exception("No se pudo eliminar el producto");
            }
        }
    }
}