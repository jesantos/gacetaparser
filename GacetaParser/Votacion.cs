using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GacetaParser
{
    public enum Partido
    {
        TODOS,
        PRI,
        PAN,
        PRD,
        PVEM,
        PT,
        PSN,
        CONV,
        PAS
    }

    public enum Posicion
    {
        Quorum,
        Favor,
        Abstencion,
        Contra,
        Total,
        Ausente
    }

    public class Votacion
    {
        public string Legislatura;
        public string Tipo;
        public string VotacionNum;
        public string Fecha;
        public string Descripcion;
        public List<Voto> Votos;

        public Regex RegFecha = new Regex("VOTETAKEN:(.*)");
        public Regex RegDesc = new Regex("PROPOSEDBY:(.*)");

        public Votacion(string filePath)
        {
            if (!File.Exists(filePath))
                return;
            try
            {

                TextReader tr = new StreamReader(filePath);
                Votos = new List<Voto>();

                string line = tr.ReadLine();
                int pos = 0;

                while (!string.IsNullOrEmpty(line))
                {
                    if (line.Contains("PROPOSEDBY"))
                    {
                        Descripcion = Utils.LookValue(line, RegDesc, 1);
                    }
                    else if (line.Contains("VOTETAKEN"))
                    {
                        Fecha = Utils.LookValue(line, RegFecha, 1);
                    }
                    else
                    {
                        if (line.Length > 5)
                        {
                            if (line[1] == ':' || line[2] == ':' || line[3] == ':')
                                Votos.Add(new Voto(line));
                        }
                    }

                    line = tr.ReadLine();
                }

                tr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }            

        }

        public override string ToString()
        {
            if(Votos == null)
                return base.ToString();
            
            string ret = "";

            foreach (Voto v in Votos)
            {
                ret += "\"" + Legislatura + "\",";
                ret += "\"" + Tipo + "\",";
                ret += "\"" + VotacionNum + "\",";
                ret += "\"" + Descripcion + "\",";
                ret += "\"" + Fecha + "\",";
                ret += "\"" + v.Indice + "\",";
                ret += "\"" + v.Partido + "\",";
                ret += "\"" + v.Posicion + "\",";
                ret += "\"" + v.Votante + "\",";
                ret += "\\r\\n";
            }

            return ret;

        }

        public bool Save(string path)
        {
            if (Votos == null)
                return false;

            bool append = true;

            string ret = "";

            if (!File.Exists(path))
            {
                append = false;

                ret += "\"Legislatura\",";
                ret += "\"Tipo\",";
                ret += "\"VotacionNum\",";
                ret += "\"Descripcion\",";
                ret += "\"Fecha\",";
                ret += "\"Indice\",";
                ret += "\"Partido\",";
                ret += "\"Posicion\",";
                ret += "\"Votante\",";                
            }

            TextWriter tw = new StreamWriter(path, append);

            if(!append)
                tw.WriteLine(ret);
            

            foreach (Voto v in Votos)
            {
                ret="";
 
                ret += "\"" + Legislatura + "\",";
                ret += "\"" + Tipo + "\",";
                ret += "\"" + VotacionNum + "\",";
                ret += "\"" + Descripcion + "\",";
                ret += "\"" + Fecha + "\",";
                ret += "\"" + v.Indice + "\",";
                ret += "\"" + v.Partido + "\",";
                ret += "\"" + v.Posicion + "\",";
                ret += "\"" + v.Votante + "\",";
                tw.WriteLine(ret);                
            }

            tw.Close();
            return true;

        }

    }
}
