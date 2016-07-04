using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ada.Base.BusinessObject
{
    public class TablaBDAttribute : System.Attribute
    {
        private String _nombreTablaBaseDatos;

        public String nombreTablaBaseDatos
        {
            get { return _nombreTablaBaseDatos; }
            set { _nombreTablaBaseDatos = value; }
        }

        public TablaBDAttribute(String NombreTablaBD)
        {
            nombreTablaBaseDatos = NombreTablaBD;
        }
    }

    public class DatosAttribute: System.Attribute
    {
        public DatosAttribute()
        {

        }

        
        private String _nombreCampo;

        public String nombreCampo
        {
            get { return _nombreCampo; }
            set { _nombreCampo = value; }
        }

        private Enumerados.TipoDatos _tipo;

        public Enumerados.TipoDatos Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        private Boolean _esPk;

        public Boolean esPk
        {
            get { return _esPk; }
            set { _esPk = value; }
        }

        private int _longitud;

        public int Longitud
        {
            get { return _longitud; }
            set { _longitud = value; }
        }

        private Enumerados.Operacion _operacion;

        public Enumerados.Operacion Operacion
        {
            get { return _operacion; }
            set { _operacion = value; }
        }

    }
}
