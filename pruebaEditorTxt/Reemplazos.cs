﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace pruebaEditorTxt
{
    static class Reemplazos
    {
        public static StringBuilder BuscarReemplazar(StringBuilder txt, string md, string html)
        {
            // Busca una etiqueta Markdown y la sustituye por una html, p.ej. *** -> <hr>

            if (txt.ToString().Contains(md))
            {
                txt = txt.Replace(md, html);
            }

            return txt;
        }


        public static StringBuilder BuscarReemplazar(StringBuilder txt, string md, string htmlInicio, string htmlCierre)
        {
            // Busca una etiqueta Markdown y la sustituye por una html de inicio y una de cierre, p.ej. # -> <h1></h1>
            // Usaremos este método para tags de Markdown que afectan a UNA SOLA LÍNEA, como los encabezados

            if (txt.ToString().Contains(md))
            {
                List<string> lineas = new List<string>(Regex.Split(txt.ToString(), Environment.NewLine));
                StringBuilder sbLinea = new StringBuilder();  // Aquí iremos guardando cada línea para trabajar con ella en el foreach
                StringBuilder txtMod = new StringBuilder();  // Aquí iremos añadiendo cada línea, modificada o no, y lo devolveremos al final

                foreach(string linea in lineas)
                {
                    sbLinea.Append(linea);

                    if (sbLinea.ToString().StartsWith(md))
                    {
                        sbLinea.Replace(md, htmlInicio);
                        sbLinea.Append(htmlCierre);                        
                        txtMod.Append(sbLinea);
                        txtMod.AppendLine();
                    }
                    else
                    {
                        txtMod.Append(sbLinea);
                        txtMod.AppendLine();
                    }

                    sbLinea.Clear();
                }

                return txtMod;
            }
            else
            {
                return txt;
            }            
        }

        public static StringBuilder ReemplazarParrafos(StringBuilder txt)
        {
            List<string> lineas = new List<string>(Regex.Split(txt.ToString(), Environment.NewLine));

            for (int i = 1; i < lineas.Count; i++)
            {
                if (lineas[i] == String.Empty)
                {
                    if(!lineas[i - 1].EndsWith("</li>"))
                    {
                        lineas[i - 1] += "</p>";
                        lineas[i] = lineas[i].Insert(0, "<p>");
                    }                    
                }
            }

            StringBuilder sb = new StringBuilder();

            foreach (string linea in lineas)
            {
                sb.Append(linea);
            }

            return sb;
        }

        public static StringBuilder ReemplazarBloque(string txt, string md, string htmlAbre, string htmlCierra)
        {
            // Busca una etiqueta Markdown y la sustituye por una html de inicio y una de cierre, p.ej. ** -> <strong></strong>
            // Usaremos este método para tags de Markdown que afectan a VARIAS LÍNEAS

            bool existeTag = false;

            // Comprobamos que existe la etiqueta markdown
            if (txt.ToString().Contains(md)) existeTag = true;

            while (existeTag)
            {
                // Delimitamos el bloque con tags markdown incluidos, es decir, por ejemplo, **Texto en negrita**
                int indiceInicioBloque = txt.IndexOf(md);

                if(indiceInicioBloque == -1)
                {
                    existeTag = false;
                }
                else
                {
                    int buscar2Tag = indiceInicioBloque + md.Length;
                    int indiceFinBloque = txt.IndexOf(md, buscar2Tag) + md.Length;
                    int longitudBloque = indiceFinBloque - indiceInicioBloque;

                    if(longitudBloque > 0)
                    {
                        // Extraemos el bloque de texto
                        string bloqueMd = txt.Substring(indiceInicioBloque, longitudBloque);

                        // Un string nuevo que será el bloque anterior pero sin los tags md y con los tags html
                        string bloqueHtml = bloqueMd.Replace(md, "");
                        bloqueHtml = bloqueHtml.Insert(0, htmlAbre);
                        bloqueHtml += htmlCierra;

                        // Reemplazamos el bloque antiguo por el nuevo en el texto
                        txt = txt.Replace(bloqueMd, bloqueHtml);
                    }
                    else
                    {
                        existeTag = false;
                    }
                }
            }

            StringBuilder sb = new StringBuilder(txt);
            return sb;
        }

        public static StringBuilder ReemplazarImagenes(string txt)
        {
            string patron = @"!\[.*\]\(.*\)";
            MatchCollection m = Regex.Matches(txt, patron);

            foreach (Match item in m)
            {
                string imagenMd = item.Value;
                string textoAlt = RecuperarTextoCentral(imagenMd, "[", "]");
                string rutaImagen = RecuperarTextoCentral(imagenMd, "(", ")");

                txt = txt.Replace(item.Value, $"<img src='{rutaImagen}' alt='{textoAlt}'>");
            }

            StringBuilder sb = new StringBuilder(txt);
            return sb;
        }

        public static StringBuilder ReemplazarEnlaces(string txt)
        {
            string patron = @"\[.*\]\(.*\)";
            MatchCollection m = Regex.Matches(txt, patron);

            foreach(Match item in m)
            {
                string enlaceMd = item.Value;
                string textoEnlace = RecuperarTextoCentral(enlaceMd, "[", "]");
                string dirEnlace = RecuperarTextoCentral(enlaceMd, "(", ")");

                txt = txt.Replace(item.Value, $"<a href = '{dirEnlace}' target='_blank'>{textoEnlace}</a>");
            }

            StringBuilder sb = new StringBuilder(txt);
            return sb;
        }

        private static string RecuperarTextoCentral(string txtCompleto, string inicio, string cierre)
        {
            // Dado un texto extraer de él una porción de texto result comprendida entre dos strings especificados inicio y cierre

            bool ok = false;
            int indiceInicioResult;
            int indiceFinResult;
            int longitudResult;
            string result;

            // Comprobamos que existen los parámetros inicio y cierre
            if (txtCompleto.Contains(inicio) && txtCompleto.Contains(cierre)) ok = true;

            if (ok)
            {
                indiceInicioResult = txtCompleto.IndexOf(inicio) + inicio.Length;

                if (inicio != cierre)
                {
                    // Si los parámetros de inicio y cierre son distintos
                    indiceFinResult = txtCompleto.IndexOf(cierre);
                }
                else
                {
                    // Si son iguales
                    // Una vez encontrado el primer tag empezará a buscar la primera coincidencia DESPUÉS del primer tag
                    int buscar2Tag = txtCompleto.IndexOf(inicio) + inicio.Length;

                    indiceFinResult = txtCompleto.IndexOf(cierre, buscar2Tag);
                }

                longitudResult = indiceFinResult - indiceInicioResult;
                result = txtCompleto.Substring(indiceInicioResult, longitudResult);
            }
            else
            {
                result = "ERROR: No se han encontrado el parámetro de inicio o el de cierre.";
            }

            return result;
        }

        public static StringBuilder CrearListasOrdenadas(string txt)
        {
            // Dividimos el texto en líneas
            List<string> lineasTxt = new List<string>(Regex.Split(txt, Environment.NewLine));

            // Aquí iremos guardando todas líneas, modificadas o no, para devolverlas al final
            StringBuilder sb = new StringBuilder();

            // Buscamos las líneas que empiecen por el patrón. Las coincidencias se añadirán al MatchCollection (pero no la línea entera)
            string patronNum = @"\d*\. ";
            // string patronRomanMay = @"\bM{0,4}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})\b\. ";
            // string patronRomanMin = @"\bm{0,4}(cm|cd|d?c{0,3})(xc|xl|l?x{0,3})(ix|iv|v?i{0,3})\b\. ";
            // string patronLetrasMay = @"[A-Z]*\. ";
            // string patronLetrasMin = @"[a-z]*\. ";
            MatchCollection m = Regex.Matches(txt, patronNum);

            // Las líneas del texto que empiecen por algún elemento del MatchCollection pertenecen a una lista
            // Ponemos todos los <li></li>
            for (int i = 0; i < lineasTxt.Count; i++)
            {
                for (int j = 0; j < m.Count; j++)
                {
                    if (lineasTxt[i].StartsWith(m[j].ToString()))
                    {
                        lineasTxt[i] = lineasTxt[i].Insert(0, "<li>");
                        lineasTxt[i] += "</li>";
                        lineasTxt[i] = lineasTxt[i].Replace(m[j].ToString(), string.Empty);
                    }
                }
            }

            // Delimitamos cada <ol>
            //  type='{}'
            for (int i = 0; i < lineasTxt.Count; i++)
            {
                if (lineasTxt[i].StartsWith("<li>"))
                {
                    try
                    {
                        if (!lineasTxt[i - 2].StartsWith("<li>") && lineasTxt[i - 1] == String.Empty)
                        {
                            lineasTxt[i - 1] += "<ol>";
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lineasTxt[i] = lineasTxt[i].Insert(0, "<ol>");
                    }

                    try
                    {
                        if (!lineasTxt[i + 2].StartsWith("<li>") && lineasTxt[i + 1] == String.Empty)
                        {
                            lineasTxt[i + 1] += "</ol>";
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lineasTxt[i] += "</ol>";
                    }
                }

            }

            // Añadimos al StringBuilder
            foreach(string item in lineasTxt)
            {
                sb.Append(item);
                sb.AppendLine();
            }

            return sb;
        }
    }
}
