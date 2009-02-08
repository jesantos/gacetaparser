using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GacetaParser
{
    public class Votacion60 : Votacion
    {

        public Votacion60(string filePath)            
        {
            base.RegFecha = new Regex("FECHA DEL VOTO:(.*)");
            base.RegDesc = new Regex("PROPUESTA:(.*)");
            base.PropHeader = "PROPUESTA";
            base.DateHeader = "FECHA DEL VOTO";
            base.VotoSeparator = ',';
            base.Is60 = true;

            base.Parse(filePath);

        }


    }
}
