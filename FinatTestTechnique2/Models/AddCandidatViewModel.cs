﻿namespace FinatTestTechnique2.Models
{
    public class AddCandidatViewModel
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public string Telephone { get; set; }
        public string NiveauEtude { get; set; }
        public int AnneesExperience { get; set; }
        public string DernierEmployeur { get; set; }
        public IFormFile CV { get; set; }
    }
}
