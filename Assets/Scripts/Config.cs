using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

public class Config : MonoBehaviour
{
    #if UNITY_ANDROID && !UNITY_EDITOR
        private static string _dir = Application.persistentDataPath;
    #else
        private static string _dir = Directory.GetCurrentDirectory();
    #endif

    private static string _file = @"\board.data.ini";
    private static string _path = _dir + _file;

    public static void DeleteDataFile()
    {
        File.Delete(_path);
    }

    public static void SaveBoardData(SudokuData.SudokuBoardData boardData, string difficulty, int boardIndex, 
        int errorNumber, Dictionary<string, List<string>> gridNotes)
    {
        File.WriteAllText(_path, string.Empty);
        StreamWriter writer = new StreamWriter(_path, false);
        string currentTime = "#time:" + Clock.GetCurrentTime();
        string difficultyString = "#difficulty:" + difficulty;
        string errorNumberString = "#errors:" + errorNumber;
        string boardIndexString = "#board_index:" + boardIndex;
        string unsolvedString = "#unsolved:";
        string solvedString = "#solved:";

        foreach (var unsolvedCell in boardData.unsolvedData)
        {
            unsolvedString += unsolvedCell.ToString() + ",";
        }

        foreach (var solvedData in boardData.solvedData)
        {
            solvedString += solvedData.ToString() + ",";
        }

        writer.WriteLine(currentTime);
        writer.WriteLine(difficultyString);
        writer.WriteLine(errorNumberString);
        writer.WriteLine(boardIndexString);
        writer.WriteLine(unsolvedString);
        writer.WriteLine(solvedString);

        foreach (var square in gridNotes)
        {
            string squareString = "#" + square.Key + ":";
            bool save = false;

            foreach (var note in square.Value)
            {
                if (note != " ")
                {
                    squareString += note + ",";
                    save = true;
                }
            }

            if(save)
                writer.WriteLine(squareString);
        }

        writer.Close();
    }

    public static Dictionary<int, List<int>> GetGridNotes()
    {
        Dictionary<int, List<int>> gridNotes = new Dictionary<int, List<int>>(); 

        string line;

        StreamReader file = new StreamReader(_path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#square_note")
            {
                int squareIndex = -1;
                List<int> notes = new List<int>();
                int.TryParse(word[1], out squareIndex);

                string[] substring = Regex.Split(word[2], ",");

                foreach (var note in substring)
                {
                    int noteNumber = -1;
                    int.TryParse(note, out noteNumber);
                    if (noteNumber > 0)
                        notes.Add(noteNumber);
                }

                gridNotes.Add(squareIndex, notes);
            }
        }

        file.Close();

        return gridNotes;
    }

    public static string ReadBoardDifficulty()
    {
        string line;
        string difficulty = "";
        StreamReader file = new StreamReader(_path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#difficulty")
            {
                difficulty = word[1];
            }
        }

        file.Close();
        return difficulty;
    }

    public static SudokuData.SudokuBoardData ReadGridData()
    {
        string line;
        StreamReader file = new StreamReader(_path);

        int[] unsolvedData = new int[81];
        int[] solvedData = new int[81];

        int unsolvedIndex = 0;
        int solvedIndex = 0;

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(":");
            if (word[0] == "#unsolved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (var value in substrings)
                {
                    int squareNumber = -1;
                    if (int.TryParse(value, out squareNumber))
                    {
                        unsolvedData[unsolvedIndex] = squareNumber;
                        unsolvedIndex++;
                    }
                }
            }

            if (word[0] == "#solved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (var value in substrings)
                {
                    int squareNumber = -1;
                    if (int.TryParse(value, out squareNumber))
                    {
                        solvedData[solvedIndex] = squareNumber;
                        solvedIndex++;
                    }
                }
            }

        }

        file.Close();
        return new SudokuData.SudokuBoardData(unsolvedData, solvedData);
    }

    public static int ReadBoardLevel()
    {
        int level = -1;
        string line;
        StreamReader file = new StreamReader(_path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#board_index")
            {
                int.TryParse(word[1], out level);
            }
        }

        file.Close();
        return level;
    }

    public static float ReadGameTime()
    {
        float time = -1.0f;
        string line;

        StreamReader file = new StreamReader(_path); 

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#time")
            {
                float.TryParse(word[1], out time);
            }
        }

        file.Close();
        return time;
    }

    public static int ErrorNumber()
    {
        int errors = 0;
        string line;

        StreamReader file = new StreamReader(_path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#errors")
            {
                int.TryParse(word[1], out errors);
            }
        }

        file.Close(); 
        return errors;
    }

    public static bool GameDataFileExist()
    {
        return File.Exists(_path);
    }
}
