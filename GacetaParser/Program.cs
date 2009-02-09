using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace GacetaParser
{    

    class Program
    {

        static string dirSave = @"C:\Users\jesus\Documents\Projects\GacetaParser\files\Resultados";
        static string dirLook = @"C:\Users\jesus\Documents\Projects\GacetaParser\files\legislaturas\57";
        static string dir = @"C:\Users\jesus\Desktop\dips\";
        

        static void Main(string[] args)
        {
            
            //appendLegislaturas();
            //parseLegislatura(args);
            //GetDiputados.DownloadDiputadoInfo("5");
            //saveDiputadosInfo();
            //saveDiputadosComisiones();
            saveDiputadosCV();

            Console.Read();
        }

        private static void appendLegislaturas()
        { 
            string[] files = Directory.GetFiles(dirSave, "*.csv");

            string result = dirSave + "\\VotoTodos.csv";

            if (files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                    TextReader tr = new StreamReader(file);
                    string s = tr.ReadToEnd();
                    tr.Close();
                    
                    TextWriter tw = new StreamWriter(result, true);
                    tw.Write(s);
                    tw.Close();
                }
            }

            Console.ReadKey();
        }

        private static void parseLegislatura(string[] args)
        {
            /*if (args.Length > 0)
            {
                dirLook = args[0];
                dirSave = args[1];

                if (args.Length == 3)
                {
                    legislatura = args[2];
                }
            }

            /* antes los directorios representaban las sesiones. las sesiones ahora se eliminan, intentamos usar solo la fecha
             * 
             * string[] sesiones = Directory.GetDirectories(dirLook);

            if (sesiones.Length > 0)
            {
                foreach (string dir in sesiones)
                {
                    Console.WriteLine(dir);
                    ParseSesion(dir);
                }
            }

             * */

            parseSesion(dirLook);

            Console.ReadLine();
        }

        private static void parseSesion(string dir)
        {                        
            string sesion = "";

            Regex regSesion = new Regex(@"^(.*)\\(.*)$");
            Regex regVotNum = new Regex(@"^(.*)\\(.*).txt$");

            Match m = regSesion.Match(dir);
            string legislatura = "";

            if (m != null && m.Groups.Count>1)
            {
                legislatura = m.Groups[2].Value;
            }

            Console.WriteLine("Trabajando votos");

            string[] dirFiles = Directory.GetFiles(dir, "*.txt");

            for (int i = 0; i < dirFiles.Length; i++)
            {
                Console.WriteLine(dirFiles[i]);

                Votacion vt ;

                if (legislatura == "60")
                    vt = new Votacion60(dirFiles[i]);
                else
                    vt = new Votacion(dirFiles[i]);

                vt.Legislatura = legislatura;
                //vt.Sesion = sesion;
                vt.VotacionNum = dirFiles[i];
                m = regVotNum.Match(dirFiles[i]);

                if (m != null && m.Groups.Count > 1)
                {
                    vt.VotacionNum = m.Groups[2].Value;
                }
                
                if (vt.Save(Program.dirSave + "\\voto_"+ legislatura +"_"+sesion+".csv"))
                {
                    Console.WriteLine("Listo!");
                }

            }            
        }

        private static void saveDiputadosInfo()
        {
            StreamWriter sw = new StreamWriter(dir + "vals.csv", false, System.Text.Encoding.Default);
            sw.WriteLine("Cabecera,Circunscripcion,Curul,Eleccion,Email,Entidad,Fnac,PageId,Suplente");
            foreach (string s in GetDiputados.Ids)
            {
                try
                {
                    Votante v = GetDiputados.GetDiputadoInfo(s);
                    string row = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\"", v.Cabecera, v.Circunscripcion, v.Curul, v.Eleccion, v.Email, v.Entidad, v.Fnac, v.PageId, v.Suplente);
                    sw.WriteLine(row);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    sw.WriteLine(e.ToString());
                }
            }

            sw.Close();
            sw.Dispose();



        }

        private static void saveDiputadosComisiones()
        {
            StreamWriter sw = new StreamWriter(dir + "vals.csv", false, System.Text.Encoding.Default);
            sw.WriteLine("VotanteId,Comision,ComisionId");
            foreach (string s in GetDiputados.Ids)
            {
                try
                {
                    Votante v = GetDiputados.GetDiputadoInfo(s);

                    foreach (Comision c in v.Comisiones)
                    {
                        string row = string.Format("\"{0}\",\"{1}\",\"{2}\"", v.PageId, c.Nombre, c.Id);
                        sw.WriteLine(row);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    sw.WriteLine(e.ToString());
                }
            }

            sw.Close();
            sw.Dispose();



        }

        private static void saveDiputadosCV()
        {
            StreamWriter sw = new StreamWriter(dir + "vals.csv", false, System.Text.Encoding.Default);
            sw.WriteLine("VotanteId,Evento,Detalles,Periodo");
            foreach (string s in GetDiputados.Ids)
            {
                try
                {
                    Votante v = GetDiputados.GetDiputadoInfo(s);

                    foreach (CvItem c in v.Curricula)
                    {
                        string row = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", v.PageId, c.Evento.Replace(",",""), c.Detalles.Replace(",",""), c.Periodo);
                        sw.WriteLine(row);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    sw.WriteLine(e.ToString());
                }
            }

            sw.Close();
            sw.Dispose();



        }


    }
}
