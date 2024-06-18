using Newtonsoft.Json;
using UnityManifestCompare.Data;

namespace UnityManifestCompare
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var firstManifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(args[0]));
            var secondManifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(args[1]));

            ShowExtraScopedRegistries(firstManifest, secondManifest);
            Console.WriteLine();
            Console.WriteLine("===============================================================");
            Console.WriteLine();
            ShowExtraDependencies(firstManifest, secondManifest);
            Console.WriteLine();
            Console.WriteLine("===============================================================");
            Console.WriteLine();
            ShowVersionDifferences(firstManifest, secondManifest);
        }
        private static void ShowExtraScopedRegistries(Manifest firstManifest, Manifest secondManifest)
        {
            var uniqueToFirstManifest = firstManifest.ScopedRegistries
                .Where(entry => secondManifest.ScopedRegistries.All(sr => sr.Name != entry.Name))
                .ToList();

            Console.WriteLine("Scoped registries missing from the second manifest:");
            Console.WriteLine("---------------------------------------------------");
            foreach (var scopedRegistry in uniqueToFirstManifest)
            {
                Console.WriteLine($"{JsonConvert.SerializeObject(scopedRegistry, Formatting.Indented)},");
            }
        }

        private static void ShowExtraDependencies(Manifest firstManifest, Manifest secondManifest)
        {
            var uniqueToFirstManifest = firstManifest.Dependencies
                .Where(entry => !secondManifest.Dependencies.ContainsKey(entry.Key))
                .ToDictionary(entry => entry.Key, entry => entry.Value);

            Console.WriteLine("Dependencies missing from the second manifest:");
            Console.WriteLine("----------------------------------------------");

            foreach (var dependency in uniqueToFirstManifest)
            {
                Console.WriteLine($"\"{dependency.Key}\": \"{dependency.Value}\",");
            }
        }
        
        private static void ShowVersionDifferences(Manifest firstManifest, Manifest secondManifest)
        {
            var differentVersions = firstManifest.Dependencies
                .Where(entry => secondManifest.Dependencies.ContainsKey(entry.Key) && secondManifest.Dependencies[entry.Key] != entry.Value)
                .ToDictionary(entry => entry.Key, entry => (entry.Value, secondManifest.Dependencies[entry.Key]));

            Console.WriteLine("Dependencies with different versions:");
            Console.WriteLine("-------------------------------------");

            foreach (var dependency in differentVersions)
            {
                Console.WriteLine($"{dependency.Key}: {dependency.Value.Item1} vs {dependency.Value.Item2}");
            }
        }
    }
}
