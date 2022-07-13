/*namespace CmsHeadless.Pages.Content
{
    public class LocationsOfContent
    {
        public int LocationId;
        public string NationName;
        public string RegionName;
        public string ProvinceName;
        public string CityName;
        public string LocationString;
        public LocationsOfContent()
        {

        }
        public LocationsOfContent(int LocationId, string NationName, string RegionName, string ProvinceName, string CityName)
        {
            this.LocationId = LocationId;
            this.NationName = NationName==null?"none":NationName;
            this.LocationString = NationName == null ? null : NationName;

            this.RegionName = RegionName == null ? "none" : RegionName;
            this.LocationString = RegionName == null ? this.LocationString : this.LocationString+ ", "+ RegionName;

            this.ProvinceName = ProvinceName == null ? "none" : ProvinceName;
            this.LocationString = ProvinceName == null ? this.LocationString : this.LocationString + ", " + ProvinceName;

            this.CityName = CityName==null ? "none" : CityName;
            this.LocationString = CityName == null ? this.LocationString : this.LocationString + ", " + CityName;

        }

    }
}
*/