using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GacetaParser
{
    public class Voto
    {

        public string Indice;
        public string Votante;
        public Partido Partido;
        public Posicion Posicion;

        // indice: votante partido, partido, posicion
        public Regex RegVoto = new Regex("(.*):(.*) (.*),(.*),(.*)");
        
        // solo para el voto 60
        public Regex RegVoto60 = new Regex("(.*),(.*),(.*),(.*),(.*)");

        public Voto(string content, bool is60)
        {
            if (string.IsNullOrEmpty(content))
                return;
            List<string> div;

            if (is60)
            {
                div = Utils.LookValueGroup(content, RegVoto60, new int[] { 1, 3, 4, 5 });
            }
            else
            {
                div = Utils.LookValueGroup(content, RegVoto, new int[] { 1, 2, 4, 5 });
            }


            if (div.Count > 3)
            {
                Indice = div[0];
                Votante = div[1].Trim();

                switch (div[2])
                {                   
                    case "PRI":
                        Partido = Partido.PRI;
                        break;
                    case "PAN":
                        Partido = Partido.PAN;
                        break;
                    case "PRD":
                        Partido = Partido.PRD;
                        break;
                    case "PVEM":
                        Partido = Partido.PVEM;
                        break;
                    case "PT":
                        Partido = Partido.PT;
                        break;
                    case "PSN":
                        Partido = Partido.PSN;
                        break;
                    case "CONV":
                        Partido = Partido.CONV;
                        break;
                    case "PAS":
                        Partido = Partido.PAS;
                        break;
                    default:                        
                        Partido = Partido.IND;
                        break;
                }

                switch (div[3])
                {
                    case "0":
                        Posicion = Posicion.Ausente;
                        break;
                    case "1":
                        Posicion = Posicion.Quorum;
                        break;
                    case "2":
                        Posicion = Posicion.Favor;
                        break;
                    case "3":
                        Posicion = Posicion.Abstencion;
                        break;
                    case "4":
                        Posicion = Posicion.Contra;
                        break;
                    case "5":
                        Posicion = Posicion.Total;
                        break;
                    default:
                        Posicion = Posicion.Invalido;
                        break;
                }



            }
            
        }

    }
}
