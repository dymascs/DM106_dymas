namespace projeto_dm106.Migrations
{
    using projeto_dm106.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<projeto_dm106.Data.projeto_dm106Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(projeto_dm106.Data.projeto_dm106Context context)
        {
           context.Products.AddOrUpdate(
                p => p.Id,
                new Product { Id = 1, nome = "produto 1", descricao = "descrição produto 1", cor = "verde",    modelo = "ra9", codigo = "COD1", preco = 10, peso = 10, altura = 1, largura = 1, comprimento = 1, diametro = 1, Url = "www.siecolasystems.com/produto1" },
                new Product { Id = 2, nome = "produto 2", descricao = "descrição produto 2", cor = "vermelho", modelo = "xm3", codigo = "COD2", preco = 12, peso = 12, altura = 2, largura = 2, comprimento = 2, diametro = 2, Url = "www.siecolasystems.com/produto2" },
                new Product { Id = 3, nome = "produto 3", descricao = "descrição produto 3", cor = "azul",     modelo = "tm6", codigo = "COD3", preco = 13, peso = 13, altura = 3, largura = 3, comprimento = 3, diametro = 3, Url = "www.siecolasystems.com/produto3" }
               
            );

           

        }
    }
}
