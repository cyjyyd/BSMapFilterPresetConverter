using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FilterPresetConverter.Models;

namespace FilterPresetConverter.Services
{
    /// <summary>
    /// BRSET到BSF格式转换服务
    /// </summary>
    public class BrsetToBsfConverter
    {
        /// <summary>
        /// 转换结果
        /// </summary>
        public class ConversionResult
        {
            public bool Success { get; set; }
            public string Message { get; set; } = "";
            public FilterPreset? Preset { get; set; }
            public List<string> Warnings { get; set; } = new List<string>();
            public List<string> SkippedItems { get; set; } = new List<string>();
        }

        /// <summary>
        /// 从文件加载BRSET预设
        /// </summary>
        public BrsetPreset? LoadBrsetFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<BrsetPreset>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从JSON字符串加载BRSET预设
        /// </summary>
        public BrsetPreset? LoadBrsetFromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<BrsetPreset>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 执行转换
        /// </summary>
        public ConversionResult Convert(BrsetPreset brset, string? presetName = null)
        {
            var result = new ConversionResult();

            try
            {
                // 从文件名或参数获取预设名称
                var name = presetName ?? "Converted Preset";
                // 清理名称中的日期占位符和后缀
                name = System.Text.RegularExpressions.Regex.Replace(name, @"\s*更新至\[\[日期\]\].*", "");
                name = System.Text.RegularExpressions.Regex.Replace(name, @"@WGzeyu.*", "");
                name = name.Trim();

                var preset = new FilterPreset(name);
                var conditions = new List<FilterCondition>();

                // 收集筛选项目节点已启用的条件类型（优先保留筛选项目节点）
                var filterItemsEnabledTypes = CollectEnabledFilterItemTypes(brset.FilterItems);

                // 处理BeatSaver节点下的条件（跳过筛选项目已设置的字段）
                ConvertBeatSaverSection(brset.BeatSaver, conditions, result, filterItemsEnabledTypes);

                // 转换筛选项目 (FilterItems节点的条件，优先级最高)
                ConvertFilterItems(brset.FilterItems, conditions, result);

                // 转换ScoreSaber难度 (如果有)
                ConvertScoreSaber(brset.ScoreSaber, conditions, result);

                // 转换下载限制
                ConvertDownloadLimit(brset.DownloadLimit, conditions, result);

                // 转换搜索过滤
                ConvertSearchFilter(brset.SearchFilter, conditions, result);

                // 如果有条件，创建默认组
                if (conditions.Count > 0)
                {
                    var group = new FilterGroup("筛选条件")
                    {
                        Conditions = conditions,
                        UseLocalCache = true
                    };
                    preset.Groups.Add(group);
                }

                result.Preset = preset;
                result.Success = true;
                result.Message = $"成功转换 {conditions.Count} 个筛选条件";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"转换失败: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 收集筛选项目节点已启用的条件类型
        /// </summary>
        private HashSet<FilterConditionType> CollectEnabledFilterItemTypes(FilterItemsSection items)
        {
            var types = new HashSet<FilterConditionType>();

            if (items == null) return types;

            // 基本字段 - 使用Range类型
            if (items.BPM?.Enabled == true)
            {
                types.Add(FilterConditionType.BpmRange);
            }
            if (items.NPS?.Enabled == true)
            {
                types.Add(FilterConditionType.NpsRange);
            }
            if (items.SongDuration?.Enabled == true)
            {
                types.Add(FilterConditionType.DurationRange);
            }
            if (items.UploadTime?.Enabled == true)
            {
                types.Add(FilterConditionType.MinUploadedDate);
                types.Add(FilterConditionType.MaxUploadedDate);
            }

            // 统计字段 - 使用Range类型
            if (items.Downloads?.Enabled == true)
            {
                types.Add(FilterConditionType.DownloadsRange);
            }
            if (items.Plays?.Enabled == true)
            {
                types.Add(FilterConditionType.PlaysRange);
            }
            if (items.Upvotes?.Enabled == true)
            {
                types.Add(FilterConditionType.UpvotesRange);
            }
            if (items.UpvoteRatio?.Enabled == true)
            {
                types.Add(FilterConditionType.UpvoteRatioRange);
            }
            if (items.Downvotes?.Enabled == true)
            {
                types.Add(FilterConditionType.DownvotesRange);
            }
            if (items.DownvoteRatio?.Enabled == true)
            {
                types.Add(FilterConditionType.DownvoteRatioRange);
            }
            if (items.Score?.Enabled == true)
            {
                types.Add(FilterConditionType.ScoreRange);
            }
            if (items.SageScore?.Enabled == true)
            {
                types.Add(FilterConditionType.SageScoreRange);
            }
            if (items.MaxScore?.Enabled == true)
            {
                types.Add(FilterConditionType.MaxScoreRange);
            }

            // 难度参数 - 使用Range类型
            if (items.NJS?.Enabled == true)
            {
                types.Add(FilterConditionType.NjsRange);
            }
            if (items.Offset?.Enabled == true)
            {
                types.Add(FilterConditionType.OffsetRange);
            }
            if (items.Bombs?.Enabled == true)
            {
                types.Add(FilterConditionType.BombsRange);
            }
            if (items.Events?.Enabled == true)
            {
                types.Add(FilterConditionType.EventsRange);
            }

            // 新增字段 - 使用Range类型
            if (items.Walls?.Enabled == true)
            {
                types.Add(FilterConditionType.ObstaclesRange);
            }
            if (items.MapDuration?.Enabled == true)
            {
                types.Add(FilterConditionType.SecondsRange);
            }
            if (items.Notes?.Enabled == true || items.NoteCount?.Enabled == true)
            {
                types.Add(FilterConditionType.NotesRange);
            }
            if (items.VerifyErrors?.Enabled == true)
            {
                types.Add(FilterConditionType.ParityErrorsRange);
            }
            if (items.VerifyWarnings?.Enabled == true)
            {
                types.Add(FilterConditionType.ParityWarnsRange);
            }
            if (items.VerifyResets?.Enabled == true)
            {
                types.Add(FilterConditionType.ParityResetsRange);
            }

            // 内容字段
            if (items.UploaderId?.Enabled == true || items.UploaderName?.Enabled == true)
            {
                types.Add(FilterConditionType.UploaderName);
            }
            if (items.IncludeMode?.Enabled == true)
            {
                types.Add(FilterConditionType.Characteristic);
            }
            if (items.IncludeDifficulty?.Enabled == true)
            {
                types.Add(FilterConditionType.Difficulty);
            }
            if (items.RequiredMods?.Enabled == true)
            {
                types.Add(FilterConditionType.Chroma);
                types.Add(FilterConditionType.Ne);
                types.Add(FilterConditionType.Me);
                types.Add(FilterConditionType.Cinema);
                types.Add(FilterConditionType.Vivify);
                types.Add(FilterConditionType.CustomMod);
            }
            if (items.ExcludeMods?.Enabled == true)
            {
                types.Add(FilterConditionType.Chroma);
                types.Add(FilterConditionType.Ne);
                types.Add(FilterConditionType.Me);
                types.Add(FilterConditionType.Cinema);
                types.Add(FilterConditionType.Vivify);
                types.Add(FilterConditionType.ExcludeCustomMod);
            }

            // 标签
            if (items.Tags?.Include?.Enabled == true)
            {
                types.Add(FilterConditionType.Tags);
            }
            if (items.Tags?.Exclude?.Enabled == true)
            {
                types.Add(FilterConditionType.ExcludeTags);
            }

            // 其他
            if (items.AutoGenerated?.Enabled == true)
            {
                types.Add(FilterConditionType.Automapper);
            }
            if (items.Ranked?.Enabled == true)
            {
                types.Add(FilterConditionType.Ranked);
            }
            if (items.Stars?.Enabled == true)
            {
                types.Add(FilterConditionType.SsStarsRange);
            }

            return types;
        }

        private void ConvertBeatSaverSection(BeatSaverSection bs, List<FilterCondition> conditions, ConversionResult result, HashSet<FilterConditionType>? skipTypes = null)
        {
            // BeatSaver节点下的搜索词 -> Query (使用SearchQueryValue，默认搜索全部字段)
            if (!string.IsNullOrWhiteSpace(bs.SearchTerm) && !skipTypes?.Contains(FilterConditionType.Query) == true)
            {
                var queryValue = new SearchQueryValue(bs.SearchTerm, SearchFieldType.All);
                conditions.Add(new FilterCondition(FilterConditionType.Query, queryValue));
            }

            // BeatSaver节点下的排位曲
            if (bs.Ranked != null && bs.Ranked.Enabled && !skipTypes?.Contains(FilterConditionType.Ranked) == true)
            {
                conditions.Add(new FilterCondition(FilterConditionType.Ranked, bs.Ranked.RankedValue));
            }

            // BeatSaver节点下的自动生成
            // BSF使用字符串值: "仅AI谱" 或 "排除AI谱"
            if (bs.AutoGenerated != null && bs.AutoGenerated.Enabled && !skipTypes?.Contains(FilterConditionType.Automapper) == true)
            {
                string autoValue = bs.AutoGenerated.AutoGeneratedValue ? "仅AI谱" : "排除AI谱";
                conditions.Add(new FilterCondition(FilterConditionType.Automapper, autoValue));
            }

            // BeatSaver节点下的NPS
            if (!skipTypes?.Contains(FilterConditionType.NpsRange) == true)
            {
                AddRangeCondition(conditions, bs.NPS, FilterConditionType.NpsRange, "NPS");
            }

            // BeatSaver节点下的BPM
            if (!skipTypes?.Contains(FilterConditionType.BpmRange) == true)
            {
                AddRangeCondition(conditions, bs.BPM, FilterConditionType.BpmRange, "BPM");
            }

            // BeatSaver节点下的歌曲时长
            if (!skipTypes?.Contains(FilterConditionType.DurationRange) == true)
            {
                AddRangeCondition(conditions, bs.Duration, FilterConditionType.DurationRange, "时长");
            }

            // BeatSaver节点下的上传时间
            if (!skipTypes?.Contains(FilterConditionType.MinUploadedDate) == true)
            {
                AddUploadTimeCondition(conditions, bs.UploadTime, result);
            }

            // BeatSaver节点下的评分
            if (!skipTypes?.Contains(FilterConditionType.ScoreRange) == true)
            {
                AddRangeCondition(conditions, bs.Rating, FilterConditionType.ScoreRange, "评分");
            }

            // BeatSaver节点下的需求组件
            AddModCondition(conditions, bs.RequiredMods, true, result, skipTypes);

            // BeatSaver节点下的排除组件
            AddModCondition(conditions, bs.ExcludeMods, false, result, skipTypes);
        }

        private void ConvertFilterItems(FilterItemsSection items, List<FilterCondition> conditions, ConversionResult result)
        {
            // NPS (方块密度)
            AddRangeCondition(conditions, items.NPS, FilterConditionType.NpsRange, "NPS");

            // BPM
            AddRangeCondition(conditions, items.BPM, FilterConditionType.BpmRange, "BPM");

            // 歌曲时长
            AddRangeCondition(conditions, items.SongDuration, FilterConditionType.DurationRange, "时长");

            // 难度星级 (SS星级)
            AddRangeCondition(conditions, items.Stars, FilterConditionType.SsStarsRange, "SS星级");

            // 下载量
            AddRangeCondition(conditions, items.Downloads, FilterConditionType.DownloadsRange, "下载量");

            // 游戏次数
            AddRangeCondition(conditions, items.Plays, FilterConditionType.PlaysRange, "游戏次数");

            // 点赞数量
            AddRangeCondition(conditions, items.Upvotes, FilterConditionType.UpvotesRange, "点赞数");

            // 点赞比例
            AddRangeCondition(conditions, items.UpvoteRatio, FilterConditionType.UpvoteRatioRange, "点赞比例");

            // 点踩数量
            AddRangeCondition(conditions, items.Downvotes, FilterConditionType.DownvotesRange, "点踩数");

            // 点踩比例
            AddRangeCondition(conditions, items.DownvoteRatio, FilterConditionType.DownvoteRatioRange, "点踩比例");

            // 总评分
            AddRangeCondition(conditions, items.Score, FilterConditionType.ScoreRange, "评分");

            // Sage分数
            AddRangeCondition(conditions, items.SageScore, FilterConditionType.SageScoreRange, "Sage分数");

            // 飞行速度 (NJS)
            AddRangeCondition(conditions, items.NJS, FilterConditionType.NjsRange, "NJS");

            // 偏移值
            AddRangeCondition(conditions, items.Offset, FilterConditionType.OffsetRange, "偏移值");

            // 炸弹数量
            AddRangeCondition(conditions, items.Bombs, FilterConditionType.BombsRange, "炸弹数");

            // 灯光事件
            AddRangeCondition(conditions, items.Events, FilterConditionType.EventsRange, "事件数");

            // 上传时间
            AddUploadTimeCondition(conditions, items.UploadTime, result);

            // 自动生成 (AI谱面)
            // BSF使用字符串值: "仅AI谱" 或 "排除AI谱"，"全部"表示不过滤
            if (items.AutoGenerated.Enabled)
            {
                string autoValue = items.AutoGenerated.AutoGeneratedValue ? "仅AI谱" : "排除AI谱";
                conditions.Add(new FilterCondition(FilterConditionType.Automapper, autoValue));
            }

            // 排位曲
            if (items.Ranked.Enabled)
            {
                conditions.Add(new FilterCondition(FilterConditionType.Ranked, items.Ranked.RankedValue));
            }

            // 上传者名称
            AddContentCondition(conditions, items.UploaderName, FilterConditionType.UploaderName, "上传者");

            // 包含模式
            AddCharacteristicCondition(conditions, items.IncludeMode, result);

            // 包含难度
            AddDifficultyCondition(conditions, items.IncludeDifficulty, result);

            // 需求组件 (Mod支持为"是")
            AddModCondition(conditions, items.RequiredMods, true, result);

            // 排除组件 (Mod支持为"否")
            AddModCondition(conditions, items.ExcludeMods, false, result);

            // 上传者ID (映射到UploaderName，bsf支持ID和名称筛选)
            AddContentCondition(conditions, items.UploaderId, FilterConditionType.UploaderName, "上传者ID");

            // 标签(包含)
            AddTagsCondition(conditions, items.Tags?.Include, FilterConditionType.Tags, "包含标签");

            // 标签(排除)
            AddTagsCondition(conditions, items.Tags?.Exclude, FilterConditionType.ExcludeTags, "排除标签");

            // 墙壁数量
            AddRangeCondition(conditions, items.Walls, FilterConditionType.ObstaclesRange, "墙壁数");

            // 谱面时长
            AddRangeCondition(conditions, items.MapDuration, FilterConditionType.SecondsRange, "谱面时长");

            // 节拍数量
            AddRangeCondition(conditions, items.Notes, FilterConditionType.NotesRange, "节拍数");

            // 方块数量
            AddRangeCondition(conditions, items.NoteCount, FilterConditionType.NotesRange, "方块数");

            // 校验错误
            AddRangeCondition(conditions, items.VerifyErrors, FilterConditionType.ParityErrorsRange, "校验错误");

            // 校验警告
            AddRangeCondition(conditions, items.VerifyWarnings, FilterConditionType.ParityWarnsRange, "校验警告");

            // 校验重置
            AddRangeCondition(conditions, items.VerifyResets, FilterConditionType.ParityResetsRange, "校验重置");

            // 最高分数
            AddRangeCondition(conditions, items.MaxScore, FilterConditionType.MaxScoreRange, "最高分数");

            // 忽略的项目 (记录到警告)
            if (items.FilterChinese.Enabled)
                result.SkippedItems.Add("筛选中文 (BSF不支持)");
        }

        private void ConvertScoreSaber(ScoreSaberSection ss, List<FilterCondition> conditions, ConversionResult result)
        {
            if (ss.Difficulty != null)
            {
                // ScoreSaber难度实际上对应SS星级
                AddRangeCondition(conditions, ss.Difficulty, FilterConditionType.SsStarsRange, "SS星级");
            }
        }

        private void ConvertSearchFilter(SearchFilterSection sf, List<FilterCondition> conditions, ConversionResult result)
        {
            // 搜索开关未启用或搜索内容为空则跳过
            if (!sf.SearchEnabled || string.IsNullOrWhiteSpace(sf.SearchContent))
                return;

            // 支持 \r\n 和 \n 分隔符，按行拆分成多个搜索词
            var searchLines = sf.SearchContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // 构建 SearchFieldType 标志位
            // BRSET搜索范围映射到BSF的SearchFieldType:
            // 搜索标题 -> MapName (name:)
            // 搜索歌名 -> SongName (songname:)
            // 搜索作者 -> Artist (artist:)
            // 搜索谱师 -> Mapper (mapper:)
            // 搜索介绍 -> Description (description:)
            SearchFieldType fieldTypes = SearchFieldType.None;

            if (sf.SearchTitle)
                fieldTypes |= SearchFieldType.MapName;
            if (sf.SearchSongName)
                fieldTypes |= SearchFieldType.SongName;
            if (sf.SearchAuthor)
                fieldTypes |= SearchFieldType.Artist;
            if (sf.SearchMapper)
                fieldTypes |= SearchFieldType.Mapper;
            if (sf.SearchDescription)
                fieldTypes |= SearchFieldType.Description;

            // 如果没有任何范围选项被选中，使用All（全部搜索）
            if (fieldTypes == SearchFieldType.None)
                fieldTypes = SearchFieldType.All;

            // 为每行搜索词创建一个 Query 条件，使用 SearchQueryValue
            // 多行搜索词之间使用 OR 逻辑（任一匹配即可）
            for (int i = 0; i < searchLines.Length; i++)
            {
                var line = searchLines[i].Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    var queryValue = new SearchQueryValue(line, fieldTypes);
                    var condition = new FilterCondition(FilterConditionType.Query, queryValue);
                    // 从第二个搜索词开始，使用 OR 逻辑
                    if (i > 0)
                    {
                        condition.Operator = LogicOperator.Or;
                    }
                    conditions.Add(condition);
                }
            }
        }

        /// <summary>
        /// 将多行搜索词添加为指定类型的条件，条件之间使用 OR 逻辑（已弃用）
        /// </summary>
        [Obsolete("Use ConvertSearchFilter with SearchQueryValue instead")]
        private void AddSearchLinesAsConditions(List<FilterCondition> conditions, string[] searchLines, FilterConditionType conditionType)
        {
            for (int i = 0; i < searchLines.Length; i++)
            {
                var line = searchLines[i].Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    var condition = new FilterCondition(conditionType, line);
                    // 从第二个条件开始，使用 OR 逻辑
                    if (i > 0)
                    {
                        condition.Operator = LogicOperator.Or;
                    }
                    conditions.Add(condition);
                }
            }
        }

        private void ConvertDownloadLimit(DownloadLimitSection limit, List<FilterCondition> conditions, ConversionResult result)
        {
            if (limit.LimitCount != null && limit.LimitCount.Enabled)
            {
                if (int.TryParse(limit.LimitCount.Content, out int count) && count > 0)
                {
                    var resultLimit = new ResultLimitValue(count, ResultSortOption.Newest);
                    conditions.Add(new FilterCondition(FilterConditionType.ResultLimit, resultLimit));
                }
            }
        }

        private void AddMinMaxCondition(List<FilterCondition> conditions, MinMaxSection section,
            FilterConditionType minType, FilterConditionType maxType, string displayName)
        {
            if (section == null || !section.Enabled) return;

            if (!string.IsNullOrEmpty(section.Min))
            {
                if (double.TryParse(section.Min, out double minVal))
                {
                    conditions.Add(new FilterCondition(minType, minVal));
                }
            }

            if (!string.IsNullOrEmpty(section.Max))
            {
                if (double.TryParse(section.Max, out double maxVal))
                {
                    conditions.Add(new FilterCondition(maxType, maxVal));
                }
            }
        }

        /// <summary>
        /// 添加范围条件(新版UI合并的min-max条件)
        /// </summary>
        private void AddRangeCondition(List<FilterCondition> conditions, MinMaxSection section,
            FilterConditionType rangeType, string displayName)
        {
            if (section == null || !section.Enabled) return;

            double? minVal = null;
            double? maxVal = null;

            if (!string.IsNullOrEmpty(section.Min) && double.TryParse(section.Min, out double min))
            {
                minVal = min;
            }

            if (!string.IsNullOrEmpty(section.Max) && double.TryParse(section.Max, out double max))
            {
                maxVal = max;
            }

            if (minVal.HasValue || maxVal.HasValue)
            {
                conditions.Add(new FilterCondition(rangeType, new RangeValue(minVal, maxVal)));
            }
        }

        private void AddContentCondition(List<FilterCondition> conditions, EnabledContentSection section,
            FilterConditionType type, string displayName)
        {
            if (section == null || !section.Enabled || string.IsNullOrEmpty(section.Content)) return;
            conditions.Add(new FilterCondition(type, section.Content));
        }

        private void AddTagsCondition(List<FilterCondition> conditions, TagFilterSection section,
            FilterConditionType type, string displayName)
        {
            if (section == null || !section.Enabled || string.IsNullOrEmpty(section.Content)) return;

            // 支持逗号分隔的多个标签，每个标签转换为独立条件
            var tags = section.Content.Split(new[] { ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);

            // BRSET的与/或配置：如果"或"为true，使用Or逻辑；否则使用And逻辑
            var useOr = section.Or;

            for (int i = 0; i < tags.Length; i++)
            {
                var trimmed = tags[i].Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    var condition = new FilterCondition(type, trimmed);
                    // 从第二个标签开始，根据BRSET的与/或配置设置Operator
                    // 第一个标签的Operator不影响结果（因为没有前一个条件）
                    if (i > 0 && useOr)
                    {
                        condition.Operator = LogicOperator.Or;
                    }
                    conditions.Add(condition);
                }
            }
        }

        private void AddUploadTimeCondition(List<FilterCondition> conditions, MinMaxSection section, ConversionResult result)
        {
            if (section == null || !section.Enabled) return;

            // brset的上传时间格式可能是：日期字符串、Unix时间戳、或天数偏移
            if (!string.IsNullOrEmpty(section.Min))
            {
                if (DateTime.TryParse(section.Min, out DateTime minDate))
                {
                    // 日期字符串格式
                    conditions.Add(new FilterCondition(FilterConditionType.MinUploadedDate, minDate));
                }
                else if (double.TryParse(section.Min, out double minValue))
                {
                    // 判断是Unix时间戳还是天数偏移
                    // Unix时间戳通常 > 1000000000 (约2001年)
                    // 天数偏移通常 < 100000 (约274年)
                    if (minValue > 1000000000)
                    {
                        // Unix时间戳（秒）
                        var date = DateTimeOffset.FromUnixTimeSeconds((long)minValue).DateTime;
                        conditions.Add(new FilterCondition(FilterConditionType.MinUploadedDate, date));
                    }
                    else
                    {
                        // 天数偏移
                        var date = DateTime.Now.AddDays(-minValue);
                        conditions.Add(new FilterCondition(FilterConditionType.MinUploadedDate, date));
                    }
                }
            }

            if (!string.IsNullOrEmpty(section.Max))
            {
                if (DateTime.TryParse(section.Max, out DateTime maxDate))
                {
                    // 日期字符串格式
                    conditions.Add(new FilterCondition(FilterConditionType.MaxUploadedDate, maxDate));
                }
                else if (double.TryParse(section.Max, out double maxValue))
                {
                    // 判断是Unix时间戳还是天数偏移
                    if (maxValue > 1000000000)
                    {
                        // Unix时间戳（秒）
                        var date = DateTimeOffset.FromUnixTimeSeconds((long)maxValue).DateTime;
                        conditions.Add(new FilterCondition(FilterConditionType.MaxUploadedDate, date));
                    }
                    else
                    {
                        // 天数偏移
                        var date = DateTime.Now.AddDays(-maxValue);
                        conditions.Add(new FilterCondition(FilterConditionType.MaxUploadedDate, date));
                    }
                }
            }
        }

        private void AddCharacteristicCondition(List<FilterCondition> conditions, EnabledContentSection section, ConversionResult result)
        {
            if (section == null || !section.Enabled || string.IsNullOrEmpty(section.Content)) return;

            // 映射中文模式名到英文
            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "标准", "Standard" },
                { "单手", "OneSaber" },
                { "无箭头", "NoArrows" },
                { "360度", "360Degree" },
                { "90度", "90Degree" },
                { "灯光秀", "Lightshow" },
                { "无法无天", "Lawless" },
                { "旧版", "Legacy" }
            };

            // 支持逗号分隔的多个值，每个值转换为独立条件
            var values = section.Content.Split(new[] { ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var value in values)
            {
                var trimmed = value.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                if (mapping.TryGetValue(trimmed, out var mapped))
                {
                    conditions.Add(new FilterCondition(FilterConditionType.Characteristic, mapped));
                }
                else
                {
                    // 可能已经是英文
                    conditions.Add(new FilterCondition(FilterConditionType.Characteristic, trimmed));
                }
            }
        }

        private void AddDifficultyCondition(List<FilterCondition> conditions, EnabledContentSection section, ConversionResult result)
        {
            if (section == null || !section.Enabled || string.IsNullOrEmpty(section.Content)) return;

            // 映射中文难度名到英文
            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "简单", "Easy" },
                { "普通", "Normal" },
                { "困难", "Hard" },
                { "专家", "Expert" },
                { "专家+", "ExpertPlus" }
            };

            // 支持逗号分隔的多个值，每个值转换为独立条件
            var values = section.Content.Split(new[] { ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var value in values)
            {
                var trimmed = value.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                if (mapping.TryGetValue(trimmed, out var mapped))
                {
                    conditions.Add(new FilterCondition(FilterConditionType.Difficulty, mapped));
                }
                else
                {
                    // 可能已经是英文
                    conditions.Add(new FilterCondition(FilterConditionType.Difficulty, trimmed));
                }
            }
        }

        /// <summary>
        /// 添加Mod条件
        /// </summary>
        /// <param name="conditions">条件列表</param>
        /// <param name="section">配置节</param>
        /// <param name="requireMod">true=需求组件(Mod支持为是), false=排除组件(Mod支持为否)</param>
        /// <param name="result">转换结果</param>
        /// <param name="skipTypes">需要跳过的条件类型集合</param>
        private void AddModCondition(List<FilterCondition> conditions, EnabledContentSection section, bool requireMod, ConversionResult result, HashSet<FilterConditionType>? skipTypes = null)
        {
            if (section == null || !section.Enabled || string.IsNullOrEmpty(section.Content)) return;

            var mods = section.Content.Split(new[] { ',', '，', ' ', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var mod in mods)
            {
                var trimmedMod = mod.Trim();
                if (string.IsNullOrEmpty(trimmedMod)) continue;

                // 映射常见Mod名（包含常用缩写）
                var modLower = trimmedMod.ToLower();
                FilterConditionType? modType = modLower switch
                {
                    "chroma" => FilterConditionType.Chroma,
                    "noodle" or "noodleextensions" or "ne" => FilterConditionType.Ne,
                    "me" or "mappingextensions" => FilterConditionType.Me,
                    "cinema" => FilterConditionType.Cinema,
                    "vivify" => FilterConditionType.Vivify,
                    _ => null
                };

                if (modType.HasValue)
                {
                    // 检查是否应该跳过此Mod类型
                    if (skipTypes?.Contains(modType.Value) == true) continue;

                    // requireMod=true表示需求组件，Value=true
                    // requireMod=false表示排除组件，Value=false
                    conditions.Add(new FilterCondition(modType.Value, requireMod));
                }
                else
                {
                    // 自定义Mod
                    // requireMod=true表示需求组件，使用CustomMod类型
                    // requireMod=false表示排除组件，使用ExcludeCustomMod类型
                    var customModType = requireMod ? FilterConditionType.CustomMod : FilterConditionType.ExcludeCustomMod;

                    // 检查是否应该跳过
                    if (skipTypes?.Contains(customModType) == true) continue;

                    conditions.Add(new FilterCondition(customModType, trimmedMod));
                }
            }
        }

        /// <summary>
        /// 将BSF预设保存到文件
        /// </summary>
        public void SaveBsfFile(FilterPreset preset, string filePath)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK"
            };

            var json = JsonConvert.SerializeObject(preset, settings);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// 将BSF预设转换为JSON字符串
        /// </summary>
        public string ToBsfJson(FilterPreset preset)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK"
            };

            return JsonConvert.SerializeObject(preset, settings);
        }
    }
}