
namespace Mix.Tenancy.Domain.ViewModels.Init
{
    public class InitDomainViewModel : TenantDataViewModelBase<MixCmsContext, MixDomain, int, InitDomainViewModel>
    {
        public string Host { get; set; }
        public override void InitDefaultValues(string language = null, int? DomainId = null)
        {
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;
        }
    }
}