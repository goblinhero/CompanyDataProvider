using FluentNHibernate.Mapping;
using SuperSchnell.CompanyDataProvider.Contracts;
using SuperSchnell.CompanyDataProvider.Contracts.Dto;

namespace SuperSchnell.CompanyDataProvider.Mappings.Dto
{
    public class DanishCompanyDtoMap : ClassMap<DanishCompanyDto>
    {
        public DanishCompanyDtoMap()
        {
            Table("DanishCompany");
            Not.LazyLoad();
            ReadOnly();
            Id(m => m.Id).GeneratedBy.Assigned();
            Map(m => m.Version).Not.Nullable();
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