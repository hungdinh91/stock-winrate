namespace StockAnalyzer.Shared.Utils
{
    public class CsvReader
    {
        public static List<string[]> ReadCsv(string filePath)
        {
            var result = new List<string[]>();

            // Ensure the file exists before proceeding
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                return result;
            }

            // Read all lines from the CSV file
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // Split each line into an array based on the delimiter (comma in this case)
                var values = line.Split(',');
                result.Add(values);
            }

            return result;
        }
    }
}
