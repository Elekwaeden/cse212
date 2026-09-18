using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        // PLAN:
        // 1. Put every word into a HashSet so we can check for the existence of its
        //    reverse in O(1) time instead of scanning the whole array again (which would be O(n^2)).
        // 2. Loop through each word once. Skip words where both letters are the same
        //    (e.g. "aa") since those can never have a distinct symmetric match.
        // 3. For each word, build its reversed version (swap the two letters).
        // 4. If the reversed version exists in the set, and we haven't already recorded
        //    this pair (using a second "seen" set to avoid adding both "am & ma" and "ma & am"),
        //    add the pair to the result and mark both words as seen.
        // 5. Return the collected pairs as an array.

        var wordSet = new HashSet<string>(words);
        var seen = new HashSet<string>();
        var result = new List<string>();

        foreach (var word in words)
        {
            if (word[0] == word[1])
            {
                continue; // Same-letter words like "aa" can't form a pair.
            }

            var reversed = new string(new[] { word[1], word[0] });

            if (wordSet.Contains(reversed) && !seen.Contains(word) && !seen.Contains(reversed))
            {
                result.Add($"{word} & {reversed}");
                seen.Add(word);
                seen.Add(reversed);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");

            // PLAN:
            // 1. The degree is in column index 3 (4th column, zero-indexed).
            // 2. Check if this degree is already a key in the dictionary.
            // 3. If it is, increment its count by 1.
            // 4. If it is not, add it to the dictionary with a starting count of 1.
            var degree = fields[3];
            if (degrees.ContainsKey(degree))
            {
                degrees[degree] += 1;
            }
            else
            {
                degrees[degree] = 1;
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        // PLAN:
        // 1. Normalize both words: remove spaces and convert to a single case (lowercase)
        //    so that case and spacing differences don't affect the comparison.
        // 2. If the cleaned lengths don't match, they can't be anagrams - return false immediately.
        // 3. Build a dictionary counting how many times each letter appears in word1.
        // 4. Walk through word2's letters, decrementing the count for each letter found.
        //    If a letter in word2 isn't in the dictionary at all, they can't be anagrams.
        // 5. After processing word2, if every count in the dictionary is exactly 0,
        //    the words used exactly the same letters the same number of times - they're anagrams.
        //    If any count is not 0, they are not anagrams.

        var cleaned1 = word1.Replace(" ", "").ToLower();
        var cleaned2 = word2.Replace(" ", "").ToLower();

        if (cleaned1.Length != cleaned2.Length)
        {
            return false;
        }

        var letterCounts = new Dictionary<char, int>();

        foreach (var c in cleaned1)
        {
            if (letterCounts.ContainsKey(c))
            {
                letterCounts[c] += 1;
            }
            else
            {
                letterCounts[c] = 1;
            }
        }

        foreach (var c in cleaned2)
        {
            if (!letterCounts.ContainsKey(c))
            {
                return false;
            }

            letterCounts[c] -= 1;
        }

        return letterCounts.Values.All(count => count == 0);
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        // PLAN:
        // 1. The classes in FeatureCollection.cs map the GeoJSON structure: a FeatureCollection
        //    has a list of Features, and each Feature has a Properties object containing
        //    "place" and "mag".
        // 2. Loop through featureCollection.Features (or use LINQ Select) and, for each feature,
        //    build a string in the format "{place} - Mag {mag}".
        // 3. Return the collected strings as an array.
        return featureCollection!.Features
            .Select(feature => $"{feature.Properties.Place} - Mag {feature.Properties.Mag}")
            .ToArray();
    }
}