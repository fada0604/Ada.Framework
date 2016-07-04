using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ada.Base.BusinessObject;
using Ada.Base.Data;

namespace Ada.Base.Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            EmpresaD obj = new EmpresaD();
            EmpresaBO def = new EmpresaBO();
            def.RazonSocial = "prueba2";
            def.Rif = "J123454581";
            obj.Consultar(obj, ref def);
            Console.ReadLine(); 
        }
    }
}
