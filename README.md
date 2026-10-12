# slay_mod：借据（IOU）

一个《杀戮尖塔2》mod，添加遗物「借据」：可以在商店赊账购物，代价是负债期间战斗金币减少。

## 遗物设计

**借据**（IOU）· 商店遗物 · 价格 33 金币

> 在商店购物时可以赊账，最多欠款 **100** 金币。
> 负债期间，战斗获得的金币减少 **30%**；欠款超过 **50** 时，改为减少 **50%**。
> 还清欠款后，借据失效，不能再赊账。

*「商人总是很乐意借钱给你……利息嘛，另算。」*

### 规则细节

| # | 规则 |
|---|---|
| 1 | 金币可以变成负数，之后获得的金币自动抵债，回到 0 及以上即为还清。**备选**：如果游戏无法让金币显示为负数，金币停在 0，欠款记在遗物上并写进遗物介绍里，获得金币时同样自动先还债 |
| 2 | 惩罚比例按**当前**欠款计算：欠款 1–50 减 30%，51–100 减 50% |
| 3 | 金币最低为 -100；还掉一部分但还没还清时，可以继续借 |
| 4 | 只减少战斗奖励金币（普通/精英/Boss），事件、遗物给的金币不减；结果向下取整 |
| 5 | 所有商店消费都可以赊账（卡牌、遗物、药水、删牌服务）；商店以外不能赊账 |
| 6 | 借据在每个商店都会出现，直到被买下。优先作为额外格子放在删牌服务旁边/上方；做不到则替换一个遗物格。不会从宝箱、精英奖励等其他途径出现 |
| 7 | 价格 33（打折前），打折效果对借据正常生效 |
| 8 | 失去金币的效果（被偷、事件扣钱）不会让欠款增加：金币不足时最多扣到 0 |
| 9 | 遗物图标上显示当前欠款；悬停显示当前惩罚比例；金币为负时显示为红色 |
| 10 | 欠款还清（金币从负数回到 0 或以上）后，借据失效（图标变灰），本局不能再赊账 |

## 开发环境（Windows）

- .NET SDK 9 或更新
- Godot **4.5.1 .NET 版**（必须与游戏的 Godot 版本一致）
- 游戏安装在 `C:\Steam\steamapps\common\Slay the Spire 2`
  （不在这里的话，修改 `IOUMod.csproj` 里的 `Sts2DataDir`，以及 `install.ps1` 里的 `GameDir`）

`sts2.dll` 直接从游戏目录引用，**不会**复制进仓库（它是游戏的版权文件）。

本 mod 依赖 **BaseLib**：在 Steam 创意工坊订阅 [BaseLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127)，测试时在游戏的 mod 列表里同时启用 BaseLib 和 IOU。

## 编译和安装

1. 在项目文件夹里新建 `godot_path.txt`，只写一行：你电脑上 Godot 的 **`_console.exe`** 完整路径。
   （这个文件不会提交到仓库，每台电脑各写各的。）
2. 在项目文件夹里打开 PowerShell，运行：
   ```powershell
   powershell -ExecutionPolicy Bypass -File build.ps1
   powershell -ExecutionPolicy Bypass -File install.ps1
   ```
3. 启动游戏，在 mod 列表里启用 **BaseLib** 和 **IOU**。

## 上传到 Steam 创意工坊

使用 Mega Crit 官方的 [ModUploader](https://github.com/megacrit/sts2-mod-uploader)。

1. 在项目文件夹里新建 `uploader_path.txt`，只写一行：`ModUploader.exe` 的完整路径（不会提交到仓库）。
2. 确保 Steam 已经打开并登录。
3. 先运行 `build.ps1`，再运行：
   ```powershell
   powershell -ExecutionPolicy Bypass -File publish.ps1
   ```
4. 第一次上传后会生成 `workshop/mod_id.txt`（创意工坊物品编号），**要提交到仓库**，以后更新才会传到同一个物品。

标题、简介、可见性（默认 `private`）在 `workshop/workshop.json` 里；预览图是 `workshop/image.png`（必须小于 1MB）。

## 文件说明

| 文件 | 作用 |
|---|---|
| `IOUMod.json` | mod 说明文件（名字、版本、作者） |
| `IOUMod.csproj` | C# 项目配置，引用游戏的 `sts2.dll`、Harmony 和 BaseLib |
| `Sts2PathDiscovery.props` | 自动查找游戏安装位置 |
| `IOUModCode/MainFile.cs` | mod 入口，加载时启用所有补丁 |
| `IOUModCode/IouRelic.cs` | 借据遗物：介绍文字、欠款计数、战斗金币减少 |
| `IOUModCode/Patches/GoldPatches.cs` | 允许商店消费让金币变负；负债时其他扣钱无效；标记战斗奖励金币 |
| `IOUModCode/Patches/MerchantPatches.cs` | 每个商店加一个借据格子（删牌服务上方）；固定价格 33；赊账额度 |
| `IOUModCode/Patches/TopBarGoldPatch.cs` | 金币为负时顶部金币显示为红色 |
| `IOUMod/` | 资源文件（图标等），会打包进 `.pck` |
| `project.godot`、`export_presets.cfg` | Godot 项目和导出设置 |
| `build.ps1`、`install.ps1` | 编译、安装脚本 |
| `publish.ps1`、`workshop/` | 上传创意工坊的脚本、说明和预览图 |
