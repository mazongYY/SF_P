using System;
using System.Collections.Generic;
using System.Linq;
using ReturnOrderGenerator.Models;

namespace ReturnOrderGenerator.Services
{
    /// <summary>
    /// 随机数据生成服务
    /// </summary>
    public class RandomDataService
    {
        private readonly Random _random = new Random();

        // 退货原因列表
        private readonly List<string> _returnReasons = new List<string>
        {
            "商品质量问题",
            "商品与描述不符",
            "尺寸不合适",
            "颜色不满意",
            "收到商品损坏",
            "不喜欢了",
            "买错了",
            "发错货了",
            "商品有瑕疵",
            "包装破损",
            "功能不符合预期",
            "材质问题",
            "做工粗糙",
            "使用不便",
            "性价比不高"
        };

        // 商品名称列表
        private readonly List<string> _productNames = new List<string>
        {
            "无线蓝牙耳机",
            "智能手机壳",
            "运动休闲鞋",
            "时尚T恤",
            "保温水杯",
            "笔记本电脑包",
            "充电宝",
            "数据线",
            "键盘鼠标套装",
            "护肤面膜",
            "洗发水",
            "牙刷套装",
            "毛巾浴巾",
            "床上四件套",
            "抱枕靠垫",
            "台灯",
            "收纳盒",
            "雨伞",
            "背包",
            "钱包",
            "手表",
            "眼镜",
            "帽子",
            "围巾",
            "手套"
        };

        // 商品规格列表
        private readonly List<string> _specifications = new List<string>
        {
            "黑色/M码",
            "白色/L码",
            "蓝色/XL码",
            "红色/S码",
            "灰色/均码",
            "粉色/XXL码",
            "绿色/38码",
            "黄色/39码",
            "紫色/40码",
            "橙色/41码",
            "标准版",
            "豪华版",
            "经典款",
            "限量版",
            "升级版"
        };

        // 商品类别列表
        private readonly List<string> _categories = new List<string>
        {
            "数码配件",
            "服装鞋帽",
            "家居用品",
            "美妆个护",
            "运动户外",
            "箱包配饰",
            "食品饮料",
            "母婴用品",
            "图书文具",
            "汽车用品"
        };

        // 商品品牌列表
        private readonly List<string> _brands = new List<string>
        {
            "小米",
            "华为",
            "苹果",
            "三星",
            "联想",
            "戴尔",
            "惠普",
            "索尼",
            "松下",
            "飞利浦",
            "美的",
            "格力",
            "海尔",
            "TCL",
            "创维",
            "优衣库",
            "ZARA",
            "H&M",
            "耐克",
            "阿迪达斯",
            "李宁",
            "安踏",
            "特步",
            "361°",
            "匹克"
        };

        /// <summary>
        /// 生成随机订单号
        /// </summary>
        public string GenerateOrderNumber()
        {
            // 格式：平台前缀 + 年月日 + 随机数字
            string prefix = "TB"; // 淘宝前缀
            string date = DateTime.Now.ToString("yyyyMMdd");
            string randomPart = _random.Next(100000, 999999).ToString();
            
            return $"{prefix}{date}{randomPart}";
        }

        /// <summary>
        /// 生成随机退货原因
        /// </summary>
        public string GenerateReturnReason()
        {
            return _returnReasons[_random.Next(_returnReasons.Count)];
        }

        /// <summary>
        /// 生成随机商品信息
        /// </summary>
        public ProductInfo GenerateProductInfo()
        {
            return new ProductInfo
            {
                Name = _productNames[_random.Next(_productNames.Count)],
                Specification = _specifications[_random.Next(_specifications.Count)],
                Price = GenerateRandomPrice(),
                Quantity = _random.Next(1, 4), // 1-3个
                Category = _categories[_random.Next(_categories.Count)],
                Brand = _brands[_random.Next(_brands.Count)]
            };
        }

        /// <summary>
        /// 生成随机价格
        /// </summary>
        private decimal GenerateRandomPrice()
        {
            // 生成10-500元之间的价格
            double price = _random.NextDouble() * 490 + 10;
            return Math.Round((decimal)price, 2);
        }

        /// <summary>
        /// 生成完整的随机订单信息
        /// </summary>
        public OrderInfo GenerateOrderInfo()
        {
            return new OrderInfo
            {
                OrderNumber = GenerateOrderNumber(),
                ReturnReason = GenerateReturnReason(),
                Product = GenerateProductInfo(),
                CreateTime = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-1)),
                ReturnTime = DateTime.Now
            };
        }

        /// <summary>
        /// 生成指定范围内的随机日期
        /// </summary>
        private DateTime GenerateRandomDate(DateTime startDate, DateTime endDate)
        {
            int range = (endDate - startDate).Days;
            return startDate.AddDays(_random.Next(range));
        }

        /// <summary>
        /// 生成完整的随机退货订单信息（不包含收件人信息）
        /// </summary>
        public ReturnOrderInfo GenerateReturnOrderInfo()
        {
            return new ReturnOrderInfo
            {
                Order = GenerateOrderInfo(),
                Recipient = new RecipientInfo() // 空的收件人信息，需要用户填写
            };
        }

        /// <summary>
        /// 为现有订单信息随机填充缺失的字段
        /// </summary>
        public void FillMissingOrderData(OrderInfo orderInfo)
        {
            if (string.IsNullOrWhiteSpace(orderInfo.OrderNumber))
            {
                orderInfo.OrderNumber = GenerateOrderNumber();
            }

            if (string.IsNullOrWhiteSpace(orderInfo.ReturnReason))
            {
                orderInfo.ReturnReason = GenerateReturnReason();
            }

            if (string.IsNullOrWhiteSpace(orderInfo.Product.Name))
            {
                var randomProduct = GenerateProductInfo();
                orderInfo.Product = randomProduct;
            }
            else
            {
                // 只填充缺失的商品字段
                if (string.IsNullOrWhiteSpace(orderInfo.Product.Specification))
                {
                    orderInfo.Product.Specification = _specifications[_random.Next(_specifications.Count)];
                }
                if (orderInfo.Product.Price <= 0)
                {
                    orderInfo.Product.Price = GenerateRandomPrice();
                }
                if (orderInfo.Product.Quantity <= 0)
                {
                    orderInfo.Product.Quantity = _random.Next(1, 4);
                }
                if (string.IsNullOrWhiteSpace(orderInfo.Product.Category))
                {
                    orderInfo.Product.Category = _categories[_random.Next(_categories.Count)];
                }
                if (string.IsNullOrWhiteSpace(orderInfo.Product.Brand))
                {
                    orderInfo.Product.Brand = _brands[_random.Next(_brands.Count)];
                }
            }

            if (orderInfo.CreateTime == default)
            {
                orderInfo.CreateTime = GenerateRandomDate(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-1));
            }

            if (orderInfo.ReturnTime == default)
            {
                orderInfo.ReturnTime = DateTime.Now;
            }
        }
    }
}
