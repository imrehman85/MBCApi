namespace MbcApi.Core.Entities
{
    public class Rooms
    {
        public string RoomId { get; set; } 

        public string RoomNumber { get; set; }

        public string Floor { get; set; }

        public int TotalBed { get; set; }

        public int TotalCapacity { get; set; }

        public bool IsAttachBath { get; set; }

        public bool IsAttachKitchen { get; set; }

        public string Status { get; set; }
    }
}
