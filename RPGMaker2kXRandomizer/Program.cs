using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Media;
using System.Threading;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace RPGMaker2kXRandomizer
{ 
    public static class Program
    {
        public static IEnumerable<FileInfo> GetFilesByExtensions (this DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension));
        }

        static void Shuffle<T> (this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = new Random().Next(n + 1);

                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        static string CurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        static bool ShufflePalettes = true;
        static bool ShuffleFilenames = false;

        public static void Main (string[] args)
        {
            /*for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == "-nopalette") ShufflePalettes = false;
                if (args[i].ToLower() == "-name") ShuffleFilenames = true;
            }*/

            CMDStart:
            Console.WriteLine("RPG Maker 200X randomizer by mike309\nPress F1 to toggle palette randomization (currently {0})\nPress F2 to toggle file name randomization (currently {1})\nPress Enter to corrupt!",ShufflePalettes.ToString(),ShuffleFilenames.ToString());
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.F1) { ShufflePalettes = !ShufflePalettes; Console.Clear(); goto CMDStart; }
            if (key.Key == ConsoleKey.F2) { ShuffleFilenames = !ShuffleFilenames; Console.Clear(); goto CMDStart; }
            if (key.Key == ConsoleKey.Enter) { Console.Clear(); goto Randomize; }

            Randomize:
            if (Directory.Exists(CurrentPath + "Randomized\\"))
            {
                foreach (var file in new DirectoryInfo(CurrentPath + "Randomized\\").GetFiles())
                {
                    file.Delete();
                }
                foreach (var dir in new DirectoryInfo(CurrentPath + "Randomized\\").GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            for (int i = 0; i < 19; i++ )
            {
                string[] folder = {"Backdrop",
                                      "Battle",
                                      "Battle2",
                                      "BattleCharSet",
                                      "BattleWeapon",
                                      "CharSet",
                                      "ChipSet",
                                      "FaceSet",
                                      "Frame",
                                      "GameOver",
                                      "Monster",
                                      "Movie",
                                      "Music",
                                      "Panorama",
                                      "Picture",
                                      "Sound",
                                      "System",
                                      "System2",
                                      "Title"
                                  };

                if (!Directory.Exists(CurrentPath + folder[i]))
                {
                    Console.WriteLine(folder[i] + " doesn't exist, skipping.");
                    continue;
                }
                else
                {
                    Directory.CreateDirectory(CurrentPath + "Randomized\\" + folder[i]);
                    List<string> files = new List<string>();
                    List<Bitmap> images = new List<Bitmap>();
                    List<ColorPalette> palettes = new List<ColorPalette>();

                    foreach (var file in new DirectoryInfo(CurrentPath + folder[i]).GetFilesByExtensions(".png", ".bmp"))
                    {
                        files.Add(file.Name);
                        images.Add(new Bitmap(file.FullName));
                    }
                    Console.WriteLine(folder[i] + " - Images added");

                    foreach (var img in images)
                    {
                        palettes.Add(img.Palette);
                    }

                    Console.WriteLine(folder[i] + " - Palettes added");

                    if (ShuffleFilenames)
                    {
                        files.Shuffle();
                        Console.WriteLine(folder[i] + " - File names shuffled");
                    }
                    if (ShufflePalettes)
                    {
                        palettes.Shuffle();
                        Console.WriteLine(folder[i] + " - Palettes shuffled");
                    }

                    for (int j = 0; j < files.Count; j++)
                    {
                        images[j].Palette = palettes[j];
                        images[j].Save(CurrentPath + "Randomized\\" + folder[i] + "\\" + files[j]);
                        images[j].Dispose();
                    }
                }
            }
            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}