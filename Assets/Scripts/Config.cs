// Клас для роботи з конфігураційним файлом для зберігання та завантаження даних гри
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

public class Config : MonoBehaviour
{
    #if UNITY_ANDROID && !UNITY_EDITOR
        // Шлях до директорії для зберігання даних на платформі Android у випадку, якщо програма не запущена в редакторі Unity
        static string dir = Application.persistentDataPath;
    #else
        // Приватний шлях до поточної робочої директорії для зберігання даних, використовується, коли програма запущена не на платформі Android або в редакторі Unity
        private static string dir = Directory.GetCurrentDirectory();
    #endif

    static string file = @"\board.data.ini"; // Ім'я файлу для зберігання даних
    static string path = dir + file; // Повний шлях до файлу, об'єднання шляху до директорії ім'ям файлу

    // Видаляє конфігураційний файл
    public static void DeleteDataFile()
    {
        File.Delete(path);
    }

    // Зберігає дані ігрового поля: час, складність гри, індекс поля, кількість помилок та нотатки до клітинок у файл конфігурації
    public static void SaveBoardData(SudokuData.SudokuBoardData board_data, string difficulty, int board_index, 
        int error_number, Dictionary<string, List<string>> grid_notes)
    {
        File.WriteAllText(path, string.Empty);
        StreamWriter writer = new StreamWriter(path, false);
        string current_time = "#time:" + Clock.GetCurrentTime(); // Час
        string difficulty_string = "#difficulty:" + difficulty; // Складність
        string error_number_string = "#errors:" + error_number; // Кількість помилок
        string board_index_string = "#board_index:" + board_index; // Індекс поля
        string unsolved_string = "#unsolved:"; // Невирішені клітинки
        string solved_string = "#solved:"; // Вирішені клітинки

        foreach (var unsolved_data in board_data.unsolved_data)
        {
            unsolved_string += unsolved_data.ToString() + ","; // Додаємо значення невирішених клітинок
        }

        foreach (var solved_data in board_data.solved_data)
        {
            solved_string += solved_data.ToString() + ","; // Додаємо значення вирішених клітинок
        }

        writer.WriteLine(current_time); // Записуємо час в файл
        writer.WriteLine(difficulty_string); // Записуємо складність в файл
        writer.WriteLine(error_number_string); // Записуємо кількість помилок в файл
        writer.WriteLine(board_index_string); // Записуємо індекс поля в файл
        writer.WriteLine(unsolved_string); // Записуємо невирішені клітинки в файл
        writer.WriteLine(solved_string); // Записуємо вирішені клітинки в файл

        // { Записуємо комірки з нотатками {
        foreach (var square in grid_notes)
        {
            string square_string = "#" + square.Key + ":";
            bool save = false;

            foreach (var note in square.Value)
            {
                if (note != " ")
                {
                    square_string += note + ",";
                    save = true;
                }
            }

            if(save)
                writer.WriteLine(square_string);
        }
        // } Записуємо комірки з нотатками }

        writer.Close();
    }

    // Отримує нотатки для клітинок з конфігураційного файлу
    public static Dictionary<int, List<int>> GetGridNotes()
    {
        Dictionary<int, List<int>> grid_notes = new Dictionary<int, List<int>>(); // Словник, де ключами є цілі числа, а значеннями
        // є списки цілих чисел. В даному випадку ключем є номер комірки, а списком представлені нотатки в комірці
        string line; // Рядоки з файлу в форматі: #square_note:(номер комірки):(нотактки через кому (','))
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#square_note")
            {
                int square_index = -1;
                List<int> notes = new List<int>();
                int.TryParse(word[1], out square_index);

                string[] substring = Regex.Split(word[2], ",");

                foreach (var note in substring)
                {
                    int note_number = -1;
                    int.TryParse(note, out note_number);
                    if (note_number > 0)
                        notes.Add(note_number);
                }

                grid_notes.Add(square_index, notes);
            }
        }

        file.Close();

        return grid_notes;
    }

    // Зчитує складність гри з конфігураційного файлу
    public static string ReadBoardDifficulty()
    {
        string line; // Рядок з файлу в форматі: #time:(час)
        string difficulty = ""; // Складність гри
        StreamReader file = new StreamReader(path);

        // { Логіка зчитування складності гри {
        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#difficulty")
            {
                difficulty = word[1];
            }
        }
        // } Логіка зчитування складності гри }

        file.Close();
        return difficulty;
    }

    // Зчитує дані ігрового поля (вирішені й невирішені комірки) з конфігураційного файлу
    public static SudokuData.SudokuBoardData ReadGridData()
    {
        string line; // Рядки з файлу в форматах: #unsolved: (невирішені комірки через кому (',')); #solved: (вирішені комірки через кому (','))
        StreamReader file = new StreamReader(path);

        int[] unsolved_data = new int[81]; // Невирішені комірки (осередки судоку)
        int[] solved_data = new int[81]; // Вирішені комірки (осередки судоку)

        int unsolved_index = 0; // Ідекс невирішених квадратів (осередків судоку)
        int solved_index = 0; // Ідекс вирішених квадратів (осередків судоку)

        // { Логіка зчитування невирішених й вирішених комірок сітки {
        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(":");
            if (word[0] == "#unsolved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (var value in substrings)
                {
                    int square_number = -1;
                    if (int.TryParse(value, out square_number))
                    {
                        unsolved_data[unsolved_index] = square_number;
                        unsolved_index++;
                    }
                }
            }

            if (word[0] == "#solved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (var value in substrings)
                {
                    int square_number = -1;
                    if (int.TryParse(value, out square_number))
                    {
                        solved_data[solved_index] = square_number;
                        solved_index++;
                    }
                }
            }

        }
        // } Логіка зчитування невирішених й вирішених комірок сітки }

        file.Close();
        return new SudokuData.SudokuBoardData(unsolved_data, solved_data);
    }

    // Зчитує індекс рівня гри з конфігураційного файлу
    public static int ReadBoardLevel()
    {
        int level = -1; // Індекс рівня гри (сюди отримаємо індекс рівня)
        string line; // Рядок з файлу в форматі: #board_index:(індекс рівня гри)
        StreamReader file = new StreamReader(path);

        // { Логіка зчитування індексу рівня гри {
        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#board_index")
            {
                int.TryParse(word[1], out level);
            }
        }
        // } Логіка зчитування індексу рівня гри }

        file.Close();
        return level;
    }

    // Зчитує час гри з конфігураційного файлу
    public static float ReadGameTime()
    {
        float time = -1.0f; // Час (сюди отримаємо час)
        string line; // Рядок з файлу в форматі: #time:(час)

        StreamReader file = new StreamReader(path); 

        // { Логіка зчитування часу {
        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#time")
            {
                float.TryParse(word[1], out time);
            }
        }
        // } Логіка зчитування часу }

        file.Close();
        return time;
    }

    // Зчитує кількість помилок з конфігураційного файлу
    public static int ErrorNumber()
    {
        int errors = 0; // Кіклькість помилок (сюди отримаємо кількість помилок)
        string line; // Рядок з файлу в форматі: #errors:(кількість помилок)

        StreamReader file = new StreamReader(path);

        // { Логіка зчитування кількості помилок {
        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#errors")
            {
                int.TryParse(word[1], out errors);
            }
        }
        // } Логіка зчитування кількості помилок }

        file.Close(); 
        return errors;
    }

    // Перевіряє чи існує конфігураційний файл
    public static bool GameDataFileExist()
    {
        return File.Exists(path);
    }
}
