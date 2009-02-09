using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace GacetaParser
{
    public struct Votante {
        public int PageId;
        public string Fnac;
        public string Suplente;
        public string Eleccion;
        public string Entidad;
        public string Curul;
        public string Email;
        public string Circunscripcion;
        public string Cabecera;
        public List<Comision> Comisiones;
        public List<CvItem> Curricula;
    }

    public struct Comision {
        public string Id;
        public string Nombre;

        public Comision(string id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

    }

    public struct CvItem {
        public string Evento;
        public string Detalles;
        public string Periodo;

        public CvItem(string evento, string detalles, string periodo)
        {
            Evento = evento;
            Detalles = detalles;
            Periodo = periodo;
        }
    }

    class GetDiputados
    {
        public static string[] Ids = { "129", "268", "309", "358", "121", "794", "311", "112", "80", "828", "353", "1", "132", "332", "351", "290", "191", "219", "365", "298", "356", "20", "336", "315", "323", "509", "342", "607", "192", "287", "170", "173", "266", "215", "314", "85", "111", "23", "21", "498", "277", "237", "357", "275", "228", "560", "226", "286", "352", "334", "349", "169", "337", "292", "331", "151", "296", "269", "175", "246", "346", "820", "42", "214", "326", "348", "7", "211", "252", "808", "263", "186", "236", "16", "261", "231", "344", "55", "159", "355", "41", "318", "123", "120", "616", "282", "368", "321", "324", "178", "210", "227", "343", "217", "140", "87", "235", "302", "273", "89", "233", "305", "117", "105", "354", "330", "262", "88", "152", "139", "333", "90", "304", "5", "335", "232", "3", "310", "339", "267", "312", "22", "283", "114", "115", "819", "841", "124", "774", "118", "303", "77", "188", "109", "213", "248", "340", "347", "24", "146", "325", "329", "723", "243", "79", "301", "247", "10", "218", "366", "363", "122", "345", "271", "39", "1272", "110", "6", "81", "806", "295", "189", "647", "44", "11", "238", "18", "251", "307", "338", "234", "82", "361", "108", "179", "225", "4", "2", "278", "316", "76", "359", "249", "997", "119", "78", "362", "220", "113", "143", "182", "364", "86", "367", "313", "317", "8", "216", "84", "221", "83", "327", "165", "350", "195", "322", "193", "95", "62", "451", "94", "49", "172", "202", "138", "258", "270", "446", "484", "163", "53", "61", "155", "431", "92", "300", "93", "438", "59", "450", "50", "465", "206", "299", "256", "156", "208", "198", "144", "458", "447", "158", "259", "99", "72", "180", "56", "456", "47", "52", "71", "31", "200", "102", "473", "207", "428", "471", "184", "453", "291", "474", "13", "46", "157", "483", "131", "150", "433", "469", "201", "133", "142", "135", "443", "96", "257", "63", "440", "174", "177", "297", "54", "963", "65", "33", "439", "176", "30", "432", "426", "478", "64", "12", "435", "468", "48", "482", "68", "106", "444", "141", "137", "34", "281", "98", "136", "171", "130", "972", "164", "457", "162", "70", "476", "166", "57", "97", "254", "66", "470", "255", "966", "51", "91", "58", "154", "168", "67", "69", "167", "454", "455", "145", "19", "381", "380", "379", "126", "403", "244", "209", "369", "394", "125", "242", "370", "43", "73", "390", "376", "293", "190", "416", "383", "28", "413", "385", "148", "32", "265", "128", "687", "289", "45", "36", "27", "75", "402", "230", "222", "372", "540", "399", "260", "25", "391", "421", "264", "414", "17", "100", "704", "134", "74", "212", "406", "285", "29", "724", "397", "15", "389", "253", "38", "104", "280", "279", "395", "153", "14", "417", "241", "160", "382", "404", "422", "788", "26", "392", "496", "409", "408", "683", "197", "245", "239", "272", "407", "374", "101", "387", "419", "196", "681", "377", "401", "229", "378", "194", "185", "37", "425", "411", "750", "240", "103", "423", "161", "199", "373", "896", "400", "398", "415", "410", "412", "405", "893", "420", "924", "884", "388", "875", "871", "886", "418", "127", "437", "464", "430", "459", "479", "276", "149", "452", "1273", "449", "477", "442", "462", "284", "481", "205", "461", "427", "467", "434", "429", "485", "35", "448", "441", "203", "436", "475", "495", "494", "487", "489", "488", "493", "490", "491", "992", "499", "500", "486", "480", "460" };

        static string dir = @"C:\Users\jesus\Desktop\dips\";
        //static string tablaDiputado = @"C:\Users\jesus\Desktop\test.html";        

        public  static void GetDiputadoId(string file)
        {

            StreamReader sr = new StreamReader(file, System.Text.Encoding.Default);
            Regex r = new Regex("href=\"(.*?)\".*>(.*?)</a>");
            string s = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            MatchCollection mm = r.Matches(s);
            StreamWriter sw = new StreamWriter(file + "_res", false, System.Text.Encoding.Default);

            foreach (Match m in mm)
            {
                string val = string.Format("{0} - {1} ", m.Groups[1].Value, m.Groups[2].Value);
                Console.WriteLine(val);
                sw.WriteLine(val);
            }
            sw.Close();
            sw.Dispose();
        }

        public static void DownloadDiputadoInfo(string pageId)
        {


            string page = "http://sitl.diputados.gob.mx/curricula.php?dipt=" + pageId;
            Console.WriteLine("conectando a " + page);

            WebRequest req = WebRequest.Create(page);
            WebResponse response = req.GetResponse();
            Stream stream = response.GetResponseStream();
            byte[] buffer = new byte[1024];
            int dataLength = (int)response.ContentLength;

            Console.WriteLine("bajando " + pageId + "...");

            MemoryStream memStream = new MemoryStream();
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                {
                    Console.WriteLine("terminado!");
                    break;
                }
                else
                {
                    memStream.Write(buffer, 0, bytesRead);
                }
            }

            byte[] downloadedData = new byte[0];

            downloadedData = memStream.ToArray();

            stream.Close();
            memStream.Close();

            FileStream newFile = new FileStream(dir + pageId + ".html", FileMode.Create);
            newFile.Write(downloadedData, 0, downloadedData.Length);
            newFile.Close();


        }

        public static Votante GetDiputadoInfo(string id)
        {
            
            StreamReader sr = new StreamReader(dir + id + ".html", Encoding.Default, false);
            string s = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            List<Regex> regs = new List<Regex>();
            regs.Add(new Regex("<td.*class=\"textoNegro\"><.*class=\"Estilo67\">(.*?)</span></td>\n.*<td .* class=\"textoNegro\">(.*?)</td>"));
            regs.Add(new Regex("<span class=\"Estilo67\">(.*?):</span> (.*?)</td>"));
            regs.Add(new Regex("<span class=\"Estilo67\">(.*)</span><a href=\"mailto:(.*?)\""));
            regs.Add(new Regex("Suplente: (.*?)</span>"));
            regs.Add(new Regex("<a href=\"integrantes_de_comision.php\\?comt=(.*?)\" .* class=\"linkNegroSin\">(.*?)</a>"));
            regs.Add(new Regex("<tr>\n.*<td width=\"300\" class=\"textoNegro\">(.*?)</td>\n.*<td width=\"300\" class=\"textoNegro\">(.*?)</td>\n.*<td width=\"160\" class=\"textoNegro\">(.*?)</td>\n.*</tr>"));


            Votante d = new Votante();
            d.PageId = int.Parse(id);

            d.Comisiones = new List<Comision>();
            d.Curricula = new List<CvItem>();

            foreach (Regex r in regs)
            {
                MatchCollection mm = r.Matches(s);

                foreach (Match m in mm)
                {

                    switch (m.Groups.Count)
                    {
                        case 2:
                            d.Suplente = m.Groups[1].Value;
                            Console.WriteLine("Suplente: " + m.Groups[1].Value);
                            break;
                        case 3:
                            switch (m.Groups[1].Value)
                            {
                                case "Tipo de elección:":
                                    d.Eleccion = m.Groups[2].Value;
                                    break;
                                case "Entidad:":
                                    d.Entidad = m.Groups[2].Value;
                                    break;
                                case "Circunscripción: ":
                                    d.Circunscripcion = m.Groups[2].Value;
                                    break;
                                case "Cabecera:":
                                    d.Cabecera = m.Groups[2].Value;
                                    break;
                                case "Curul:":
                                    d.Curul = m.Groups[2].Value;
                                    break;
                                case "Fecha de Nacimiento":
                                    d.Fnac = m.Groups[2].Value;
                                    break;
                                case "Correo electrónico: ":
                                    d.Email = m.Groups[2].Value;
                                    break;
                                default:
                                    // comisiones
                                    d.Comisiones.Add(new Comision(m.Groups[1].Value, m.Groups[2].Value));                                  
                                    break;
                            }
                            break;
                        default:
                            // curricula
                            d.Curricula.Add(new CvItem(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value));                     
                            break;
                    }

                }

            }

            return d;
        }

    }
}
