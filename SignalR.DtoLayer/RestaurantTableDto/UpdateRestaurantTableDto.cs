namespace SignalR.DtoLayer.RestaurantTableDto
{
    public class UpdateRestaurantTableDto
    {
        public int RestaurantTableId { get; set; }
        public string TableName { get; set; }
        public bool Status { get; set; }
    }
}
