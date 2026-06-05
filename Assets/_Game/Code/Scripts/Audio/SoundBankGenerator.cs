using System.IO;
using System.Text;
using UnityEditor;

public static class SoundBankGenerator
{
    public static void Generate(SoundBank bank)
    {
        var sb = new StringBuilder();

        string className = bank.name.Replace(" ", "");

        sb.AppendLine("using FMODUnity;");
        sb.AppendLine("");
        sb.AppendLine($"public static class {className}");
        sb.AppendLine("{");

        var references = bank.References;

        foreach (var sound in references)
        {
            string safeName = sound.soundName.Replace(" ", "_");
            sb.AppendLine($"    public static EventReference {safeName} =>");
            //sb.AppendLine($"        EventReference.Find(\"{sound.eventReference.Path}\");");
        }

        sb.AppendLine("}");

        string path = $"Assets/Generated/{className}.cs";
        File.WriteAllText(path, sb.ToString());

        AssetDatabase.Refresh();
    }
}