namespace SplitPeWebAPI.Models
{
    public class SettlementDetails
    {
        public List<OweTo> IOweTo { get; set; }
        public List<OweBy> TheyOweMe { get; set; }
    }
}
