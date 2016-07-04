using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ada.Base.BusinessObject
{
    [DatosAttribute()]
    public class EmpresaBO: BaseBO
    {
        private Guid _unico;

        [DatosAttribute(esPk =true, nombreCampo ="Unico", Tipo = Enumerados.TipoDatos.Guid)]
        public Guid Unico
        {
            get { return _unico; }
            set { _unico = value; }
        }


        private String _razonSocial;

        [DatosAttribute(Longitud =50, nombreCampo = "RazonSocial", Tipo = Enumerados.TipoDatos.Varchar)]
        public String RazonSocial
        {
            get { return _razonSocial; }
            set { _razonSocial = value; }
        }

        private String _rif;

        [DatosAttribute(Longitud =20, nombreCampo ="Rif", Tipo = Enumerados.TipoDatos.Varchar)]
        public String Rif
        {
            get { return _rif; }
            set { _rif = value; }
        }

    }
}
