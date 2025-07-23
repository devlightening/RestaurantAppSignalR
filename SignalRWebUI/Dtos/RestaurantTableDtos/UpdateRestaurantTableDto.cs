using SignalR.EntityLayer.Entities;

namespace SignalRWebUI.Dtos.RestaurantTableDtos
{
    public class UpdateRestaurantTableDto
    {
        public int RestaurantTableId { get; set; }
        public int TableNo { get; set; }
        public bool Status { get; set; }
        public TableLocation Location { get; set; }
    }
}
