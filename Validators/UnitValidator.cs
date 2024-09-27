using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using FluentValidation;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class UnitValidator : AbstractValidator<Unit>
    {
        private readonly PiacomDbContext piacomDbContext;

        public UnitValidator(PiacomDbContext piacomDbContext )
        {
            this.piacomDbContext = piacomDbContext;

            RuleFor(un => un.UnitCode)
                .Must(isUniqueCode).WithMessage("Unit code has existed!");
        }
        private bool isUniqueCode(Unit unit, string unitCode)
        {
            if(unit.UnitCode != null)
            {
                return !piacomDbContext.Units.Any(un => un.UnitCode == unitCode);
            }
            return true;
        }
    }
}
