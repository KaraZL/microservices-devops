using System;

namespace Books.API.Dtos
{
    //Ce DTO est nécessaire car lors de la creation de la table
    //Id est en Identity, c'est la BDD qui s'occupe d' incrementer
    //Cela dit, vu que la classe Book contient le champ Id, une erreur va se produire pour la seed migration
    //En effet meme si l'id n'est pas specifié dans la migration flutermigrator va quand meme l'ajouter dans la requete sql
    public class BookMigrationDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
    }
}
