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

        static void Main(string[] args)
        {
            
            //AppendLegislaturas();
            ParseLegislatura(args);

        }

        private static void AppendLegislaturas()
        { 
            string[] files = Directory.GetFiles(dirSave, "*.csv");

            string result = dirSave + "\\megaVoto.txt";

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

        private static void ParseLegislatura(string[] args)
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
    }
}
