//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParisESport.fr.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Matchs
    {
        public Matchs()
        {
            this.paris_user = new HashSet<paris_user>();
        }
    
        public int Id { get; set; }
        public int id_Eq1 { get; set; }
        public int id_Eq2 { get; set; }
        public int resultat { get; set; }
        public System.DateTime date { get; set; }
        public int tournoi_id { get; set; }
    
        public virtual Equipes Equipes { get; set; }
        public virtual Equipes Equipes1 { get; set; }
        public virtual ICollection<paris_user> paris_user { get; set; }
        public virtual Tournois Tournois { get; set; }
    }
}
