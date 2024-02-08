using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces;

public interface IVillaRepository : IRepository<Villa>
{
	public Villa? Get(int id, string? includeProperties = null);

	public void Update(Villa villa);

	public void Save();
}
