using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projeto_dm106.Models
{
    public class Product

	{
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string nome { get; set; }

        public string descricao { get; set; }

        public string cor { get; set; }

        [Required]
        public string modelo { get; set; }

        [Required]
        public string codigo { get; set; }

        [Range(0, 999, ErrorMessage = "O preço deverá ser entre 0  e 999.")]
        public decimal preco { get; set; }

        public decimal peso { get; set; }

        [Range(0, 999, ErrorMessage = "O altura deverá ser entre 0  e 999.")]
        public decimal altura { get; set; }

        [Range(0, 999, ErrorMessage = "O largura deverá ser entre 0  e 999.")]
        public decimal largura { get; set; }

        [Range(0, 999, ErrorMessage = "O comprimento deverá ser entre 0  e 999.")]
        public decimal comprimento { get; set; }

        [Range(0, 999, ErrorMessage = "O diametro deverá ser entre 0  e 999.")]
        public decimal diametro { get; set; }

        public string Url { get; set; }



    }
}