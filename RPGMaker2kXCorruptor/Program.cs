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

namespace RPGMaker2kXCorruptor
{ 
    public static class Program
    {
        public static Random rng = new Random();

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
                int k = rng.Next(n + 1);

                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /*public static Image MyConvert(Bitmap oldbmp)
        {
            using (var ms = new MemoryStream())
            {
                oldbmp.Save(ms, ImageFormat.Gif);
                ms.Position = 0;
                return Image.FromStream(ms);
            }
        }*/

        static readonly string CurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        static bool ShufflePalettes = true;
        static bool ShuffleFilenames = false;
        static bool ShuffleOnlyFilenames = false;

        static bool DoBackdrop = true;
        static bool DoBattle = true;
        static bool DoBattleTwo = true;
        static bool DoBattleCharSet = true;
        static bool DoBattleWeapon = true;
        static bool DoCharSet = true;
        static bool DoChipSet = true;
        static bool DoFaceSet = true;
        static bool DoFrame = true;
        static bool DoGameOver = true;
        static bool DoMonster = true;
        static bool DoMovie = true;
        static bool DoMusic = true;
        static bool DoPanorama = true;
        static bool DoPicture = true;
        static bool DoSound = true;
        static bool DoSystem = true;
        static bool DoSystemTwo = true;
        static bool DoTitle = true;

        //test
        static bool Verbosity = false;

        public static void Main (string[] args)
        {
            /*for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == "-nopalette") ShufflePalettes = false;
                if (args[i].ToLower() == "-name") ShuffleFilenames = true;
            }*/

            Console.WriteLine("RNG: (only numbers please)");

            var k = Console.Read();
            rng = new Random(Convert.ToInt32(k));
            Console.Clear();

            CMDStart:
            Console.WriteLine("RPG Maker 200X corruptor v2.1.0.4 by mike309\nPress F1 to toggle palette randomization (currently {0})\nPress F2 to toggle file name randomization (currently {1})\nPress Enter to corrupt!\nPress F3 for more options\nPress F4 to shuffle only filenames (currently {2})",ShufflePalettes.ToString(),ShuffleFilenames.ToString(),ShuffleOnlyFilenames.ToString());
            var key = Console.ReadKey();

            switch (key.Key)
            {
                default:
                    Console.Clear();
                    goto CMDStart;
                    break;

                case ConsoleKey.F1:
                    ShufflePalettes = !ShufflePalettes;
                    ShuffleOnlyFilenames = false;
                    goto default;
                    break;

                case ConsoleKey.F2:
                    ShuffleFilenames = !ShuffleFilenames;
                    ShuffleOnlyFilenames = false;
                    goto default;
                    break;

                case ConsoleKey.Enter:
                    Console.Clear();
                    goto Randomize;
                    break;

                case ConsoleKey.F3:
                    Console.Clear();
                    goto Option;
                    break;

                case ConsoleKey.F4:
                    ShufflePalettes = false;
                    ShuffleFilenames = false;
                    ShuffleOnlyFilenames =! ShuffleOnlyFilenames;
                    goto default;
                    break;
            }

            Option:
            Console.Clear();
            Console.WriteLine(" F1 = Backdrop {0}\n F2 = Battle {1}\n F3 = Battle2 {2}\n F4 = BattleCharSet {3}\n F5 = BattleWeapon {4}\n F6 = CharSet {5}\n F7 = ChipSet {6}\n F8 = FaceSet {7}\n F9 = Frame {8}\n F10 = GameOver {9}\n F11 = Monster {10}\n F12 = Movie {11}\n 1 = Music {12}\n 2 = Panorama {13}\n 3 = Picture {14}\n 4 = Sound {15}\n 5 = System {16}\n 6 = System2 {17}\n 7 = Title {18}\n 8 = Verbosity {19}\n Press Enter to go back",DoBackdrop.ToString(),DoBattle.ToString(),DoBattleTwo.ToString(),DoBattleCharSet.ToString(),DoBattleWeapon.ToString(),DoCharSet.ToString(),DoChipSet.ToString(),DoFaceSet.ToString(),DoFrame.ToString(),DoGameOver.ToString(),DoMonster.ToString(),DoMovie.ToString(),DoMusic.ToString(),DoPanorama.ToString(),DoPicture.ToString(),DoSound.ToString(),DoSystem.ToString(),DoSystemTwo.ToString(),DoTitle.ToString(),Verbosity);
            var keyt = Console.ReadKey();

            switch (keyt.Key)
            {
                default:
                    Console.Clear();
                    goto Option;
                    break;

                case ConsoleKey.Enter:
                    Console.Clear();
                    goto CMDStart;
                    break;

                case ConsoleKey.F1:
                    DoBackdrop = !DoBackdrop;
                    goto default;
                    break;

                case ConsoleKey.F2:
                    DoBattle = !DoBattle;
                    goto default;
                    break;

                case ConsoleKey.F3:
                    DoBattleTwo = !DoBattleTwo;
                    goto default;
                    break;

                case ConsoleKey.F4:
                    DoBattleCharSet = !DoBattleCharSet;
                    goto default;
                    break;

                case ConsoleKey.F5:
                    DoBattleWeapon = !DoBattleWeapon;
                    goto default;
                    break;

                case ConsoleKey.F6:
                    DoCharSet = !DoCharSet;
                    goto default;
                    break;

                case ConsoleKey.F7:
                    DoChipSet = !DoChipSet;
                    goto default;
                    break;

                case ConsoleKey.F8:
                    DoFaceSet = !DoFaceSet;
                    goto default;
                    break;

                case ConsoleKey.F9:
                    DoFrame = !DoFrame;
                    goto default;
                    break;

                case ConsoleKey.F10:
                    DoGameOver = !DoGameOver;
                    goto default;
                    break;

                case ConsoleKey.F11:
                    DoMonster = !DoMonster;
                    goto default;
                    break;

                case ConsoleKey.F12:
                    DoMovie =! DoMovie;
                    goto default;
                    break;

                case ConsoleKey.D1:
                    DoMusic = !DoMusic;
                    goto default;
                    break;

                case ConsoleKey.D2:
                    DoPanorama = !DoPanorama;
                    goto default;
                    break;

                case ConsoleKey.D3:
                    DoPicture = !DoPicture;
                    goto default;
                    break;

                case ConsoleKey.D4:
                    DoSound = !DoSound;
                    goto default;
                    break;

                case ConsoleKey.D5:
                    DoSystem = !DoSystem;
                    goto default;
                    break;

                case ConsoleKey.D6:
                    DoSystemTwo = !DoSystemTwo;
                    goto default;
                    break;

                case ConsoleKey.D7:
                    DoTitle = !DoTitle;
                    goto default;
                    break;

                case ConsoleKey.D8:
                    Verbosity = !Verbosity;
                    goto default;
                    break;
            }

            Randomize:
            string[] folder = new string[19];
            if (Directory.Exists(CurrentPath + "Corrupted\\"))
            {
                foreach (var file in new DirectoryInfo(CurrentPath + "Corrupted\\").GetFiles())
                {
                    file.Delete();
                }
                foreach (var dir in new DirectoryInfo(CurrentPath + "Corrupted\\").GetDirectories())
                {
                    dir.Delete(true);
                }
            }

            for (int i = 0; i < 19; i++ )
            {
                switch (i)
                {
                    case 0:
                        if (DoBackdrop) { folder[i] = "Backdrop"; continue; }
                        break;

                    case 1:
                        if (DoBattle) { folder[i] = "Battle"; continue; }
                        break;

                    case 2:
                        if (DoBattleTwo) { folder[i] = "Battle2"; continue; }
                        break;

                    case 3:
                        if (DoBattleCharSet) { folder[i] = "BattleCharSet"; continue; }
                        break;

                    case 4:
                        if (DoBattleWeapon) { folder[i] = "BattleWeapon"; continue; }
                        break;

                    case 5:
                        if (DoCharSet) { folder[i] = "CharSet"; continue; }
                        break;

                    case 6:
                        if (DoChipSet) { folder[i] = "ChipSet"; continue; }
                        break;

                    case 7:
                        if (DoFaceSet) { folder[i] = "FaceSet"; continue; }
                        break;

                    case 8:
                        if (DoFrame) { folder[i] = "Frame"; continue; }
                        break;

                    case 9:
                        if (DoGameOver) { folder[i] = "GameOver"; continue; }
                        break;

                    case 10:
                        if (DoMonster) { folder[i] = "Monster"; continue; }
                        break;

                    case 11:
                        if (DoMovie) { folder[i] = "Movie"; continue; }
                        break;

                    case 12:
                        if (DoMusic) { folder[i] = "Music"; continue; }
                        break;

                    case 13:
                        if (DoPanorama) { folder[i] = "Panorama"; continue; }
                        break;

                    case 14:
                        if (DoPicture) { folder[i] = "Picture"; continue; }
                        break;

                    case 15:
                        if (DoSound) { folder[i] = "Sound"; continue; }
                        break;

                    case 16:
                        if (DoSystem) { folder[i] = "System"; continue; }
                        break;

                    case 17:
                        if (DoSystemTwo) { folder[i] = "System2"; continue; }
                        break;

                    case 18:
                        if (DoTitle) { folder[i] = "Title"; continue; }
                        break;
                }
                folder[i] = "Skipped";
            }

            for (int i = 0; i < 19; i++ )
            {
                if (!Directory.Exists(CurrentPath + folder[i]))
                {
                    Console.WriteLine(folder[i] + " doesn't exist, skipping.");
                    continue;
                }
                else
                {
                    Directory.CreateDirectory(CurrentPath + "Corrupted\\" + folder[i]);
                    if (ShuffleOnlyFilenames)
                    {
                        List<string> filenames = new List<string>();

                        foreach (var file in new DirectoryInfo(CurrentPath + folder[i]).GetFiles())
                        {
                            filenames.Add(file.Name);
                            //files.Add(File.ReadAllBytes(file.FullName));
                        }

                        filenames.Shuffle();

                        int j = 0;
                        foreach (var file in new DirectoryInfo(CurrentPath + folder[i]).GetFiles())
                        {
                            File.WriteAllBytes(CurrentPath + "Corrupted\\" + folder[i] + "\\" + filenames[j], File.ReadAllBytes(file.FullName));
                            j++;
                        }
                    }
                    else
                    {
                        List<string> files = new List<string>();
                        List<Image> images = new List<Image>();
                        List<ColorPalette> palettes = new List<ColorPalette>();

                        foreach (var file in new DirectoryInfo(CurrentPath + folder[i]).GetFilesByExtensions(".png", ".PNG", ".bmp", ".gif"))
                        {
                            if(Verbosity) { Console.WriteLine("About to add '{0}' to 'files'",file.Name); }
                            files.Add(file.Name);
                            if (Verbosity) { Console.WriteLine("Added '{0}' to 'files'", file.Name); }

                            if (Verbosity) { Console.WriteLine("About to add '{0}' to 'images'", file.Name); }
                            images.Add(new Bitmap(file.FullName));
                            if (Verbosity) { Console.WriteLine("Added '{0}' to 'images'", file.Name); }
                        }

                        Console.WriteLine(folder[i] + " - All images added");
                        if (true)
                        {
                            int j = 0;
                            foreach (var img in images)
                            {
                                if (Verbosity) { Console.WriteLine("About to add '{0}' to 'palettes'", files[j]); }
                                palettes.Add(img.Palette);
                                if (Verbosity) { Console.WriteLine("Added '{0}' to 'palettes'", files[j]); }
                                j++;
                            }

                            Console.WriteLine(folder[i] + " - Palettes added");
                        }

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
                            if (true) {
                                if(Verbosity) { Console.WriteLine("About to set '{0}' palette to a random one", files[j]); }
                                images[j].Palette = palettes[j];
                                if(Verbosity) { Console.WriteLine("Set '{0}' palette to a random one", files[j]); }
                            }

                            if (files[j].Contains(".png"))
                            {
                                Console.WriteLine("About to save '{0}' to '{1}'",files[j],folder[i]);
                                images[j].Save(CurrentPath + "Corrupted\\" + folder[i] + "\\" + files[j], ImageFormat.Png);
                                Console.WriteLine("Saved '{0}' to '{1}'", files[j], folder[i]);
                            }
                            else
                            {
                                Console.WriteLine("About to save '{0}' to '{1}'", files[j], folder[i]);
                                images[j].Save(CurrentPath + "Corrupted\\" + folder[i] + "\\" + files[j], ImageFormat.Bmp);
                                Console.WriteLine("Saved '{0}' to '{1}'", files[j], folder[i]);
                            }
                            images[j].Dispose();
                        }
                    }
                }
            } goto End;

            End:
            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}