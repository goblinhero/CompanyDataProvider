using System.Collections.Generic;
using NHibernate.Search.Attributes;
using SuperSchnell.CompanyDataProvider.Domain.Abstract;
using SuperSchnell.CompanyDataProvider.Domain.Rules;

namespace SuperSchnell.CompanyDataProvider.Domain
{
    [Indexed]
    public class DanishCompany:Entity<DanishCompany>
    {
        [Field]
        public virtual string CompanyName { get; set; }
        [IndexedEmbedded]
        public virtual Address Address { get; set; }
        [Field]
        public virtual string CVRNumber { get; set; }
        [Field]
        public virtual string Email { get; set; }
        [Field]
        public virtual string Phone { get; set; }

        protected override IEnumerable<IRule<DanishCompany>> GetValidationRules()
        {
            return new[]
                {
                    new RelayRule<DanishCompany>(dc => string.IsNullOrEmpty(CompanyName),Errors.Domain_Validation_DanishCompany_No_CompanyName), 
                    new RelayRule<DanishCompany>(dc => dc.Address == null,Errors.Domain_Validation_DanishCompany_No_Address), 
                };
        }
    }
}
