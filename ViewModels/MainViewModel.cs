using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using ReturnOrderGenerator.Models;
using ReturnOrderGenerator.Services;

namespace ReturnOrderGenerator.ViewModels
{
    /// <summary>
    /// 主界面视图模型
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        private readonly AddressParseService _addressParseService;
        private readonly RandomDataService _randomDataService;
        private readonly ImageGenerationService _imageGenerationService;

        [ObservableProperty]
        private string _recipientName = string.Empty;

        [ObservableProperty]
        private string _recipientPhone = string.Empty;

        [ObservableProperty]
        private string _recipientAddress = string.Empty;



        [ObservableProperty]
        private string _pasteText = string.Empty;

        [ObservableProperty]
        private string _statusMessage = "就绪";

        [ObservableProperty]
        private bool _isGenerating = false;

        public MainViewModel()
        {
            _addressParseService = new AddressParseService();
            _randomDataService = new RandomDataService();
            _imageGenerationService = new ImageGenerationService();
        }

        /// <summary>
        /// 解析粘贴的地址信息命令
        /// </summary>
        public ICommand ParseAddressCommand => new RelayCommand(ParseAddress);

        private void ParseAddress()
        {
            if (string.IsNullOrWhiteSpace(PasteText))
            {
                StatusMessage = "请先粘贴地址信息";
                return;
            }

            try
            {
                var parseResult = _addressParseService.ParseAddress(PasteText);

                // 详细的调试信息
                var debugInfo = $"解析结果: 成功={parseResult.IsSuccess}, " +
                               $"姓名='{parseResult.Name}', " +
                               $"电话='{parseResult.Phone}', " +
                               $"省='{parseResult.Province}', " +
                               $"市='{parseResult.City}', " +
                               $"区='{parseResult.District}', " +
                               $"详址='{parseResult.DetailAddress}'";

                System.Diagnostics.Debug.WriteLine(debugInfo);

                if (parseResult.IsSuccess)
                {
                    // 填充解析结果
                    if (!string.IsNullOrEmpty(parseResult.Name))
                        RecipientName = parseResult.Name;

                    if (!string.IsNullOrEmpty(parseResult.Phone))
                        RecipientPhone = parseResult.Phone;

                    // 构建地址
                    var addressParts = new List<string>();
                    if (!string.IsNullOrEmpty(parseResult.Province)) addressParts.Add(parseResult.Province);
                    if (!string.IsNullOrEmpty(parseResult.City)) addressParts.Add(parseResult.City);
                    if (!string.IsNullOrEmpty(parseResult.District)) addressParts.Add(parseResult.District);
                    if (!string.IsNullOrEmpty(parseResult.DetailAddress)) addressParts.Add(parseResult.DetailAddress);

                    RecipientAddress = string.Join("", addressParts);

                    StatusMessage = $"地址解析成功 - {debugInfo}";
                }
                else
                {
                    StatusMessage = $"地址解析失败: {string.Join(", ", parseResult.Errors)} - {debugInfo}";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"解析出错: {ex.Message}";
            }
        }



        /// <summary>
        /// 清空收件人信息命令
        /// </summary>
        public ICommand ClearRecipientInfoCommand => new RelayCommand(ClearRecipientInfo);

        private void ClearRecipientInfo()
        {
            RecipientName = string.Empty;
            RecipientPhone = string.Empty;
            RecipientAddress = string.Empty;
            PasteText = string.Empty;
            StatusMessage = "已清空收件人信息";
        }

        /// <summary>
        /// 生成图片命令
        /// </summary>
        public ICommand GenerateImageCommand => new RelayCommand(async () => await GenerateImage());

        private async Task GenerateImage()
        {
            if (IsGenerating) return;

            try
            {
                IsGenerating = true;
                StatusMessage = "正在生成图片...";

                // 验证必填信息
                if (!ValidateInput())
                {
                    return;
                }

                // 创建订单信息
                var returnOrderInfo = CreateReturnOrderInfo();

                // 生成图片
                var imageData = await Task.Run(() => _imageGenerationService.GenerateReturnOrderImage(returnOrderInfo));

                // 自动保存到桌面
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = $"退货订单_{returnOrderInfo.Order.OrderNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var filePath = Path.Combine(desktopPath, fileName);

                await Task.Run(() => _imageGenerationService.SaveImageToFile(imageData, filePath));
                StatusMessage = $"图片已保存到桌面: {fileName}";

                // 成功消息已通过StatusMessage显示，不再弹出对话框
            }
            catch (Exception ex)
            {
                StatusMessage = $"生成图片失败: {ex.Message}";
                MessageBox.Show($"生成图片时发生错误：\n{ex.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsGenerating = false;
            }
        }

        /// <summary>
        /// 验证输入信息
        /// </summary>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(RecipientName))
            {
                StatusMessage = "请填写收件人姓名";
                return false;
            }

            if (string.IsNullOrWhiteSpace(RecipientPhone))
            {
                StatusMessage = "请填写收件人电话";
                return false;
            }

            if (string.IsNullOrWhiteSpace(RecipientAddress))
            {
                StatusMessage = "请填写收件人地址";
                return false;
            }



            return true;
        }

        /// <summary>
        /// 创建退货订单信息对象
        /// </summary>
        private ReturnOrderInfo CreateReturnOrderInfo()
        {
            // 自动生成随机订单数据
            var orderInfo = _randomDataService.GenerateOrderInfo();

            return new ReturnOrderInfo
            {
                Recipient = new RecipientInfo
                {
                    Name = RecipientName,
                    Phone = RecipientPhone,
                    Address = RecipientAddress,
                    // 为了确保FullAddress能正确显示，将完整地址也设置到DetailAddress
                    DetailAddress = RecipientAddress
                },
                Order = orderInfo
            };
        }
    }
}
