using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
	public VillaNumber Get(int id, string? includeProperties = null);

	public void Update(VillaNumber villaNumber);

	public void Save();
}
