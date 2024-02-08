using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories;

public class VillaRepository(ApplicationDbContext context) : Repository<Villa>(context), IVillaRepository
{
    private readonly ApplicationDbContext _context = context;

    public Villa? Get(int id, string? includeProperties)
    {
        IQueryable<Villa> query = _context.Villas.AsQueryable();

        query = query.Where(v => v.Id == id);

        if (!includeProperties.IsNullOrEmpty())
        {
            foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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

    public void Update(Villa villa)
    {
        _context.Villas.Update(villa);
    }
}
