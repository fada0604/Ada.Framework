using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ada.Base.BusinessObject;

namespace Ada.Base.Data
{
    public class EmpresaD: BaseD<EmpresaD, EmpresaBO>
    {
        public EmpresaD()
        {
            nombreSPInsertar = "spInsertarEmpresa";
            nombreSPActualizar = "spActualizarEmpresa";
            nombreSPConsultar = "spConsultarEmpresa";
            nombreSPEliminar = "spEliminarEmpresa";
            nombreSPListar = "spListarEmpresas";
        }

        public override int Insertar(EmpresaD objeto, EmpresaBO definicion)
        {
            return base.Insertar(objeto, definicion);
        }

        public override int Actualizar(EmpresaD objeto, EmpresaBO definicion)
        {
            return base.Actualizar(objeto, definicion);
        }

        public override EmpresaBO Consultar(EmpresaD objeto, ref EmpresaBO definicion)
        {
            return base.Consultar(objeto, ref definicion);
        }

        public override List<EmpresaBO> Listar(EmpresaD objeto, EmpresaBO definicion)
        {
            return base.Listar(objeto, definicion);
        }
    }
}
