﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Common.Interfaces;

public interface IUnitOfWork
{
	IVillaRepository VillaRepo { get; }
	IVillaNumberRepository VillaNumberRepo { get; }
	IAmenityRepository AmenityRepo { get; }
}
