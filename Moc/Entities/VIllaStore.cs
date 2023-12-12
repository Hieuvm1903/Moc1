using Moc.DTO;

namespace Moc.Entities
{
    public static class VillaStore
    {
        public static List<VillaDTO> villasList = new List<VillaDTO> {
            new VillaDTO {ID =1,Name = "Pool View",Sqft = 100,Occupancy = 4},
            new VillaDTO {ID =2,Name ="Beach View",Sqft = 300, Occupancy = 3}
        };

    }
}
