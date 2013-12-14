using FluentNHibernate.Mapping;
using SuperSchnell.CompanyDataProvider.Domain;

namespace SuperSchnell.CompanyDataProvider.Mappings
{
    public class DanishCompanyMap : ClassMap<DanishCompany>
    {
        public DanishCompanyMap()
        {
            Id(m => m.Id).GeneratedBy.HiLo("100");
            Version(m => m.Version).Not.Nullable();
            Map(m => m.CVRNumber).Not.Nullable();
            Map(m => m.CompanyName).Not.Nullable();
            Component(m => m.Address, a =>
                {
                    a.Map(m => m.Street).Not.Nullable();
                    a.Map(m => m.Zip).Not.Nullable();
                    a.Map(m => m.City).Not.Nullable();
                });
        }
    }
}