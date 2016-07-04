using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ada.Base.BusinessObject
{
    public class Enumerados
    {
        public enum TipoDatos
        {
            Bit,
            Char,
            Varchar,
            Int,
            Decimal,
            DateTime,
            Guid
        }

        public enum Operacion
        {
            Insertar,
            Actualizar,
            Consultar,
            Eliminar
        }

        public enum Estado
        {
            Nuevo,
            Viejo,
            Eliminado
        }
    }
}
