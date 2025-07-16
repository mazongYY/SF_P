# 顺丰退货订单生成器

<div align="center">

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)
![Language](https://img.shields.io/badge/language-C%23-green.svg)

一个现代化的 Windows 桌面应用程序，用于生成电商平台退货订单图片，支持智能地址识别和随机数据生成。

[功能特性](#-功能特性) • [快速开始](#-快速开始) • [详细说明](#-详细说明) • [技术架构](#-技术架构) • [开发指南](#-开发指南)

</div>

---

## ✨ 功能特性

### 🎯 核心功能

| 功能            | 描述                                   | 状态 |
| --------------- | -------------------------------------- | ---- |
| 🔍 智能地址识别 | 自动解析收件信息，提取姓名、电话、地址 | ✅   |
| ✏️ 手动信息编辑 | 支持手动输入和修改收件人信息           | ✅   |
| 🎲 随机数据生成 | 自动生成订单号、退货原因、商品信息     | ✅   |
| 🖼️ 图片生成     | 生成手机尺寸的退货订单图片             | ✅   |
| 💾 一键保存     | PNG 格式图片保存和预览                 | ✅   |

### 📱 界面特色

- 🎨 **现代化设计**：基于 WPF 的现代化界面
- 📋 **分组布局**：清晰的功能分区，操作直观
- 🔄 **实时反馈**：状态提示和进度显示
- 📱 **响应式布局**：适配不同屏幕尺寸

## 🚀 快速开始

### � 安装运行

#### 方式一：直接运行（推荐）

1. 下载 `publish-new` 文件夹中的可执行文件
2. 双击 `ReturnOrderGenerator.exe` 即可运行
3. 无需安装 .NET 运行时，开箱即用

#### 方式二：源码编译

```bash
# 克隆项目
git clone <repository-url>
cd ReturnOrderGenerator

# 还原依赖
dotnet restore

# 运行程序
dotnet run
```

### 📖 使用说明

#### 1️⃣ 地址信息解析

- 在"地址信息解析"区域粘贴收件信息
- 点击"解析地址信息"按钮自动提取信息
- 系统会自动识别姓名、电话、省市区地址

#### 2️⃣ 完善收件人信息

- 检查并完善自动识别的收件人信息
- 可手动修改姓名、电话、地址等字段
- 使用"清空信息"按钮重置收件人信息

#### 3️⃣ 订单信息管理

- 点击"生成随机数据"获取随机订单信息
- 可手动修改订单号、退货原因、商品信息等
- 系统会自动计算商品总价

#### 4️⃣ 生成退货订单图片

- 确认所有信息填写完整
- 点击"生成退货订单图片"按钮
- 选择保存位置，系统生成 PNG 格式图片
- 可选择立即预览生成的图片

## 📋 详细说明

### 📝 地址识别格式

系统支持多种地址信息格式的智能识别：

```text
# 格式一：空格分隔
张三 13812345678 广东省深圳市福田区华强北路1号

# 格式二：逗号分隔
李四，电话：13987654321，地址：北京市朝阳区建国门外大街2号

# 格式三：混合格式
王五 上海市徐汇区淮海中路3号 手机：13611111111
```

### 🎲 随机数据生成

| 数据类型 | 生成规则                          | 示例                     |
| -------- | --------------------------------- | ------------------------ |
| 订单号   | TB + 日期 + 6 位随机数字          | TB20250116123456         |
| 退货原因 | 15 种常见原因随机选择             | 商品质量问题             |
| 商品信息 | 25 种商品 × 15 种规格 × 25 种品牌 | iPhone 15 Pro 256GB 苹果 |
| 商品价格 | 10-500 元随机价格                 | ¥299.00                  |
| 商品数量 | 1-3 个随机数量                    | 2 件                     |

### 🖼️ 图片输出规格

| 属性 | 规格           | 说明           |
| ---- | -------------- | -------------- |
| 尺寸 | 430 × 932 像素 | 手机屏幕比例   |
| 格式 | PNG            | 支持透明背景   |
| DPI  | 96             | 标准屏幕分辨率 |
| 内容 | 完整订单信息   | 包含时间戳     |

## 🏗️ 技术架构

### 🛠️ 技术栈

| 技术                   | 版本    | 用途           |
| ---------------------- | ------- | -------------- |
| .NET                   | 9.0     | 应用程序框架   |
| WPF                    | -       | 桌面应用界面   |
| MVVM                   | -       | 架构设计模式   |
| SkiaSharp              | 3.119.0 | 图像处理和生成 |
| Microsoft.Toolkit.Mvvm | 7.1.2   | MVVM 框架支持  |

### 📁 项目结构

```
ReturnOrderGenerator/
├── Models/              # 数据模型
│   ├── RecipientInfo.cs        # 收件人信息模型
│   ├── OrderInfo.cs            # 订单信息模型
│   └── AddressParseResult.cs   # 地址解析结果模型
├── ViewModels/          # 视图模型
│   └── MainViewModel.cs        # 主界面视图模型
├── Views/               # 界面视图
│   └── MainWindow.xaml         # 主窗口
├── Services/            # 业务服务
│   ├── AddressParseService.cs  # 地址解析服务
│   ├── RandomDataService.cs    # 随机数据生成服务
│   └── ImageGenerationService.cs # 图片生成服务
├── Utils/               # 工具类
│   └── Converters.cs           # 数据转换器
├── Data/                # 数据文件
│   └── ChinaRegions.json       # 中国行政区划数据
└── Resources/           # 资源文件
```

## 🛠️ 开发指南

### 📋 系统要求

| 组件     | 版本要求                     | 说明       |
| -------- | ---------------------------- | ---------- |
| 操作系统 | Windows 10/11                | 64 位系统  |
| .NET     | 9.0                          | 运行时环境 |
| IDE      | Visual Studio 2022 / VS Code | 开发环境   |

### 🔧 开发依赖

```xml
<PackageReference Include="SkiaSharp" Version="3.119.0" />
<PackageReference Include="SkiaSharp.Views.WPF" Version="3.119.0" />
<PackageReference Include="Microsoft.Toolkit.Mvvm" Version="7.1.2" />
```

### 🏗️ 构建和发布

```bash
# 克隆项目
git clone <repository-url>
cd ReturnOrderGenerator

# 还原依赖
dotnet restore

# 编译项目
dotnet build -c Release

# 运行程序
dotnet run

# 发布自包含程序
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

## 📝 更新日志

### v1.0.0 (2025-01-16)

- ✅ 实现智能地址识别功能
- ✅ 实现随机数据生成功能
- ✅ 实现高质量图片生成功能
- ✅ 完成现代化 WPF 界面开发
- ✅ 支持 PNG 格式图片导出
- ✅ 自包含部署，无需安装运行时

## 🤝 贡献指南

欢迎提交 Issue 和 Pull Request 来改进项目！

### 提交规范

- 🐛 **Bug 修复**：`fix: 修复地址解析问题`
- ✨ **新功能**：`feat: 添加批量生成功能`
- 📝 **文档**：`docs: 更新 README`
- 🎨 **样式**：`style: 优化界面布局`

## 📄 许可证

本项目采用 [MIT License](LICENSE) 开源协议。

## 📞 联系方式

- 📧 **邮箱**：[your-email@example.com](mailto:your-email@example.com)
- 🐛 **问题反馈**：[GitHub Issues](https://github.com/your-username/ReturnOrderGenerator/issues)
- 💡 **功能建议**：[GitHub Discussions](https://github.com/your-username/ReturnOrderGenerator/discussions)

---

<div align="center">

**⭐ 如果这个项目对你有帮助，请给个 Star 支持一下！**

Made with ❤️ by [Your Name]

</div>
