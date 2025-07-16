using System;
using System.Collections.Generic;

namespace ReturnOrderGenerator.Models
{
    /// <summary>
    /// 订单信息模型
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// 退货原因
        /// </summary>
        public string ReturnReason { get; set; } = string.Empty;

        /// <summary>
        /// 商品信息
        /// </summary>
        public ProductInfo Product { get; set; } = new ProductInfo();

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 退货时间
        /// </summary>
        public DateTime ReturnTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 商品信息模型
    /// </summary>
    public class ProductInfo
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 商品规格
        /// </summary>
        public string Specification { get; set; } = string.Empty;

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// 商品总价
        /// </summary>
        public decimal TotalPrice => Price * Quantity;

        /// <summary>
        /// 商品类别
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 商品品牌
        /// </summary>
        public string Brand { get; set; } = string.Empty;
    }

    /// <summary>
    /// 退货订单完整信息
    /// </summary>
    public class ReturnOrderInfo
    {
        /// <summary>
        /// 收件人信息
        /// </summary>
        public RecipientInfo Recipient { get; set; } = new RecipientInfo();

        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderInfo Order { get; set; } = new OrderInfo();

        /// <summary>
        /// 验证订单信息是否完整
        /// </summary>
        public bool IsValid()
        {
            return Recipient.IsValid() &&
                   !string.IsNullOrWhiteSpace(Order.OrderNumber) &&
                   !string.IsNullOrWhiteSpace(Order.ReturnReason) &&
                   !string.IsNullOrWhiteSpace(Order.Product.Name);
        }
    }
}
