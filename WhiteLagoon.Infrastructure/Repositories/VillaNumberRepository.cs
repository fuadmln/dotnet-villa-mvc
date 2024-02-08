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

public class VillaNumberRepository(ApplicationDbContext context) : Repository<VillaNumber>(context), IVillaNumberRepository
{
	private readonly ApplicationDbContext _context = context;

	public VillaNumber? Get(int id, string? includeProperties)
	{
		IQueryable<VillaNumber> query = _context.VillaNumbers.AsQueryable();

		query = query.Where(vn => vn.Villa_Number == id);

		if (!includeProperties.IsNullOrEmpty())
		{
            foreach (var prop in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
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

	public void Update(VillaNumber villaNumber)
	{
		_context.VillaNumbers.Update(villaNumber);
	}
}
