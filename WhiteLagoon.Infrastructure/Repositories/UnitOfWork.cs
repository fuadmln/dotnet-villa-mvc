using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
	private readonly ApplicationDbContext _context = context;

	public IVillaRepository VillaRepo { get; private set; } = new VillaRepository(context);

	public IVillaNumberRepository VillaNumberRepo { get; private set; } = new VillaNumberRepository(context);
}
