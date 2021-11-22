namespace SteinBackEnd.Model
{
    public class RequestResult
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public string InventoryName { get; set; }
        public int RequestedKernels { get; set; }
        public int Kernels { get; set; }
    }
}