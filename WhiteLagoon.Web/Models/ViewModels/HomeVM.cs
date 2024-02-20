using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Models.ViewModels;

public class HomeVM
{
    public IEnumerable<Villa> VillaList { get; set; }

    public DateOnly CheckInDate { get; set; }

    public DateOnly? CheckInOut { get; set; }

    public int Nights { get; set; }
}
