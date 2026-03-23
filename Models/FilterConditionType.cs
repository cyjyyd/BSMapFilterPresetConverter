using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FilterPresetConverter.Models
{
    /// <summary>
    /// 过滤条件类型枚举
    /// </summary>
    public enum FilterConditionType
    {
        None = int.MinValue,
        Custom = -2,
        Query = 0,
        Order,
        MinBpm,
        MaxBpm,
        MinNps,
        MaxNps,
        MinDuration,
        MaxDuration,
        MinSsStars,
        MaxSsStars,
        MinBlStars,
        MaxBlStars,
        Chroma,
        Noodle,
        Me,
        Cinema,
        Vivify,
        Automapper,
        Leaderboard,
        Curated,
        Verified,
        Ranked = 100,
        BlRanked,
        Qualified,
        BlQualified,
        MinPlays,
        MaxPlays,
        MinDownloads,
        MaxDownloads,
        MinUpvotes,
        MaxUpvotes,
        MinDownvotes,
        MaxDownvotes,
        MinScore,
        MaxScore,
        Tags,
        UploaderName,
        MinUpvoteRatio,
        MaxUpvoteRatio,
        MinDownvoteRatio,
        MaxDownvoteRatio,
        MinSageScore,       // 120 - Sage分数
        MaxSageScore,       // 121

        // Diff-specific filters (from 130)
        MinNjs = 130,       // Note Jump Speed
        MaxNjs,             // 131
        MinBombs,           // 132
        MaxBombs,           // 133
        MinOffset,          // 134
        MaxOffset,          // 135
        MinEvents,          // 136
        MaxEvents,          // 137
        Characteristic,     // 138 - Standard, Lawless, Lightshow, etc.
        Difficulty,         // 139 - Easy, Normal, Hard, Expert, ExpertPlus

        // Additional mods
        Ne = 150,           // Noodle Extensions
        CustomMod,          // 151 - User-defined mod name (包含)
        ExcludeCustomMod,   // 152 - User-defined mod name (排除)

        // Upload time filters (from 140)
        MinUploadedDate = 140,  // 最小上传时间（在此时间之后上传）
        MaxUploadedDate,        // 141 - 最大上传时间（在此时间之前上传）

        // Result limit (from 145)
        ResultLimit = 145,      // 数量限制（带排序选项）

        // Map objects filters (from 160)
        MinObstacles = 160,     // 最小墙壁数量
        MaxObstacles,           // 161 - 最大墙壁数量
        MinBombsMap,            // 162 - 最小地图炸弹数量
        MaxBombsMap,            // 163 - 最大地图炸弹数量
        MinNotes,               // 164 - 最小方块数量
        MaxNotes,               // 165 - 最大方块数量
        MinSeconds,             // 166 - 最小谱面时长(秒)
        MaxSeconds,             // 167 - 最大谱面时长(秒)
        MinLength,              // 168 - 最小节拍数量
        MaxLength,              // 169 - 最大节拍数量
        MinParityErrors = 170,  // 最小校验错误数
        MaxParityErrors,        // 171 - 最大校验错误数
        MinParityWarns,         // 172 - 最小校验警告数
        MaxParityWarns,         // 173 - 最大校验警告数
        MinParityResets,        // 174 - 最小校验重置数
        MaxParityResets,        // 175 - 最大校验重置数
        ExcludeTags = 180,      // 排除标签
        MinMaxScore = 181,      // 最小最高分数
        MaxMaxScore,            // 182 - 最大最高分数

        // Range-type filters (from 200) - simplified UI with min-max in one condition
        BpmRange = 200,         // BPM范围
        NpsRange,               // 201 - NPS范围
        DurationRange,          // 202 - 时长范围
        SsStarsRange,           // 203 - SS星级范围
        BlStarsRange,           // 204 - BL星级范围
        ScoreRange,             // 205 - 评分范围
        PlaysRange,             // 206 - 游玩次数范围
        DownloadsRange,         // 207 - 下载次数范围
        UpvotesRange,           // 208 - 点赞数范围
        DownvotesRange,         // 209 - 踩数范围
        UpvoteRatioRange,       // 210 - 点赞比例范围
        DownvoteRatioRange,     // 211 - 点踩比例范围
        SageScoreRange,         // 212 - Sage分数范围
        NjsRange,               // 213 - NJS范围
        BombsRange,             // 214 - 炸弹数范围(难度)
        OffsetRange,            // 215 - 偏移范围
        EventsRange,            // 216 - 事件数范围
        ObstaclesRange,         // 217 - 墙壁数范围
        BombsMapRange,          // 218 - 地图炸弹数范围
        NotesRange,             // 219 - 方块数范围
        SecondsRange,           // 220 - 谱面时长范围
        LengthRange,            // 221 - 节拍数范围
        ParityErrorsRange,      // 222 - 校验错误范围
        ParityWarnsRange,       // 223 - 校验警告范围
        ParityResetsRange,      // 224 - 校验重置范围
        MaxScoreRange           // 225 - 最高分范围
    }

    /// <summary>
    /// 逻辑运算符
    /// </summary>
    public enum LogicOperator
    {
        And,
        Or
    }

    /// <summary>
    /// 值类型
    /// </summary>
    public enum FilterValueType
    {
        Text,
        Number,
        Boolean,
        Selection,
        Date,
        NumberWithSort,
        Range,          // 范围输入(min-max)
        SearchQuery,    // 搜索关键词+类型
        ExcludeMod      // 排除mod+严格模式选项
    }

    /// <summary>
    /// 搜索字段类型 (标志枚举，支持多选)
    /// </summary>
    [Flags]
    public enum SearchFieldType
    {
        None = 0,
        SongName = 1,       // 歌曲名 (songname:)
        Artist = 2,         // 艺术家 (artist:)
        Mapper = 4,         // 谱师 (mapper:)
        MapName = 8,        // 谱面标题 (name:)
        Description = 16,   // 简介 (description:)
        Uploader = 32,      // 上传者 (uploader:)

        // 预设组合
        All = SongName | Artist | Mapper | MapName | Description | Uploader
    }

    /// <summary>
    /// 搜索查询值（支持多字段搜索）
    /// </summary>
    public class SearchQueryValue
    {
        public string Query { get; set; } = "";
        public SearchFieldType FieldTypes { get; set; } = SearchFieldType.All;

        public SearchQueryValue() { }

        public SearchQueryValue(string query, SearchFieldType fieldTypes = SearchFieldType.All)
        {
            Query = query ?? "";
            FieldTypes = fieldTypes;
        }

        public bool HasValue => !string.IsNullOrWhiteSpace(Query);

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Query))
                return "";

            var fieldNames = new List<string>();
            if (FieldTypes.HasFlag(SearchFieldType.SongName)) fieldNames.Add("歌名");
            if (FieldTypes.HasFlag(SearchFieldType.Artist)) fieldNames.Add("艺术家");
            if (FieldTypes.HasFlag(SearchFieldType.Mapper)) fieldNames.Add("谱师");
            if (FieldTypes.HasFlag(SearchFieldType.MapName)) fieldNames.Add("标题");
            if (FieldTypes.HasFlag(SearchFieldType.Description)) fieldNames.Add("简介");
            if (FieldTypes.HasFlag(SearchFieldType.Uploader)) fieldNames.Add("上传者");

            var fieldStr = fieldNames.Count == 0 || fieldNames.Count == 6 ? "" : $"[{string.Join("/", fieldNames)}]";
            return $"{fieldStr}{Query}";
        }
    }

    /// <summary>
    /// 范围值(min-max)
    /// </summary>
    public class RangeValue
    {
        // 使用 double.NaN 作为内部"未设置"的标志
        // 这允许 0 成为有效的筛选值
        private double _min = double.NaN;
        private double _max = double.NaN;

        public double? Min
        {
            get => double.IsNaN(_min) ? null : _min;
            set => _min = value ?? double.NaN;
        }

        public double? Max
        {
            get => double.IsNaN(_max) ? null : _max;
            set => _max = value ?? double.NaN;
        }

        // 用于序列化的内部属性（直接存储NaN）
        public double MinRaw
        {
            get => _min;
            set => _min = value;
        }

        public double MaxRaw
        {
            get => _max;
            set => _max = value;
        }

        public RangeValue() { }

        public RangeValue(double? min, double? max)
        {
            Min = min;
            Max = max;
        }

        public bool HasValue => !double.IsNaN(_min) || !double.IsNaN(_max);

        public bool HasMin => !double.IsNaN(_min);
        public bool HasMax => !double.IsNaN(_max);

        public override string ToString()
        {
            if (HasMin && HasMax)
                return $"{_min}-{_max}";
            else if (HasMin)
                return $"≥{_min}";
            else if (HasMax)
                return $"≤{_max}";
            return "";
        }
    }

    /// <summary>
    /// 排除Mod值（包含Mod名称和严格模式选项）
    /// </summary>
    public class ExcludeModValue
    {
        public string ModName { get; set; } = "";
        public bool Strict { get; set; } = false;

        public ExcludeModValue() { }

        public ExcludeModValue(string modName, bool strict = false)
        {
            ModName = modName ?? "";
            Strict = strict;
        }

        public bool HasValue => !string.IsNullOrWhiteSpace(ModName);

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(ModName))
                return "";
            return Strict ? $"{ModName} (严格)" : ModName;
        }
    }

    /// <summary>
    /// 排序选项
    /// </summary>
    public enum ResultSortOption
    {
        Newest,
        Oldest,
        Random
    }

    /// <summary>
    /// 数量限制值
    /// </summary>
    public class ResultLimitValue
    {
        public int Count { get; set; }
        public ResultSortOption SortOption { get; set; } = ResultSortOption.Newest;

        public ResultLimitValue() { }

        public ResultLimitValue(int count, ResultSortOption sortOption = ResultSortOption.Newest)
        {
            Count = count;
            SortOption = sortOption;
        }
    }

    /// <summary>
    /// 过滤条件
    /// </summary>
    public class FilterCondition
    {
        public FilterConditionType Type { get; set; }
        public string CustomName { get; set; } = "";
        public object? Value { get; set; }
        public LogicOperator Operator { get; set; } = LogicOperator.And;
        public bool IsEnabled { get; set; } = true;

        [JsonIgnore]
        public string DisplayName => Type == FilterConditionType.Custom ?
            (string.IsNullOrWhiteSpace(CustomName) ? "自定义" : CustomName) :
            FilterConditionMetadata.GetDisplayName(Type);

        [JsonIgnore]
        public FilterValueType ValueType => FilterConditionMetadata.GetValueType(Type);

        [JsonIgnore]
        public List<string> Options => FilterConditionMetadata.GetOptions(Type);

        public FilterCondition() { }

        public FilterCondition(FilterConditionType type, object? value)
        {
            Type = type;
            Value = value;
            IsEnabled = true;
        }
    }

    /// <summary>
    /// 条件类型元数据
    /// </summary>
    public static class FilterConditionMetadata
    {
        private static readonly Dictionary<FilterConditionType, (string DisplayName, FilterValueType ValueType, List<string> Options)> _metadata =
            new Dictionary<FilterConditionType, (string, FilterValueType, List<string>)>
            {
                { FilterConditionType.Query, ("搜索关键词", FilterValueType.SearchQuery, null) },
                { FilterConditionType.Order, ("排序方式", FilterValueType.Selection, new List<string> { "Latest", "Relevance", "Rating", "Curated", "Random", "Duration" }) },
                { FilterConditionType.Chroma, ("Chroma", FilterValueType.Boolean, null) },
                { FilterConditionType.Noodle, ("Noodle", FilterValueType.Boolean, null) },
                { FilterConditionType.Me, ("Mapping Extensions", FilterValueType.Boolean, null) },
                { FilterConditionType.Cinema, ("Cinema", FilterValueType.Boolean, null) },
                { FilterConditionType.Vivify, ("Vivify", FilterValueType.Boolean, null) },
                { FilterConditionType.Automapper, ("AI谱面", FilterValueType.Selection, new List<string> { "全部", "仅AI谱", "排除AI谱" }) },
                { FilterConditionType.Curated, ("精选", FilterValueType.Boolean, null) },
                { FilterConditionType.Verified, ("认证谱师", FilterValueType.Boolean, null) },
                { FilterConditionType.Ranked, ("SS排位", FilterValueType.Boolean, null) },
                { FilterConditionType.BlRanked, ("BL排位", FilterValueType.Boolean, null) },
                { FilterConditionType.Qualified, ("SS待评级", FilterValueType.Boolean, null) },
                { FilterConditionType.BlQualified, ("BL待评级", FilterValueType.Boolean, null) },
                { FilterConditionType.Tags, ("包含标签", FilterValueType.Text, null) },
                { FilterConditionType.ExcludeTags, ("排除标签", FilterValueType.Text, null) },
                { FilterConditionType.UploaderName, ("上传者", FilterValueType.Text, null) },
                { FilterConditionType.Characteristic, ("特征", FilterValueType.Selection, new List<string> { "Standard", "OneSaber", "NoArrows", "360Degree", "90Degree", "Lightshow", "Lawless", "Legacy" }) },
                { FilterConditionType.Difficulty, ("难度", FilterValueType.Selection, new List<string> { "Easy", "Normal", "Hard", "Expert", "ExpertPlus" }) },
                { FilterConditionType.Ne, ("Noodle Extensions", FilterValueType.Boolean, null) },
                { FilterConditionType.CustomMod, ("包含Mod", FilterValueType.Text, null) },
                { FilterConditionType.ExcludeCustomMod, ("排除Mod", FilterValueType.ExcludeMod, null) },
                { FilterConditionType.MinUploadedDate, ("上传时间起始", FilterValueType.Date, null) },
                { FilterConditionType.MaxUploadedDate, ("上传时间截止", FilterValueType.Date, null) },
                { FilterConditionType.ResultLimit, ("数量限制", FilterValueType.NumberWithSort, new List<string> { "最新上传", "最早上传", "随机" }) },
                // Range-type conditions
                { FilterConditionType.BpmRange, ("BPM", FilterValueType.Range, null) },
                { FilterConditionType.NpsRange, ("NPS", FilterValueType.Range, null) },
                { FilterConditionType.DurationRange, ("时长(秒)", FilterValueType.Range, null) },
                { FilterConditionType.SsStarsRange, ("SS星级", FilterValueType.Range, null) },
                { FilterConditionType.BlStarsRange, ("BL星级", FilterValueType.Range, null) },
                { FilterConditionType.ScoreRange, ("评分", FilterValueType.Range, null) },
                { FilterConditionType.PlaysRange, ("游玩次数", FilterValueType.Range, null) },
                { FilterConditionType.DownloadsRange, ("下载次数", FilterValueType.Range, null) },
                { FilterConditionType.UpvotesRange, ("点赞数", FilterValueType.Range, null) },
                { FilterConditionType.DownvotesRange, ("踩数", FilterValueType.Range, null) },
                { FilterConditionType.UpvoteRatioRange, ("点赞比例(%)", FilterValueType.Range, null) },
                { FilterConditionType.DownvoteRatioRange, ("点踩比例(%)", FilterValueType.Range, null) },
                { FilterConditionType.SageScoreRange, ("Sage分数", FilterValueType.Range, null) },
                { FilterConditionType.NjsRange, ("NJS", FilterValueType.Range, null) },
                { FilterConditionType.BombsRange, ("炸弹数(难度)", FilterValueType.Range, null) },
                { FilterConditionType.OffsetRange, ("偏移", FilterValueType.Range, null) },
                { FilterConditionType.EventsRange, ("事件数", FilterValueType.Range, null) },
                { FilterConditionType.ObstaclesRange, ("墙壁数", FilterValueType.Range, null) },
                { FilterConditionType.NotesRange, ("方块数", FilterValueType.Range, null) },
                { FilterConditionType.SecondsRange, ("谱面时长(秒)", FilterValueType.Range, null) },
                { FilterConditionType.ParityErrorsRange, ("校验错误", FilterValueType.Range, null) },
                { FilterConditionType.ParityWarnsRange, ("校验警告", FilterValueType.Range, null) },
                { FilterConditionType.ParityResetsRange, ("校验重置", FilterValueType.Range, null) },
                { FilterConditionType.MaxScoreRange, ("最高分", FilterValueType.Range, null) }
            };

        public static string GetDisplayName(FilterConditionType type)
        {
            return _metadata.TryGetValue(type, out var meta) ? meta.DisplayName : type.ToString();
        }

        public static FilterValueType GetValueType(FilterConditionType type)
        {
            return _metadata.TryGetValue(type, out var meta) ? meta.ValueType : FilterValueType.Text;
        }

        public static List<string> GetOptions(FilterConditionType type)
        {
            return _metadata.TryGetValue(type, out var meta) ? meta.Options : null;
        }
    }

    /// <summary>
    /// 过滤条件组
    /// </summary>
    public class FilterGroup
    {
        public string? Name { get; set; }
        public List<FilterCondition> Conditions { get; set; } = new List<FilterCondition>();
        public LogicOperator GroupOperator { get; set; } = LogicOperator.And;
        public bool IsEnabled { get; set; } = true;
        public bool UseLocalCache { get; set; } = true;

        public FilterGroup() { }

        public FilterGroup(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// 过滤预设 (BSF格式)
    /// </summary>
    public class FilterPreset
    {
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public List<FilterGroup> Groups { get; set; } = new List<FilterGroup>();
        public string? Description { get; set; }
        public ResultLimitValue? TopLevelResultLimit { get; set; }

        public FilterPreset() { }

        public FilterPreset(string name)
        {
            Name = name;
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }
    }
}