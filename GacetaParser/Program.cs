using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GacetaParser
{    

    class Program
    {

        static void Main(string[] args)
        {
            string dirSave = @"C:\Users\jesus\Documents\Projects\GacetaParser\files\";
            string dirLook = @"C:\Users\jesus\Documents\Projects\GacetaParser\files\voto58\";
            string legis = "58";
            string tipo = "ordinaria 11";
            dirLook += @"ordi11";


            if (args.Length > 0)
            {
                dirLook = args[0];
                dirSave = args[1];
                legis = args[2];
                tipo = args[3];
            }
            
            string[] dirFiles = Directory.GetFiles(dirLook, "*.txt");

            Console.WriteLine("Trabajando...");

            for (int i = 0; i < dirFiles.Length; i++)
            {
                Console.WriteLine(dirFiles[i]);
                Votacion vt = new Votacion(dirFiles[i]);
                vt.Legislatura = legis;
                vt.Tipo = tipo;
                vt.VotacionNum = dirFiles[i];

                if (vt.Save(dirSave + "result.csv"))
                {
                    Console.WriteLine("Listo!");
                }

            }
            Console.ReadLine();

        }
    }
}
