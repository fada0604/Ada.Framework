using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ada.Base.BusinessObject
{
    public class ExcepcionesPersonalizadas: Exception
    {
        public ExcepcionesPersonalizadas(string mensaje): base(mensaje){ }
        public ExcepcionesPersonalizadas(string mensaje, Exception detalleExcepcion): base(mensaje, detalleExcepcion){ }
        public ExcepcionesPersonalizadas(string mensaje, Exception detalleExcepcion, string detalle) : base(mensaje, detalleExcepcion) { }
    }
}
