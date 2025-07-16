using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using ReturnOrderGenerator.Models;

namespace ReturnOrderGenerator.Services
{
    /// <summary>
    /// 地址解析服务
    /// </summary>
    public class AddressParseService
    {
        private List<AdministrativeRegion> _regions;
        private readonly Regex _phoneRegex = new Regex(@"1[3-9]\d{9}", RegexOptions.Compiled);
        private readonly Regex _nameRegex = new Regex(@"[\u4e00-\u9fa5]{2,4}", RegexOptions.Compiled);

        public AddressParseService()
        {
            LoadRegionData();
        }

        /// <summary>
        /// 加载行政区划数据
        /// </summary>
        private void LoadRegionData()
        {
            try
            {
                string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChinaRegions.json");

                if (File.Exists(dataPath))
                {
                    string jsonContent = File.ReadAllText(dataPath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    _regions = JsonSerializer.Deserialize<List<AdministrativeRegion>>(jsonContent, options) ?? new List<AdministrativeRegion>();
                }
                else
                {
                    // 如果文件不存在，创建基本的区域数据
                    _regions = CreateBasicRegionData();
                }
            }
            catch (Exception ex)
            {
                // 出错时创建基本的区域数据
                _regions = CreateBasicRegionData();
                System.Diagnostics.Debug.WriteLine($"加载区域数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建基本的区域数据
        /// </summary>
        private List<AdministrativeRegion> CreateBasicRegionData()
        {
            return new List<AdministrativeRegion>
            {
                // 省份
                new AdministrativeRegion { Code = "110000", Name = "北京市", ParentCode = "", Level = 1, Aliases = new List<string> { "北京" } },
                new AdministrativeRegion { Code = "120000", Name = "天津市", ParentCode = "", Level = 1, Aliases = new List<string> { "天津" } },
                new AdministrativeRegion { Code = "310000", Name = "上海市", ParentCode = "", Level = 1, Aliases = new List<string> { "上海" } },
                new AdministrativeRegion { Code = "440000", Name = "广东省", ParentCode = "", Level = 1, Aliases = new List<string> { "广东" } },
                new AdministrativeRegion { Code = "320000", Name = "江苏省", ParentCode = "", Level = 1, Aliases = new List<string> { "江苏" } },
                new AdministrativeRegion { Code = "330000", Name = "浙江省", ParentCode = "", Level = 1, Aliases = new List<string> { "浙江" } },

                // 城市
                new AdministrativeRegion { Code = "110100", Name = "北京市", ParentCode = "110000", Level = 2, Aliases = new List<string>() },
                new AdministrativeRegion { Code = "440100", Name = "广州市", ParentCode = "440000", Level = 2, Aliases = new List<string> { "广州" } },
                new AdministrativeRegion { Code = "440300", Name = "深圳市", ParentCode = "440000", Level = 2, Aliases = new List<string> { "深圳" } },
                new AdministrativeRegion { Code = "320100", Name = "南京市", ParentCode = "320000", Level = 2, Aliases = new List<string> { "南京" } },
                new AdministrativeRegion { Code = "330100", Name = "杭州市", ParentCode = "330000", Level = 2, Aliases = new List<string> { "杭州" } },

                // 区县
                new AdministrativeRegion { Code = "110101", Name = "东城区", ParentCode = "110100", Level = 3, Aliases = new List<string>() },
                new AdministrativeRegion { Code = "110105", Name = "朝阳区", ParentCode = "110100", Level = 3, Aliases = new List<string>() },
                new AdministrativeRegion { Code = "440103", Name = "荔湾区", ParentCode = "440100", Level = 3, Aliases = new List<string>() },
                new AdministrativeRegion { Code = "440303", Name = "罗湖区", ParentCode = "440300", Level = 3, Aliases = new List<string>() },
            };
        }

        /// <summary>
        /// 解析地址文本
        /// </summary>
        public AddressParseResult ParseAddress(string text)
        {
            var result = new AddressParseResult
            {
                OriginalText = text
            };

            if (string.IsNullOrWhiteSpace(text))
            {
                result.AddError("输入文本为空");
                return result;
            }

            try
            {
                // 清理文本，移除多余的空格和标点
                text = CleanText(text);

                // 提取电话号码
                var phoneMatch = _phoneRegex.Match(text);
                if (phoneMatch.Success)
                {
                    result.Phone = phoneMatch.Value;
                }

                // 提取姓名（在提取电话号码后，但不移除电话号码）
                ExtractName(text, result, phoneMatch);

                // 移除已识别的姓名和电话号码
                if (!string.IsNullOrEmpty(result.Name))
                {
                    text = text.Replace(result.Name, " ");
                }
                if (phoneMatch.Success)
                {
                    text = text.Replace(phoneMatch.Value, " ");
                }

                // 解析省市区
                ParseRegions(text, result);

                // 判断解析是否成功
                result.IsSuccess = !string.IsNullOrEmpty(result.Province) ||
                                 !string.IsNullOrEmpty(result.City) ||
                                 !string.IsNullOrEmpty(result.District) ||
                                 !string.IsNullOrEmpty(result.Phone) ||
                                 !string.IsNullOrEmpty(result.Name);

                if (!result.IsSuccess)
                {
                    result.AddError("无法识别有效的地址信息");
                }
                else
                {
                    // 构建完整地址
                    if (!string.IsNullOrEmpty(result.Province) ||
                        !string.IsNullOrEmpty(result.City) ||
                        !string.IsNullOrEmpty(result.District))
                    {
                        result.DetailAddress = ExtractDetailAddress(text, result);
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError($"解析过程中发生错误: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 清理文本
        /// </summary>
        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            // 移除常见的分隔符和标点
            text = text.Replace("，", " ").Replace(",", " ")
                      .Replace("：", " ").Replace(":", " ")
                      .Replace("；", " ").Replace(";", " ")
                      .Replace("。", " ").Replace(".", " ")
                      .Replace("电话", " ").Replace("手机", " ")
                      .Replace("地址", " ").Replace("收件人", " ");

            // 移除多余空格
            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }

            return text.Trim();
        }

        /// <summary>
        /// 提取姓名
        /// </summary>
        private void ExtractName(string text, AddressParseResult result, Match phoneMatch)
        {
            // 策略1：如果找到电话号码，在电话号码附近查找姓名
            if (phoneMatch.Success)
            {
                var phoneIndex = phoneMatch.Index;
                var phoneLength = phoneMatch.Length;

                // 在电话号码前面查找姓名（通常姓名在电话号码前面）
                var beforePhone = text.Substring(0, phoneIndex).Trim();
                var nameFromBefore = ExtractNameFromText(beforePhone, true); // true表示从末尾开始查找

                if (!string.IsNullOrEmpty(nameFromBefore))
                {
                    result.Name = nameFromBefore;
                    return;
                }

                // 在电话号码后面查找姓名
                if (phoneIndex + phoneLength < text.Length)
                {
                    var afterPhone = text.Substring(phoneIndex + phoneLength).Trim();
                    var nameFromAfter = ExtractNameFromText(afterPhone, false); // false表示从开头开始查找

                    if (!string.IsNullOrEmpty(nameFromAfter))
                    {
                        result.Name = nameFromAfter;
                        return;
                    }
                }
            }

            // 策略2：如果没有找到电话号码，使用通用姓名提取
            var generalName = ExtractNameFromText(text, false);
            if (!string.IsNullOrEmpty(generalName))
            {
                result.Name = generalName;
            }
        }

        /// <summary>
        /// 从文本中提取姓名
        /// </summary>
        private string ExtractNameFromText(string text, bool fromEnd)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            // 移除明显的非姓名内容
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[（）()：:，,。.；;]", " ");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"收件信息|支持|自动|识别|姓名|电话|地址", " ");

            // 清理多余空格
            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }

            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (fromEnd)
            {
                // 从末尾开始查找姓名
                for (int i = words.Length - 1; i >= 0; i--)
                {
                    var word = words[i].Trim();
                    if (IsValidName(word))
                    {
                        return word;
                    }
                }
            }
            else
            {
                // 从开头开始查找姓名
                foreach (var word in words)
                {
                    var trimmedWord = word.Trim();
                    if (IsValidName(trimmedWord))
                    {
                        return trimmedWord;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 判断是否是有效的姓名
        /// </summary>
        private bool IsValidName(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length < 2 || text.Length > 4)
                return false;

            // 必须是中文字符
            if (!_nameRegex.IsMatch(text))
                return false;

            // 排除明显不是姓名的词汇
            return !IsLikelyNotName(text);
        }

        /// <summary>
        /// 判断是否不太可能是姓名
        /// </summary>
        private bool IsLikelyNotName(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length < 2 || text.Length > 4)
                return true;

            var notNameWords = new[] {
                "先生", "女士", "小姐", "收件", "地址", "电话", "手机", "快递", "包裹",
                "信息", "支持", "自动", "识别", "姓名", "东街", "西街", "南街", "北街",
                "东路", "西路", "南路", "北路", "大街", "小区", "花园", "广场", "中心",
                "新城", "老城", "开发", "工业", "商业", "住宅", "别墅", "公寓"
            };

            return notNameWords.Any(word => text.Contains(word)) ||
                   text.All(c => !char.IsLetter(c)) || // 不全是字母
                   System.Text.RegularExpressions.Regex.IsMatch(text, @"[0-9]"); // 包含数字
        }

        /// <summary>
        /// 提取详细地址
        /// </summary>
        private string ExtractDetailAddress(string text, AddressParseResult result)
        {
            string detailAddress = text;

            // 移除已识别的信息
            if (!string.IsNullOrEmpty(result.Province))
                detailAddress = detailAddress.Replace(result.Province, "");
            if (!string.IsNullOrEmpty(result.City))
                detailAddress = detailAddress.Replace(result.City, "");
            if (!string.IsNullOrEmpty(result.District))
                detailAddress = detailAddress.Replace(result.District, "");
            if (!string.IsNullOrEmpty(result.Name))
                detailAddress = detailAddress.Replace(result.Name, "");
            if (!string.IsNullOrEmpty(result.Phone))
                detailAddress = detailAddress.Replace(result.Phone, "");

            return detailAddress.Trim();
        }

        /// <summary>
        /// 解析省市区信息
        /// </summary>
        private void ParseRegions(string text, AddressParseResult result)
        {
            // 按级别查找匹配的区域
            var provinces = _regions.Where(r => r.Level == 1).ToList();
            var cities = _regions.Where(r => r.Level == 2).ToList();
            var districts = _regions.Where(r => r.Level == 3).ToList();

            // 查找省份
            foreach (var province in provinces)
            {
                if (text.Contains(province.Name) || province.Aliases.Any(alias => text.Contains(alias)))
                {
                    result.Province = province.Name;
                    break;
                }
            }

            // 查找城市
            foreach (var city in cities)
            {
                if (text.Contains(city.Name) || city.Aliases.Any(alias => text.Contains(alias)))
                {
                    result.City = city.Name;
                    
                    // 如果找到城市但没有省份，尝试通过城市找省份
                    if (string.IsNullOrEmpty(result.Province))
                    {
                        var parentProvince = provinces.FirstOrDefault(p => p.Code == city.ParentCode);
                        if (parentProvince != null)
                        {
                            result.Province = parentProvince.Name;
                        }
                    }
                    break;
                }
            }

            // 查找区县
            foreach (var district in districts)
            {
                if (text.Contains(district.Name))
                {
                    result.District = district.Name;
                    
                    // 如果找到区县但没有城市，尝试通过区县找城市和省份
                    if (string.IsNullOrEmpty(result.City))
                    {
                        var parentCity = cities.FirstOrDefault(c => c.Code == district.ParentCode);
                        if (parentCity != null)
                        {
                            result.City = parentCity.Name;
                            
                            if (string.IsNullOrEmpty(result.Province))
                            {
                                var parentProvince = provinces.FirstOrDefault(p => p.Code == parentCity.ParentCode);
                                if (parentProvince != null)
                                {
                                    result.Province = parentProvince.Name;
                                }
                            }
                        }
                    }
                    break;
                }
            }

            // 提取详细地址（移除已识别的省市区信息）
            string detailAddress = text;
            if (!string.IsNullOrEmpty(result.Province))
            {
                detailAddress = detailAddress.Replace(result.Province, "");
            }
            if (!string.IsNullOrEmpty(result.City))
            {
                detailAddress = detailAddress.Replace(result.City, "");
            }
            if (!string.IsNullOrEmpty(result.District))
            {
                detailAddress = detailAddress.Replace(result.District, "");
            }
            if (!string.IsNullOrEmpty(result.Name))
            {
                detailAddress = detailAddress.Replace(result.Name, "");
            }

            result.DetailAddress = detailAddress.Trim();
        }

        /// <summary>
        /// 获取所有省份列表
        /// </summary>
        public List<string> GetProvinces()
        {
            return _regions.Where(r => r.Level == 1).Select(r => r.Name).ToList();
        }

        /// <summary>
        /// 根据省份获取城市列表
        /// </summary>
        public List<string> GetCitiesByProvince(string provinceName)
        {
            var province = _regions.FirstOrDefault(r => r.Level == 1 && r.Name == provinceName);
            if (province == null) return new List<string>();

            return _regions.Where(r => r.Level == 2 && r.ParentCode == province.Code)
                          .Select(r => r.Name).ToList();
        }

        /// <summary>
        /// 根据城市获取区县列表
        /// </summary>
        public List<string> GetDistrictsByCity(string cityName)
        {
            var city = _regions.FirstOrDefault(r => r.Level == 2 && r.Name == cityName);
            if (city == null) return new List<string>();

            return _regions.Where(r => r.Level == 3 && r.ParentCode == city.Code)
                          .Select(r => r.Name).ToList();
        }
    }
}
