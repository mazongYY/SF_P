using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReturnOrderGenerator.Models
{
    /// <summary>
    /// 地址解析结果
    /// </summary>
    public class AddressParseResult
    {
        /// <summary>
        /// 是否解析成功
        /// </summary>
        public bool IsSuccess { get; set; }

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
        /// 姓名（如果能从文本中提取）
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 电话（如果能从文本中提取）
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 原始输入文本
        /// </summary>
        public string OriginalText { get; set; } = string.Empty;

        /// <summary>
        /// 解析错误信息
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// 获取完整地址
        /// </summary>
        public string FullAddress => $"{Province}{City}{District}{DetailAddress}";

        /// <summary>
        /// 添加错误信息
        /// </summary>
        public void AddError(string error)
        {
            Errors.Add(error);
            IsSuccess = false;
        }

        /// <summary>
        /// 清空结果
        /// </summary>
        public void Clear()
        {
            IsSuccess = false;
            Province = string.Empty;
            City = string.Empty;
            District = string.Empty;
            DetailAddress = string.Empty;
            Name = string.Empty;
            Phone = string.Empty;
            OriginalText = string.Empty;
            Errors.Clear();
        }
    }

    /// <summary>
    /// 行政区划信息
    /// </summary>
    public class AdministrativeRegion
    {
        /// <summary>
        /// 区划代码
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 区划名称
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 父级区划代码
        /// </summary>
        [JsonPropertyName("parentCode")]
        public string ParentCode { get; set; } = string.Empty;

        /// <summary>
        /// 区划级别（1:省 2:市 3:区县）
        /// </summary>
        [JsonPropertyName("level")]
        public int Level { get; set; }

        /// <summary>
        /// 别名列表
        /// </summary>
        [JsonPropertyName("aliases")]
        public List<string> Aliases { get; set; } = new List<string>();
    }
}
