using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories;

public class AmenityRepository(ApplicationDbContext context) : Repository<Amenity>(context), IAmenityRepository
{
	private readonly ApplicationDbContext _context = context;

	public Amenity? Get(int id, string? includedPropeties)
	{
		IQueryable<Amenity> query = _context.Amenities.AsQueryable();
		query = query.Where(a => a.Id == id);

		if(!includedPropeties.IsNullOrEmpty())
		{
			foreach(var prop in includedPropeties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
			{
				query.Include(prop);
			}
		}

		return query.FirstOrDefault();
	}

	public void Save()
	{
		_context.SaveChanges();
	}

	public void Update(Amenity amenity)
	{
		_context.Amenities.Update(amenity);
	}
}
