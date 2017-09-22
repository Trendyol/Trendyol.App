namespace WindowsService
{
    public class ReceivingOrderItem
    {
        public long Id { get; set; }
        public long ReceivingSessionId { get; set; }
        public long ReceivingOrderId { get; set; }
        public string ReceivingOrderCode { get; set; }
        public long WarehouseReceiptDetailId { get; set; }
        public long InventorySiteId { get; set; }
        public long DepositorId { get; set; }
        public long WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public long? CampaignId { get; set; }
        public string CampaignCode { get; set; }
        public long? SupplierId { get; set; }
        public string SupplierDescription { get; set; }
        public long? PurchaseOrderId { get; set; }
        public string PurchaseOrderCode { get; set; }
        public long? PurchaseOrderTypeId { get; set; }
        public long InventoryItemId { get; set; }
        public long InventoryItemPackTypeId { get; set; }
        public long StockPalletId { get; set; }
        public string StockPalletCode { get; set; }
        public long LocationId { get; set; }
        public string CreatedBy { get; set; }
        public string Barcode { get; set; }
        public long? ReceivingOrderStatusId { get; set; }
        public bool IsPreReceivingCompleted { get; set; }
    }
}