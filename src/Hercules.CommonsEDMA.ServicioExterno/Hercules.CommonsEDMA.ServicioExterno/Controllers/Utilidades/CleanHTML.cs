using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;



namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades
{
    public class CleanHTML
    {
        /// <summary>
        /// Método para limpiar un string de tags, a excepción de los permitidos
        /// El método también elimina los atributos menos el "style"
        /// </summary>
        /// <param name="source">Texto de entrada.</param>
        /// <param name="tagsExceptions">Array con los tags "excepcionales".</param>
        /// <param name="AttrsExceptions">Array con los atributos (de los tags) "excepcionales".</param>
        /// <returns>string resultante.</returns>
        public static string StripTagsCharArray(string source, string[] tagsExceptions, string[] AttrsExceptions)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;
            List<char> currentTag = new();
            List<char> attrs = new();
            string tag = "";
            string attrsTag = "";

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                // Comprueba si es empieza un tag y lo guarda ahí
                // Los caracteres siguientes se guardarán como tag o atributo hasta el cierre del tag
                if (let == '<')
                {
                    inside = true;
                    currentTag = new();
                    attrs = new();
                    currentTag.Add(let);
                    continue;
                }
                if (let == '>')
                {
                    // Vuelve a guardar el texto, no lo guarda como tag
                    inside = false;
                    currentTag.Add(let);
                    tag = string.Join("", currentTag);
                    attrsTag = string.Join("", attrs);
                    if (tagsExceptions.Contains(tag) && !tag.Contains("script") && !attrsTag.Contains("script")) {
                        for (int n = 0; n < currentTag.Count -1; n++)
                        {
                            array[arrayIndex] = currentTag[n];
                            arrayIndex++;
                        }
                        // Añade los atributos (si es style)
                        if (attrsTag.Length > 0)
                        {
                            attrsTag = StripAttrsCharArray(attrsTag, AttrsExceptions);
                            attrsTag = attrsTag.Contains('"') ? attrsTag : "";
                            for (int n = 0; n < attrsTag.Length; n++)
                            {
                                array[arrayIndex] = attrsTag[n];
                                arrayIndex++;
                            }
                        }
                        array[arrayIndex] = let;
                        arrayIndex++;
                    }
                    continue;
                }
                // !inside significa que no estamos dentro de un tag
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                } else
                {
                    // Estamos dentro de un tag, por lo que se guardará como tag o atributo
                    if (attrs != null && attrs.Count > 0)
                    {
                        attrs.Add(let);
                    } else if (let == ' ')
                    {
                        attrs.Add(let);
                    } else
                    {
                        currentTag.Add(let);
                    }
                }
            }
            return new string(array, 0, arrayIndex);
        }

        /// <summary>
        /// Método para limpiar un string de atributos, a excepción de los permitidos
        /// </summary>
        /// <param name="source">Texto de entrada.</param>
        /// <param name="AttrsExceptions">Array con los atributos (de los tags) "excepcionales".</param>
        /// <returns>string resultante.</returns>
        private static string StripAttrsCharArray(string source, string[] AttrsExceptions)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;
            bool insideContAttr = false;
            bool beforeSpace = false;
            bool escaped = false;
            List<char> currentAttr = new();
            List<char> attrContent = new();
            string attr = "";
            string contentAttr = "";

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                // Comprueba si es empieza un tag y lo guarda ahí
                // Los caracteres siguientes se guardarán como tag o atributo hasta el cierre del tag
                if (!inside && let != ' ')
                {
                    inside = true;
                    insideContAttr = false;
                    beforeSpace = false;
                    currentAttr = new();
                    currentAttr.Add(let);
                    continue;
                }
                else if (inside && !insideContAttr && !escaped && let == '"')
                {
                    insideContAttr = true;
                    beforeSpace = false;
                    attrContent = new();
                    currentAttr.Add(let);
                    continue;
                }
                else if ((inside && !escaped && let == '"') || (inside && beforeSpace && let == ' ' && !insideContAttr) )
                {
                    // Vuelve a guardar el texto, no lo guarda como tag
                    inside = false;
                    beforeSpace = false;
                    insideContAttr = false;
                    currentAttr.Add(let);

                    string fullAttr = string.Join("", currentAttr).Trim();
                    fullAttr = fullAttr.Replace("\"\"", "").Trim();
                    attr = fullAttr.Replace("=", "").Trim();
                    contentAttr = string.Join("", attrContent);
                    if (AttrsExceptions.Contains(attr) && !attr.Contains("script") && !contentAttr.Contains("script"))
                    {
                        for (int n = 0; n < currentAttr.Count -1; n++)
                        {
                            array[arrayIndex] = currentAttr[n];
                            arrayIndex++;
                        }

                        // Añade los atributos (si es style)
                        if (contentAttr.Length > 0)
                        {
                            for (int n = 0; n < contentAttr.Length; n++)
                            {
                                array[arrayIndex] = contentAttr[n];
                                arrayIndex++;
                            }
                        }
                        array[arrayIndex] = let;
                        arrayIndex++;
                    }
                    continue;
                }
                
                if (let == ' ')
                {
                    beforeSpace = true;
                } else
                {
                    beforeSpace = false;
                }

                if (let == '\\')
                {
                    escaped = !escaped;
                } else
                {
                    escaped = false;
                }


                // !inside significa que no estamos dentro de un atributo
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
                else
                {
                    // Estamos dentro del contenido del attr de un tag, por lo que se guardará como tag o atributo
                    if (insideContAttr && attrContent != null && attrContent.Count > 0)
                    {
                        attrContent.Add(let);
                    } else if (insideContAttr && attrContent != null)
                    {
                        attrContent.Add(let);
                    }
                    else
                    {
                        beforeSpace = false;
                        currentAttr.Add(let);
                    }
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }

}