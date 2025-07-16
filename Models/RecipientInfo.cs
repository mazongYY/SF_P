using System.ComponentModel.DataAnnotations;

namespace ReturnOrderGenerator.Models
{
    /// <summary>
    /// 收件人信息模型
    /// </summary>
    public class RecipientInfo
    {
        /// <summary>
        /// 收件人姓名
        /// </summary>
        [Required(ErrorMessage = "收件人姓名不能为空")]
        [StringLength(50, ErrorMessage = "收件人姓名长度不能超过50个字符")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 收件人电话
        /// </summary>
        [Required(ErrorMessage = "收件人电话不能为空")]
        [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "请输入正确的手机号码")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 收件人地址
        /// </summary>
        [Required(ErrorMessage = "收件人地址不能为空")]
        [StringLength(200, ErrorMessage = "收件人地址长度不能超过200个字符")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; } = string.Empty;

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; set; } = string.Empty;

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; } = string.Empty;

        /// <summary>
        /// 获取完整地址
        /// </summary>
        public string FullAddress => $"{Province}{City}{District}{DetailAddress}";

        /// <summary>
        /// 验证收件人信息是否完整
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Phone) &&
                   !string.IsNullOrWhiteSpace(Address);
        }

        /// <summary>
        /// 清空所有信息
        /// </summary>
        public void Clear()
        {
            Name = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
            Province = string.Empty;
            City = string.Empty;
            District = string.Empty;
            DetailAddress = string.Empty;
        }
    }
}
