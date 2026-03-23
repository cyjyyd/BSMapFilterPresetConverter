// Simple test for ExcludeModValue
#r "bin/Debug/net9.0-windows/FilterPresetConverter.dll"

using FilterPresetConverter.Models;
using FilterPresetConverter.Services;
using Newtonsoft.Json;

var converter = new BrsetToBsfConverter();

// Test with custom mod exclusion
var testBrset = new BrsetPreset
{
    FilterItems = new FilterItemsSection
    {
        ExcludeMods = new EnabledContentSection { Enabled = true, Content = "SomeMod,AnotherMod" }
    }
};

var result = converter.Convert(testBrset, "Test Exclude Mod");
Console.WriteLine("Test: Custom Mod Exclusion");
Console.WriteLine($"Result: {result.Success}");
Console.WriteLine($"Conditions: {result.Preset?.Groups.Sum(g => g.Conditions.Count) ?? 0}");

if (result.Preset != null)
{
    var json = converter.ToBsfJson(result.Preset);
    Console.WriteLine("\nJSON Output:");
    Console.WriteLine(json);
}
