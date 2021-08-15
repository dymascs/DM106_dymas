using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projeto_dm106.Models
{
    public class Order
    {
		public Order()
		{
			this.OrderItems = new HashSet<OrderItem>();
		}

		public int Id { get; set; }

		public string userName { get; set; }

		public decimal precoFrete { get; set; }

		public string dataPostada { get; set; }

		public string dataEntrega { get; set; }

		public string status { get; set; }

		public decimal precoTotal { get; set; }

		public decimal pesoTotal { get; set; }

		public virtual ICollection<OrderItem> OrderItems{ get; set; }

	}
}