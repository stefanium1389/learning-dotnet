using System;

namespace Domain.Entities
{
    public class Accomodation
    {
        public Guid Id {get; set;}
        public AccomodationType AccomodationType {get; set;}
        public string Name {get; set;}
        public int Stars {get; set;}
        public bool HasPool {get; set;}
        public bool HasSpa {get; set;}
        public bool DisabledFriendly {get; set;}
        public bool HasWifi {get; set;}
        public IList<AccomodationUnit> AccomodationUnits {get; set;}
        public bool IsDeleted {get; set;}
        public int UnbookedUnits {get => AccomodationUnits.Where(u => !u.IsBooked).Count();}
        public int UnitCount {get => AccomodationUnits.Count; }
    }
}
