using System;
using System.IO;
using SkiaSharp;
using ReturnOrderGenerator.Models;

namespace ReturnOrderGenerator.Services
{
    /// <summary>
    /// 图片生成服务
    /// </summary>
    public class ImageGenerationService
    {
        // 手机屏幕尺寸 (iPhone 14 Pro Max 为例)
        private const int ImageWidth = 430;
        private const int ImageHeight = 932;
        
        // 颜色定义 - 售后详情页面样式
        private readonly SKColor BackgroundColor = SKColor.Parse("#F5F5F5");
        private readonly SKColor CardBackgroundColor = SKColors.White;
        private readonly SKColor PrimaryColor = SKColor.Parse("#FF4757");
        private readonly SKColor SecondaryColor = SKColor.Parse("#2F3542");
        private readonly SKColor TextGrayColor = SKColor.Parse("#57606F");
        private readonly SKColor YellowColor = SKColor.Parse("#FFC048");
        private readonly SKColor GrayColor = SKColor.Parse("#95A5A6");
        private readonly SKColor BorderColor = SKColor.Parse("#E1E8ED");

        /// <summary>
        /// 生成退货订单图片 - 售后详情页面样式
        /// </summary>
        public byte[] GenerateReturnOrderImage(ReturnOrderInfo orderInfo)
        {
            using var surface = SKSurface.Create(new SKImageInfo(ImageWidth, ImageHeight));
            var canvas = surface.Canvas;

            // 清空画布
            canvas.Clear(BackgroundColor);

            var currentY = 0f;

            // 绘制顶部标题栏
            currentY = DrawTopBar(canvas, currentY);

            // 绘制时间提醒卡片
            currentY = DrawTimeReminderCard(canvas, currentY);

            // 绘制退货地址卡片
            currentY = DrawReturnAddressCard(canvas, currentY, orderInfo.Recipient);

            // 绘制服务说明卡片
            currentY = DrawServiceCard(canvas, currentY);

            // 绘制我要寄件按钮
            currentY = DrawShipButton(canvas, currentY);

            // 绘制服务保障
            currentY = DrawServiceGuarantee(canvas, currentY);

            // 绘制底部按钮
            DrawBottomButtons(canvas, currentY);

            // 生成图片数据
            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        private void DrawRoundedRect(SKCanvas canvas, SKRect rect, float cornerRadius, SKColor fillColor, SKColor? strokeColor = null, float strokeWidth = 1f)
        {
            using var path = new SKPath();
            path.AddRoundRect(rect, cornerRadius, cornerRadius);

            // 填充
            using var fillPaint = new SKPaint
            {
                Color = fillColor,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };
            canvas.DrawPath(path, fillPaint);

            // 描边
            if (strokeColor.HasValue)
            {
                using var strokePaint = new SKPaint
                {
                    Color = strokeColor.Value,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = strokeWidth,
                    IsAntialias = true
                };
                canvas.DrawPath(path, strokePaint);
            }
        }

        /// <summary>
        /// 绘制顶部标题栏
        /// </summary>
        private float DrawTopBar(SKCanvas canvas, float startY)
        {
            var headerHeight = 60f;
            var padding = 20f;

            // 绘制白色背景
            var headerRect = new SKRect(0, startY, ImageWidth, startY + headerHeight);
            DrawRoundedRect(canvas, headerRect, 0, CardBackgroundColor);

            // 绘制标题
            using var titlePaint = new SKPaint
            {
                Color = SecondaryColor,
                TextSize = 20,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Bold)
            };

            var titleText = "售后详情";
            canvas.DrawText(titleText, padding, startY + headerHeight / 2 + 7, titlePaint);

            // 绘制返回按钮
            using var returnPaint = new SKPaint
            {
                Color = TextGrayColor,
                TextSize = 16,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei")
            };

            var returnText = "返回";
            var returnBounds = new SKRect();
            returnPaint.MeasureText(returnText, ref returnBounds);
            canvas.DrawText(returnText, ImageWidth - padding - returnBounds.Width, startY + headerHeight / 2 + 5, returnPaint);

            return startY + headerHeight + 10;
        }

        /// <summary>
        /// 绘制时间提醒卡片
        /// </summary>
        private float DrawTimeReminderCard(SKCanvas canvas, float startY)
        {
            var cardHeight = 80f;
            var padding = 20f;
            var cardMargin = 15f;

            // 绘制卡片背景
            var cardRect = new SKRect(cardMargin, startY, ImageWidth - cardMargin, startY + cardHeight);
            DrawRoundedRect(canvas, cardRect, 8, CardBackgroundColor);

            // 绘制主标题
            using var titlePaint = new SKPaint
            {
                Color = SecondaryColor,
                TextSize = 18,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Bold)
            };

            var titleText = "请在规定时间内寄回商品";
            canvas.DrawText(titleText, cardMargin + padding, startY + 30, titlePaint);

            // 绘制倒计时
            using var timePaint = new SKPaint
            {
                Color = TextGrayColor,
                TextSize = 14,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei")
            };

            var timeText = "还剩3天11时40分";
            canvas.DrawText(timeText, cardMargin + padding, startY + 55, timePaint);

            return startY + cardHeight + 10;
        }

        /// <summary>
        /// 绘制退货地址卡片
        /// </summary>
        private float DrawReturnAddressCard(SKCanvas canvas, float startY, RecipientInfo recipient)
        {
            var cardHeight = 180f; // 增加高度以容纳多行地址
            var padding = 20f;
            var cardMargin = 15f;

            // 绘制卡片背景
            var cardRect = new SKRect(cardMargin, startY, ImageWidth - cardMargin, startY + cardHeight);
            DrawRoundedRect(canvas, cardRect, 8, CardBackgroundColor);

            // 绘制标题和复制按钮
            using var titlePaint = new SKPaint
            {
                Color = SecondaryColor,
                TextSize = 18,
                IsAntialias = true,
                Typeface = CreateChineseTypeface(SKFontStyle.Bold)
            };

            var titleText = "退货地址:";
            canvas.DrawText(titleText, cardMargin + padding, startY + 30, titlePaint);

            // 复制按钮
            using var copyPaint = new SKPaint
            {
                Color = PrimaryColor,
                TextSize = 14,
                IsAntialias = true,
                Typeface = CreateChineseTypeface()
            };

            var copyText = "复制";
            var copyBounds = new SKRect();
            copyPaint.MeasureText(copyText, ref copyBounds);
            canvas.DrawText(copyText, ImageWidth - cardMargin - padding - copyBounds.Width, startY + 27, copyPaint);
            
            // 绘制收件人信息
            using var infoPaint = new SKPaint
            {
                Color = SecondaryColor,
                TextSize = 16,
                IsAntialias = true,
                Typeface = CreateChineseTypeface()
            };

            var currentInfoY = startY + 55;
            canvas.DrawText(recipient.Name, cardMargin + padding, currentInfoY, infoPaint);
            currentInfoY += 25;
            canvas.DrawText(recipient.Phone, cardMargin + padding, currentInfoY, infoPaint);
            currentInfoY += 25;

            // 地址可能需要换行
            DrawMultiLineText(canvas, recipient.FullAddress, cardMargin + padding, currentInfoY,
                ImageWidth - cardMargin * 2 - padding * 2, infoPaint);

            return startY + cardHeight + 10;
        }

        /// <summary>
        /// 绘制服务说明卡片
        /// </summary>
        private float DrawServiceCard(SKCanvas canvas, float startY)
        {
            var cardHeight = 80f;
            var padding = 20f;
            var cardMargin = 15f;

            // 绘制卡片背景
            var cardRect = new SKRect(cardMargin, startY, ImageWidth - cardMargin, startY + cardHeight);
            DrawRoundedRect(canvas, cardRect, 8, CardBackgroundColor);

            // 绘制服务标签
            using var servicePaint = new SKPaint
            {
                Color = PrimaryColor,
                TextSize = 16,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Bold)
            };

            var serviceText = "上门取件/快递柜/驿站";
            canvas.DrawText(serviceText, cardMargin + padding, startY + 30, servicePaint);

            // 绘制免运费标签
            using var freePaint = new SKPaint
            {
                Color = YellowColor,
                TextSize = 14,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei")
            };

            var freeText = "退换免运费";
            var serviceBounds = new SKRect();
            servicePaint.MeasureText(serviceText, ref serviceBounds);
            canvas.DrawText(freeText, cardMargin + padding + serviceBounds.Width + 10, startY + 28, freePaint);

            // 绘制服务描述
            using var descPaint = new SKPaint
            {
                Color = TextGrayColor,
                TextSize = 14,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei")
            };

            var descText = "2小时上门 · 免填地址 · 极速退款";
            canvas.DrawText(descText, cardMargin + padding, startY + 55, descPaint);

            return startY + cardHeight + 10;
        }
            
        /// <summary>
        /// 绘制我要寄件按钮
        /// </summary>
        private float DrawShipButton(SKCanvas canvas, float startY)
        {
            var buttonHeight = 50f;
            var buttonMargin = 30f;

            // 绘制按钮背景
            var buttonRect = new SKRect(buttonMargin, startY, ImageWidth - buttonMargin, startY + buttonHeight);
            DrawRoundedRect(canvas, buttonRect, 25, PrimaryColor);

            // 绘制按钮文字
            using var buttonPaint = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 18,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Bold)
            };

            var buttonText = "我要寄件";
            var textBounds = new SKRect();
            buttonPaint.MeasureText(buttonText, ref textBounds);

            var textX = (ImageWidth - textBounds.Width) / 2;
            var textY = startY + buttonHeight / 2 - textBounds.Top / 2;

            canvas.DrawText(buttonText, textX, textY, buttonPaint);

            return startY + buttonHeight + 20;
        }

        /// <summary>
        /// 绘制服务保障
        /// </summary>
        private float DrawServiceGuarantee(SKCanvas canvas, float startY)
        {
            using var titlePaint = new SKPaint
            {
                Color = SecondaryColor,
                TextSize = 16,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Bold)
            };

            var titleText = "服务保障";
            canvas.DrawText(titleText, 20, startY + 20, titlePaint);

            using var descPaint = new SKPaint
            {
                Color = TextGrayColor,
                TextSize = 14,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei")
            };

            var descText = "运费保障（含运费险）退换货自动理赔";
            canvas.DrawText(descText, 20, startY + 45, descPaint);

            return startY + 70;
        }
            
        /// <summary>
        /// 绘制底部按钮
        /// </summary>
        private void DrawBottomButtons(SKCanvas canvas, float startY)
        {
            var buttonHeight = 45f;
            var buttonMargin = 20f;
            var buttonSpacing = 10f;
            var buttonWidth = (ImageWidth - buttonMargin * 2 - buttonSpacing * 2) / 3;

            // 平台介入按钮（黄色）
            var button1Rect = new SKRect(buttonMargin, startY, buttonMargin + buttonWidth, startY + buttonHeight);
            DrawRoundedRect(canvas, button1Rect, 22, YellowColor);
            DrawButtonText(canvas, "平台介入", button1Rect, SecondaryColor);

            // 取消退货按钮（灰色）
            var button2Rect = new SKRect(buttonMargin + buttonWidth + buttonSpacing, startY,
                buttonMargin + buttonWidth * 2 + buttonSpacing, startY + buttonHeight);
            DrawRoundedRect(canvas, button2Rect, 22, GrayColor);
            DrawButtonText(canvas, "取消退货", button2Rect, SKColors.White);

            // 修改退货按钮（红色）
            var button3Rect = new SKRect(buttonMargin + buttonWidth * 2 + buttonSpacing * 2, startY,
                ImageWidth - buttonMargin, startY + buttonHeight);
            DrawRoundedRect(canvas, button3Rect, 22, PrimaryColor);
            DrawButtonText(canvas, "修改退货", button3Rect, SKColors.White);
        }

        /// <summary>
        /// 绘制按钮文字
        /// </summary>
        private void DrawButtonText(SKCanvas canvas, string text, SKRect buttonRect, SKColor textColor)
        {
            using var textPaint = new SKPaint
            {
                Color = textColor,
                TextSize = 14,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Microsoft YaHei", SKFontStyle.Bold)
            };

            var textBounds = new SKRect();
            textPaint.MeasureText(text, ref textBounds);

            var textX = buttonRect.Left + (buttonRect.Width - textBounds.Width) / 2;
            var textY = buttonRect.Top + (buttonRect.Height - textBounds.Height) / 2 - textBounds.Top;

            canvas.DrawText(text, textX, textY, textPaint);
        }



        /// <summary>
        /// 绘制多行文本
        /// </summary>
        private void DrawMultiLineText(SKCanvas canvas, string text, float x, float y, float maxWidth, SKPaint paint)
        {
            if (string.IsNullOrEmpty(text)) return;

            var lines = new List<string>();
            var currentLine = "";
            var lineHeight = 25f;

            // 按字符逐个添加，测试宽度
            for (int i = 0; i < text.Length; i++)
            {
                var testLine = currentLine + text[i];
                var testWidth = paint.MeasureText(testLine);

                if (testWidth > maxWidth && !string.IsNullOrEmpty(currentLine))
                {
                    // 当前行已满，添加到行列表
                    lines.Add(currentLine);
                    currentLine = text[i].ToString();
                }
                else
                {
                    currentLine = testLine;
                }
            }

            // 添加最后一行
            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
            }

            // 绘制所有行
            var currentY = y;
            foreach (var line in lines)
            {
                canvas.DrawText(line, x, currentY, paint);
                currentY += lineHeight;
            }
        }

        /// <summary>
        /// 创建中文字体
        /// </summary>
        private SKTypeface CreateChineseTypeface(SKFontStyle? style = null)
        {
            // 如果style为null，使用Normal样式
            var fontStyle = style ?? SKFontStyle.Normal;

            // 尝试多种中文字体，确保兼容性
            var fontNames = new[] { "Microsoft YaHei", "SimHei", "SimSun", "Arial Unicode MS", "sans-serif" };

            foreach (var fontName in fontNames)
            {
                try
                {
                    var typeface = SKTypeface.FromFamilyName(fontName, fontStyle);
                    if (typeface != null)
                    {
                        return typeface;
                    }
                }
                catch
                {
                    // 如果创建字体失败，继续尝试下一个
                    continue;
                }
            }

            // 如果都失败了，返回默认字体
            return SKTypeface.Default;
        }

        /// <summary>
        /// 保存图片到文件
        /// </summary>
        public void SaveImageToFile(byte[] imageData, string filePath)
        {
            File.WriteAllBytes(filePath, imageData);
        }
    }
}
