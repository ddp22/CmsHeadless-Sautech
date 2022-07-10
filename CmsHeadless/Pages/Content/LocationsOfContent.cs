namespace CmsHeadless.Pages.Content
{
    public class LocationsOfContent
    {
        public int LocationId;
        public string NationName;
        public string RegionName;
        public string ProvinceName;
        public string CityName;
        public LocationsOfContent()
        {

        }
        public LocationsOfContent(int LocationId, string NationName, string RegionName, string ProvinceName, string CityName)
        {
            this.LocationId = LocationId;
            this.NationName = NationName==null?"none":NationName;
            this.RegionName = RegionName == null ? "none" : RegionName;
            this.ProvinceName = ProvinceName == null ? "none" : ProvinceName;
            this.CityName = CityName==null ? "none" : CityName;
        }

    }
}
