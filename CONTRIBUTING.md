# 🤝 贡献指南

感谢您对 **顺丰退货订单生成器** 项目的关注！我们欢迎所有形式的贡献。

## 📋 目录

- [行为准则](#-行为准则)
- [如何贡献](#-如何贡献)
- [开发环境设置](#-开发环境设置)
- [提交规范](#-提交规范)
- [Pull Request 流程](#-pull-request-流程)
- [问题报告](#-问题报告)
- [功能请求](#-功能请求)

## 🤝 行为准则

参与此项目即表示您同意遵守我们的行为准则。请确保在所有互动中保持尊重和建设性。

## 🚀 如何贡献

### 贡献类型

我们欢迎以下类型的贡献：

- 🐛 **Bug 修复**
- ✨ **新功能开发**
- 📝 **文档改进**
- 🧪 **测试用例**
- 🎨 **UI/UX 改进**
- 🔧 **性能优化**
- 🌐 **国际化支持**

### 贡献流程

1. **Fork 项目**
   ```bash
   # 点击 GitHub 页面右上角的 Fork 按钮
   ```

2. **克隆到本地**
   ```bash
   git clone https://github.com/your-username/ReturnOrderGenerator.git
   cd ReturnOrderGenerator
   ```

3. **创建功能分支**
   ```bash
   git checkout -b feature/your-feature-name
   # 或者
   git checkout -b fix/your-bug-fix
   ```

4. **进行开发**
   - 编写代码
   - 添加测试
   - 更新文档

5. **提交更改**
   ```bash
   git add .
   git commit -m "feat: 添加新功能描述"
   ```

6. **推送分支**
   ```bash
   git push origin feature/your-feature-name
   ```

7. **创建 Pull Request**
   - 在 GitHub 上创建 PR
   - 填写 PR 模板
   - 等待代码审查

## 🛠️ 开发环境设置

### 系统要求

- **操作系统**: Windows 10/11
- **.NET SDK**: 9.0 或更高版本
- **IDE**: Visual Studio 2022 或 VS Code

### 环境配置

1. **安装 .NET SDK**
   ```bash
   # 下载并安装 .NET 9.0 SDK
   # https://dotnet.microsoft.com/download
   ```

2. **验证安装**
   ```bash
   dotnet --version
   ```

3. **克隆项目**
   ```bash
   git clone https://github.com/your-username/ReturnOrderGenerator.git
   cd ReturnOrderGenerator
   ```

4. **还原依赖**
   ```bash
   dotnet restore
   ```

5. **构建项目**
   ```bash
   dotnet build
   ```

6. **运行项目**
   ```bash
   dotnet run
   ```

### 开发工具推荐

- **Visual Studio 2022**: 完整的 IDE 体验
- **VS Code**: 轻量级编辑器
- **Git**: 版本控制
- **GitHub Desktop**: Git 图形界面（可选）

## 📝 提交规范

我们使用 [Conventional Commits](https://www.conventionalcommits.org/) 规范：

### 提交格式

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### 提交类型

| 类型 | 描述 | 示例 |
|------|------|------|
| `feat` | 新功能 | `feat: 添加批量生成功能` |
| `fix` | Bug 修复 | `fix: 修复地址解析错误` |
| `docs` | 文档更新 | `docs: 更新 README` |
| `style` | 代码格式 | `style: 修复代码缩进` |
| `refactor` | 代码重构 | `refactor: 重构地址解析逻辑` |
| `test` | 测试相关 | `test: 添加单元测试` |
| `chore` | 构建/工具 | `chore: 更新依赖包` |
| `perf` | 性能优化 | `perf: 优化图片生成速度` |

### 提交示例

```bash
# 好的提交信息
git commit -m "feat: 添加地址智能识别功能"
git commit -m "fix: 修复窗口图标显示问题"
git commit -m "docs: 更新安装说明"

# 不好的提交信息
git commit -m "修改了一些东西"
git commit -m "bug fix"
git commit -m "update"
```

## 🔄 Pull Request 流程

### PR 检查清单

提交 PR 前请确保：

- [ ] 代码遵循项目编码规范
- [ ] 所有测试通过
- [ ] 添加了必要的测试用例
- [ ] 更新了相关文档
- [ ] PR 描述清晰明了
- [ ] 关联了相关的 Issue

### PR 模板

请使用项目提供的 PR 模板，包含：

- 变更类型和描述
- 相关 Issue 链接
- 测试说明
- 截图（如适用）
- 检查清单

### 代码审查

- 所有 PR 需要至少一个维护者的审查
- 请耐心等待审查反馈
- 根据反馈及时修改代码
- 保持 PR 的简洁和专注

## 🐛 问题报告

### 报告 Bug

使用 [Bug 报告模板](.github/ISSUE_TEMPLATE/bug_report.md) 创建 Issue：

1. 清晰的标题
2. 详细的问题描述
3. 复现步骤
4. 预期行为
5. 实际行为
6. 环境信息
7. 截图或日志

### Bug 优先级

- **P0 - 紧急**: 应用崩溃、数据丢失
- **P1 - 高**: 核心功能无法使用
- **P2 - 中**: 功能异常但有替代方案
- **P3 - 低**: 小问题、优化建议

## 💡 功能请求

### 提交功能请求

使用 [功能请求模板](.github/ISSUE_TEMPLATE/feature_request.md)：

1. 功能描述
2. 使用场景
3. 预期收益
4. 实现建议
5. 替代方案

### 功能评估标准

- **用户价值**: 对用户的实际帮助
- **技术可行性**: 实现的复杂度
- **维护成本**: 长期维护的成本
- **项目一致性**: 与项目目标的一致性

## 🏷️ 标签说明

| 标签 | 描述 |
|------|------|
| `bug` | Bug 报告 |
| `enhancement` | 功能增强 |
| `documentation` | 文档相关 |
| `good first issue` | 适合新手 |
| `help wanted` | 需要帮助 |
| `question` | 问题咨询 |
| `wontfix` | 不会修复 |
| `duplicate` | 重复问题 |

## 🎯 开发指导原则

### 代码质量

- **可读性**: 代码应该易于理解
- **可维护性**: 便于后续修改和扩展
- **性能**: 考虑性能影响
- **安全性**: 注意安全问题

### 设计原则

- **用户体验优先**: 以用户需求为中心
- **简洁性**: 保持界面和功能的简洁
- **一致性**: 保持设计和交互的一致性
- **可访问性**: 考虑不同用户的需求

## 📞 联系方式

如有任何问题，欢迎通过以下方式联系：

- 📧 **邮箱**: [your-email@example.com](mailto:your-email@example.com)
- 💬 **讨论**: [GitHub Discussions](https://github.com/your-username/ReturnOrderGenerator/discussions)
- 🐛 **问题**: [GitHub Issues](https://github.com/your-username/ReturnOrderGenerator/issues)

## 🙏 致谢

感谢所有为项目做出贡献的开发者！

---

**再次感谢您的贡献！** 🎉