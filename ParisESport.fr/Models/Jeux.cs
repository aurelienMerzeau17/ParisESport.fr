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
    
    public partial class Jeux
    {
        public Jeux()
        {
            this.Tournois = new HashSet<Tournois>();
        }
    
        public int Id { get; set; }
        public string nom { get; set; }
        public string description { get; set; }
    
        public virtual ICollection<Tournois> Tournois { get; set; }
    }
}