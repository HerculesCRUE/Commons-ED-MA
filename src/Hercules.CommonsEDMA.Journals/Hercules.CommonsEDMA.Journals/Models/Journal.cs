using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.Journals.Models
{
    public class Journal
    {
        public string idJournal { get; set; }
        public string titulo { get; set; }
        public string issn { get; set; }
        public string eissn { get; set; }
        public string publicador { get; set; }
        public HashSet<IndiceImpacto> indicesImpacto { get; set; }        

        public override bool Equals(Object obj)
        {
            // Perform an equality check on two rectangles (Point object pairs).
            if (obj == null || GetType() != obj.GetType())
                return false;
            Journal compare = (Journal)obj;
            bool iguales = true;
            iguales = iguales && this.titulo == compare.titulo;
            iguales = iguales && this.issn == compare.issn;
            iguales = iguales && this.eissn == compare.eissn;
            iguales = iguales && this.publicador == compare.publicador;
            iguales = iguales && this.indicesImpacto.Count()== compare.indicesImpacto.Count() && this.indicesImpacto.Count() == this.indicesImpacto.Intersect(compare.indicesImpacto).Count();
            return iguales;
        }

        public override int GetHashCode()
        {
            return titulo.GetHashCode();
        }
    }

    public class IndiceImpacto
    {
        public string idImpactIndex { get; set; }
        public string fuente { get; set; }
        public int anyo { get; set; }
        public float indiceImpacto { get; set; }
        public HashSet<Categoria> categorias { get; set; }
        public override bool Equals(Object obj)
        {
            // Perform an equality check on two rectangles (Point object pairs).
            if (obj == null || GetType() != obj.GetType())
                return false;
            IndiceImpacto compare = (IndiceImpacto)obj;
            bool iguales = true;
            iguales = iguales && this.fuente == compare.fuente;
            iguales = iguales && this.anyo == compare.anyo;
            iguales = iguales && this.indiceImpacto == compare.indiceImpacto;
            iguales = iguales && this.categorias.Count() == compare.categorias.Count() && this.categorias.Count() == this.categorias.Intersect(compare.categorias).Count();
            return iguales;
        }

        public override int GetHashCode()
        {
            return indiceImpacto.GetHashCode();
        }
    }

    public class Categoria
    {
        public string idImpactCategory { get; set; }
        public string fuente { get; set; }
        public int anyo { get; set; }
        public string nomCategoria { get; set; }
        public int posicionPublicacion { get; set; }
        public int numCategoria { get; set; }
        public int cuartil { get; set; }

        public override bool Equals(Object obj)
        {
            // Perform an equality check on two rectangles (Point object pairs).
            if (obj == null || GetType() != obj.GetType())
                return false;
            Categoria compare = (Categoria)obj;
            bool iguales = true;
            iguales = iguales && this.fuente == compare.fuente;
            iguales = iguales && this.anyo == compare.anyo;
            iguales = iguales && this.nomCategoria == compare.nomCategoria;
            iguales = iguales && this.posicionPublicacion == compare.posicionPublicacion;
            iguales = iguales && this.numCategoria == compare.numCategoria;
            iguales = iguales && this.cuartil == compare.cuartil;
            return iguales;
        }

        public override int GetHashCode()
        {
            return nomCategoria.GetHashCode();
        }
    }
}
