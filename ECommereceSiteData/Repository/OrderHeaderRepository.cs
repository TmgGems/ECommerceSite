using ECommereceSiteData.Data;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommereceSiteData.Repository
{
	public class OrderHeaderRepository : Repository<OrderHeader>,IOrderHeaderRepositroy
	{
		private ApplicationDbContext _db;
		public OrderHeaderRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public void Update(OrderHeader obj)
		{
			_db.OrderHeaders.Update(obj);
		}
	}
}
