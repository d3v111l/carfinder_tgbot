using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carfinder_tgbotcon.ClassModels
{
    public class Adv
    {
        public string LocationCityName { get; set; }
        public int Usd { get; set; }
        public AutoData AutoData { get; set; }
        public string MarkName { get; set; }
        public int MarkId { get; set; }
        public string ModelName { get; set; }
        public int ModelId { get; set; }
        public PhotoData PhotoData { get; set; }
        public string LinkToView { get; set; }
        public string Title { get; set; }
        public StateData StateData { get; set; }
    }

    public class AutoData
    {
        public string Description { get; set; }
        public string Version { get; set; }
        public int Year { get; set; }
        public int AutoId { get; set; }
        public int StatusId { get; set; }
        public string Race { get; set; }
        public string FuelName { get; set; }
        public string GearboxName { get; set; }
        public bool IsSold { get; set; }
    }

    public class PhotoData
    {
        public int Count { get; set; }
        public string SeoLinkM { get; set; }
        public string SeoLinkSX { get; set; }
        public string SeoLinkB { get; set; }
        public string SeoLinkF { get; set; }
    }

    public class StateData
    {
        public string Name { get; set; }
        public string RegionName { get; set; }
        public string LinkToCatalog { get; set; }
        public string Title { get; set; }
        public int StateId { get; set; }
    }
}
