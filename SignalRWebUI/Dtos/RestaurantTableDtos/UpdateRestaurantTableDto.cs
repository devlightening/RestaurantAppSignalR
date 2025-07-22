namespace SignalRWebUI.Dtos.RestaurantTableDtos
{
    public class UpdateRestaurantTableDto
    {
        public int RestaurantTableId { get; set; }
        public string TableName { get; set; }
        public bool Status { get; set; }
        public string Location { get; set; }
    }
}
