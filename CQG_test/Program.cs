using System;
using System.Collections.Generic;

public class SpellChecker
{
    public static void Main(string[] args)
    {
        List<string> dictionary = new List<string>();
        List<string> text = new List<string>();
        bool readingDictionary = true;
        string line;

        Console.WriteLine("Введите словарь и текст:");

        while ((line = Console.ReadLine()) != null)
        {
            if (line.Trim() == "===")
            {
                readingDictionary = !readingDictionary;
                if (!readingDictionary)
                {
                    Console.WriteLine("Словарь считан.");
                }
                else
                {
                    Console.WriteLine("Текст считан.");
                    break;
                }
                continue;
            }

            if (readingDictionary)
            {
                dictionary.AddRange(line.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                text.Add(line);
            }
        }

        HashSet<string> dictionarySet = new HashSet<string>(dictionary, StringComparer.OrdinalIgnoreCase);
        List<string> output = new List<string>();

        foreach (var textLine in text)
        {
            var words = textLine.Split(' ');
            List<string> result = new List<string>();

            foreach (var word in words)
            {
                string correctedWord = CorrectWord(word, dictionarySet, dictionary);
                result.Add(correctedWord);
            }

            output.Add(string.Join(" ", result));
        }

        Console.WriteLine("Результат:");
        foreach (var lineOutput in output)
        {
            Console.WriteLine(lineOutput);
        }
        Console.ReadKey();
    }

    public static string CorrectWord(string word, HashSet<string> dictionarySet, List<string> dictionary)
    {
        if (dictionarySet.Contains(word.ToLower()))
        {
            return word;
        }

        List<string> oneEditCandidates = new List<string>();
        List<string> twoEditCandidates = new List<string>();

        foreach (var dictWord in dictionary)
        {
            if (IsOneEditDistance(word, dictWord))
            {
                oneEditCandidates.Add(dictWord);
            }
            else if (IsTwoEditDistance(word, dictWord))
            {
                twoEditCandidates.Add(dictWord);
            }
        }

        if (oneEditCandidates.Count > 0)
        {
            if (oneEditCandidates.Count == 1)
            {
                return oneEditCandidates[0];
            }
            return "{" + string.Join(" ", oneEditCandidates) + "}";
        }
        else if (twoEditCandidates.Count > 0)
        {
            if (twoEditCandidates.Count == 1)
            {
                return twoEditCandidates[0];
            }
            return "{" + string.Join(" ", twoEditCandidates) + "}";
        }

        return "{" + word + "?}";
    }

    public static bool IsOneEditDistance(string word1, string word2)
    {
        int len1 = word1.Length;
        int len2 = word2.Length;

        if (Math.Abs(len1 - len2) > 1)
        {
            return false;
        }

        int i = 0, j = 0, edits = 0;

        while (i < len1 && j < len2)
        {
            if (word1[i] != word2[j])
            {
                if (++edits > 1)
                {
                    return false;
                }

                if (len1 > len2)
                {
                    i++;
                }
                else if (len1 < len2)
                {
                    j++;
                }
                else
                {
                    i++;
                    j++;
                }
            }
            else
            {
                i++;
                j++;
            }
        }

        if (i < len1 || j < len2)
        {
            edits++;
        }

        return edits == 1;
    }

    public static bool IsTwoEditDistance(string word1, string word2)
    {
        int len1 = word1.Length;
        int len2 = word2.Length;

        if (Math.Abs(len1 - len2) > 2)
        {
            return false;
        }

        int i = 0, j = 0, edits = 0;

        while (i < len1 && j < len2)
        {
            if (word1[i] != word2[j])
            {
                edits++;
                if (edits > 2)
                {
                    return false;
                }

                if (len1 > len2)
                {
                    i++;
                }
                else if (len1 < len2)
                {
                    j++;
                }
                else
                {
                    i++;
                    j++;
                }
            }
            else
            {
                i++;
                j++;
            }
        }

        edits += len1 - i + len2 - j;

        return edits == 2;
    }
}
