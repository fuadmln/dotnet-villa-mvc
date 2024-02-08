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

public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : class
{
	private readonly ApplicationDbContext _context = context;
	internal DbSet<T> dbSet = context.Set<T>();

	public void Add(T entity)
	{
		_context.Add(entity);
		dbSet = _context.Set<T>();
	}

	public T? Get(Expression<Func<T, bool>> filter, string? includeProperties)
	{
		IQueryable<T> query = dbSet;

		if (filter != null)
			query = query.Where(filter);

		if (!includeProperties.IsNullOrEmpty())
		{
			// case sensitive property, e.g 'Villa,VillaNumber'
			foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query.Include(prop);
			}
		}

		return query.FirstOrDefault();
	}

	public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties)
	{
		IQueryable<T> query = dbSet;

		if (filter != null)
			query = query.Where(filter);

		if (!includeProperties.IsNullOrEmpty())
		{
			foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(prop);
			}
		}

		return query.ToList();
	}

	public void Remove(T entity)
	{
		dbSet.Remove(entity);
	}
}
