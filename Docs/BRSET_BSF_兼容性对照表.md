# BRSET与BSF格式兼容性对照表

> 更新日期：2026-03-22
> 转换器版本：FilterPresetConverter 1.4

**格式说明：**
- **BRSET**: BeatSpider 下载器使用的预设格式
- **BSF**: BSIMM 谱面管理工具使用的预设格式

---

## 一、已完全实现转换的字段

| BRSET字段 | BSF条件类型 | 类型值 | 说明 |
|-----------|-------------|--------|------|
| BPM | BpmRange | 200 | 范围条件(min-max) |
| 方块密度 (NPS) | NpsRange | 201 | 范围条件 |
| 歌曲时长 | DurationRange | 202 | 范围条件 |
| 难度星级 | SsStarsRange | 203 | SS星级范围 |
| 下载量 | DownloadsRange | 207 | 范围条件 |
| 游戏次数 | PlaysRange | 206 | 范围条件 |
| 点赞数量 | UpvotesRange | 208 | 范围条件 |
| 点赞比例 | UpvoteRatioRange | 210 | 范围条件 |
| 点踩数量 | DownvotesRange | 209 | 范围条件 |
| 点踩比例 | DownvoteRatioRange | 211 | 范围条件 |
| 总评分 | ScoreRange | 205 | 范围条件，用户输入0-100，API返回0-1 |
| Sage分数 | SageScoreRange | 212 | 范围条件 |
| 最高分数 | MaxScoreRange | 225 | 范围条件 |
| 飞行速度 (NJS) | NjsRange | 213 | 范围条件 |
| 偏移值 | OffsetRange | 215 | 范围条件 |
| 炸弹数量 | BombsRange | 214 | 范围条件 |
| 灯光事件 | EventsRange | 216 | 范围条件 |
| 墙壁数量 | ObstaclesRange | 217 | 范围条件 |
| 谱面时长 | SecondsRange | 220 | 范围条件 |
| 节拍/方块数量 | NotesRange | 219 | 范围条件 |
| 校验错误 | ParityErrorsRange | 222 | 范围条件 |
| 校验警告 | ParityWarnsRange | 223 | 范围条件 |
| 校验重置 | ParityResetsRange | 224 | 范围条件 |
| 上传时间 | MinUploadedDate | 140 | 支持日期字符串/Unix时间戳/天数偏移 |
| 上传时间(最大) | MaxUploadedDate | 141 | 同上 |
| 搜索词 | Query | 0 | 全文搜索（无范围选项时使用） |
| 搜索标题 | SearchTitle | 122 | 搜索标题字段 |
| 搜索歌名 | SearchSongName | 123 | 搜索歌名字段 |
| 搜索作者 | SearchAuthor | 124 | 搜索作者字段 |
| 搜索谱师 | SearchMapper | 125 | 搜索谱师字段 |
| 搜索介绍 | SearchDescription | 126 | 搜索介绍字段 |
| 上传者名称 | UploaderName | 115 | 文本条件 |
| 上传者ID | UploaderName | 115 | 映射到同一字段 |
| 包含模式 | Characteristic | 138 | Standard/Lawless/Lightshow等 |
| 包含难度 | Difficulty | 139 | Easy/Normal/Hard/Expert/ExpertPlus |
| 标签(包含) | Tags | 114 | 支持多标签(逗号分隔)，支持And/Or逻辑 |
| 标签(排除) | ExcludeTags | 180 | 支持多标签，支持And/Or逻辑 |
| 自动生成 | Automapper | 30 | 1=仅AI谱, 2=排除AI谱 |
| 排位曲 | Ranked | 100 | 布尔条件 |
| 需求组件(Chroma) | Chroma | 25 | 布尔条件 |
| 需求组件(Noodle/NE) | Ne | 150 | 布尔条件 |
| 需求组件(ME) | Me | 27 | 布尔条件 |
| 需求组件(Cinema) | Cinema | 28 | 布尔条件 |
| 需求组件(Vivify) | Vivify | 29 | 布尔条件 |
| 需求组件(自定义) | CustomMod | 151 | 文本条件，Mod名称 |
| 排除组件(Chroma/NE/ME等) | 同上 | - | Value=false |
| 排除组件(自定义) | ExcludeCustomMod | 152 | 文本条件，Mod名称 |
| 下载限制 | ResultLimit | 145 | 包含数量和排序选项 |

---

## 二、BSF不支持的功能

| BRSET字段 | 说明 | 后续建议 |
|-----------|------|----------|
| 开始页数 | 分页控制 | 下载器特有功能，不适合作为筛选预设 |
| 全难度 | 下载全部难度 | 下载器特有功能 |
| 下载方式 | 下载配置 | 下载器特有功能 |
| 筛选中文 | 中文歌曲筛选 | BSF无此筛选类型，需新增 Chinese 类型 |
| 高级搜索 | 高级搜索开关 | 已废弃，搜索范围现由独立的搜索类型支持 |
| 正则搜索 | 正则表达式搜索 | BSF Query不支持正则表达式 |

---

## 三、下载器特有配置（非筛选条件）

这些配置项属于下载器行为设置，不适合转换为筛选预设：

| BRSET节点 | 字段 | 功能说明 |
|-----------|------|----------|
| MapperProfile | 地址 | 谱师个人页面下载 |
| BeastSaber | 地址/开始页数 | BeastSaber网站下载 |
| SongList | 路径 | 歌曲列表文件导入 |
| Manual | 手动下载歌曲 | 手动输入歌曲ID/链接 |
| LocalSongDirectory | 目录/跳过/现行子夹 | 本地歌曲目录处理 |
| SaveMethod | 歌曲列表/下载歌曲/命名方式 | 保存路径和命名配置 |
| CoverTag | 封面标签识别 | AI封面识别功能配置 |
| LocalCache | 本地/在线缓存 | 缓存源设置 |
| DownloadMethod | 下载方式选择 | 下载行为配置 |

---

## 四、部分支持/需要改进

| BRSET字段 | 当前状态 | 问题说明 |
|-----------|----------|----------|
| 上传者ID vs 名称 | 合并 | 两者都映射到UploaderName，BSF不区分ID和名称 |

---

## 五、后续开发建议

### 优先级中（功能增强）

1. **筛选中文**
   - 建议：BSF新增 `Chinese` 筛选类型

### 优先级低（下载器功能）

2. 下载方式、保存方式、封面标签等
   - 这些是下载器特有功能，不适合作为筛选预设的一部分
   - 如需支持，考虑在BSF中新增"下载配置"独立模块

---

## 六、技术细节

### Range类型条件格式

```json
{
  "Type": 200,
  "Value": {
    "Min": 100.0,
    "Max": 160.0,
    "HasValue": true
  }
}
```

### 上传时间格式支持

BRSET上传时间支持三种格式：
- 日期字符串：`"2022-12-11"`
- Unix时间戳：`1670774400`（秒）
- 天数偏移：`30`（30天前）

转换后统一为BSF日期格式。

### 搜索过滤转换逻辑

BRSET的搜索过滤配置映射规则：
- `搜索开关=false` 或 `搜索内容为空`：跳过，不生成条件
- 无任何范围选项被选中：生成 `Query` 条件（全文搜索）
- 有范围选项被选中：生成对应的搜索类型条件

| BRSET搜索选项 | BSF条件类型 | 类型值 |
|---------------|-------------|--------|
| 搜索标题 | SearchTitle | 122 |
| 搜索歌名 | SearchSongName | 123 |
| 搜索作者 | SearchAuthor | 124 |
| 搜索谱师 | SearchMapper | 125 |
| 搜索介绍 | SearchDescription | 126 |

示例（搜索歌名"Miku"）：
```json
{
  "Type": 123,
  "Value": "Miku",
  "Operator": 0,
  "IsEnabled": true
}
```

示例（同时搜索标题和作者"Camellia"）：
```json
{
  "Type": 122,
  "Value": "Camellia",
  "Operator": 0,
  "IsEnabled": true
},
{
  "Type": 124,
  "Value": "Camellia",
  "Operator": 0,
  "IsEnabled": true
}
```

### 条件优先级

当BeatSaver节点和FilterItems节点有重复字段时：
- FilterItems（筛选项目）优先级更高
- 避免生成重复条件

### 标签And/Or逻辑

BRSET的标签支持"与"/"或"配置，转换时映射到BSF条件的Operator属性：
- BRSET "或"=true → 后续标签条件 `Operator = Or`（任一标签匹配即可）
- BRSET "与"=true 或默认 → 后续标签条件 `Operator = And`（必须匹配所有标签）

示例（Or逻辑）：
```json
{
  "Type": 114,
  "Value": "anime",
  "Operator": 0  // And (第一个条件)
},
{
  "Type": 114,
  "Value": "kpop",
  "Operator": 1  // Or (匹配anime OR kpop)
}
```

---

## 七、版本历史

| 日期 | 版本 | 更新内容 |
|------|------|----------|
| 2026-03-21 | 1.4 | 新增搜索范围支持：SearchTitle(122)/SearchSongName(123)/SearchAuthor(124)/SearchMapper(125)/SearchDescription(126) |
| 2026-03-21 | 1.3 | 新增搜索词→Query转换支持 |
| 2026-03-21 | 1.2 | 新增ExcludeCustomMod支持，排除自定义Mod已完全支持 |
| 2026-03-21 | 1.1 | 修正Tags And/Or逻辑说明，现已支持转换 |
| 2026-03-21 | 1.0 | 初始版本，基于Range类型条件更新 |