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
                
            RuleFor(un => un.UnitName)
                .Must(isUniqueName).WithMessage("Unit name has existed!");
        }
        private bool isUniqueCode(Unit unit, string unitCode)
        {
            if(string.IsNullOrEmpty(unit.UnitCode))
            {
                return !piacomDbContext.Units.Any(un => un.UnitCode == unitCode);
            }
            else
            {
                // If UnitCode is not empty and it's being edited, ensure the current Unit's UnitCode is excluded
                return !piacomDbContext.Units.Any(un => un.UnitCode == unitCode && un.UnitID != unit.UnitID);
            }
        }
        private bool isUniqueName(Unit unit, string unitName)
        {
            if (string.IsNullOrEmpty(unit.UnitName))
            {
                return !piacomDbContext.Units.Any(un => un.UnitName == unitName);
            }
            else
            {
                // If UnitCode is not empty and it's being edited, ensure the current Unit's UnitCode is excluded
                return !piacomDbContext.Units.Any(un => un.UnitName == unitName && un.UnitID != unit.UnitID);
            }
        }
    }
}
