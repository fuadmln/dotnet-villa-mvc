using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces;

public interface IAmenityRepository : IRepository<Amenity>
{
	public Amenity? Get(int id, string? includedPropeties);

	public void Update(Amenity amenity);

	public void Save();

}
