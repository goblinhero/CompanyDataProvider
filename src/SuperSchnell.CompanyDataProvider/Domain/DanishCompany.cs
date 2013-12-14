using System.Collections.Generic;
using SuperSchnell.CompanyDataProvider.Domain.Rules;

namespace SuperSchnell.CompanyDataProvider.Domain
{
    public class DanishCompany:Entity<DanishCompany>
    {
        public virtual string CompanyName { get; set; }
        public virtual Address Address { get; set; }
        public virtual string CVRNumber { get; set; }
        public virtual int Version { get; set; }

        protected override IEnumerable<Rules.IRule<DanishCompany>> GetValidationRules()
        {
            return new[]
                {
                    new RelayRule<DanishCompany>(dc => string.IsNullOrEmpty(CompanyName),Errors.Domain_Validation_DanishCompany_No_CompanyName), 
                    new RelayRule<DanishCompany>(dc => dc.Address == null,Errors.Domain_Validation_DanishCompany_No_Address), 
                };
        }
    }
}
