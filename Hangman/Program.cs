using System.Text;
using System.Text.RegularExpressions;

namespace Hangman
{
    class Program
    {
        static void Main()
        {
            TitleHangman();
            string theme = "";

            // Words in an array of strings (and the theme in form of a string for the words).
            string[] words = GetWords(ref theme);
            bool runGame = true;
            while (runGame)
            {
                runGame = GameDisplay(theme, words);
            }
        }

        private static bool GameDisplay(string theme, string[] words)
        {
            PlayOrQuitText();
            string playOrQuit = (Console.ReadLine().ToUpper());
            if (string.IsNullOrEmpty(playOrQuit))
            {
                return true;
            }
            if (playOrQuit == "Q")
            {
                return false;
            }
            if (playOrQuit == "P")
            {
                Console.Clear();

                string[][] hangingGraphics = new string[10][];
                HangingGraphics(hangingGraphics);

                // The secret word randomly chosen from an array of Strings
                Random randomNumber = new Random();
                int indexNumber = randomNumber.Next(0, words.Length);
                string secretWord = words[indexNumber];

                // The correct letters inside a char array. 
                int secretWordLength = words[indexNumber].Length;
                char[] secretLetters = new char[secretWordLength];

                int letterTally = 0;
                int spaceTally = 0;
                int otherTally = secretWord.Length - letterTally - spaceTally;

                // Unrevealed letters represented by a lower dash(_).
                HideWord(secretLetters, secretWord, ref letterTally, ref spaceTally);
                int hiddenLetters = letterTally;
                int visibleCharacters = letterTally - hiddenLetters;

                // The incorrect letters the player has guessed, should be put inside a StringBuilder and presented to the player after each guess
                StringBuilder flunkedLetters = new StringBuilder();

                // The player has 10 guesses to complete the word before losing the game.
                for (int usedGuesses = 0; usedGuesses < 10; usedGuesses++)
                {
                    var winCheck = new string(secretLetters);
                    if (winCheck == secretWord)
                    {
                        CorrectWordText(secretWord, secretLetters, flunkedLetters, usedGuesses, hiddenLetters, spaceTally, visibleCharacters, theme, hangingGraphics);
                        return true;
                    }
                    GameText(secretWord, secretLetters, flunkedLetters, usedGuesses, hiddenLetters, spaceTally, visibleCharacters, theme, hangingGraphics);

                    // The player can make two type of guesses; letter or word.
                    string inputGuess = (Console.ReadLine().ToUpper());
                    int inputLength = inputGuess.Length;
                    if (inputLength != 0)
                    {
                        // Guesses a specific letter.
                        if (inputLength == 1)
                        {
                            Regex englishLetters = new Regex(@"[a-zA-Z]");
                            if (englishLetters.IsMatch(inputGuess))
                            {
                                var usedLetterCheck = new string(secretLetters) + flunkedLetters;
                                if (!usedLetterCheck.Contains(inputGuess))
                                {
                                    if (secretWord.Contains(inputGuess))
                                    {
                                        // If player guess a letter that occurs in the word, the program update by inserting the letter in the correct position(s).
                                        GuessLetter(char.Parse(inputGuess), secretWord, secretLetters, ref hiddenLetters, ref visibleCharacters);
                                        usedGuesses--;
                                    }
                                    else
                                    {
                                        flunkedLetters.Append(inputGuess);
                                        Console.Clear();
                                    }
                                }
                                // If the player guesses the same letter twice, the program will not consume a guess.
                                else
                                {
                                    usedGuesses--;
                                    Console.Clear();
                                }
                            }
                            else
                            {
                                usedGuesses--;
                                Console.Clear();
                            }
                        }
                        else
                        {
                            if (inputLength == secretWord.Length)
                            {
                                // Guess for the whole word. If the guess is correct player wins the game and the whole word is revealed. If the word is incorrect nothing should get revealed.
                                if (inputGuess == secretWord)
                                {
                                    // Fill hidden letters with the correct hidden word when player guess the right word.
                                    AllCorrectLetters(secretWord, secretLetters);
                                    usedGuesses--;
                                }
                                else
                                {
                                    Console.Clear();
                                }
                            }
                            else
                            {
                                usedGuesses--;
                                Console.Clear();
                            }
                        }
                    }
                    else
                    {
                        usedGuesses--;
                        Console.Clear();
                    }
                }
                GameOverText(secretWord, secretLetters, flunkedLetters, hiddenLetters, spaceTally, visibleCharacters, theme, hangingGraphics);
                return true;
            }
            else
            {
                return true;
            }
        }

        static void GameText(string secretWord, char[] secretLetters, StringBuilder flunkedLetters, int usedGuesses, int hiddenLetters, int spaceTally, int visibleCharacters, string theme, string[][] hangingGraphics)
        {
            TitleHangman();

            // Calculates how much space that needs to be filled in so the hanging graphics is placed correctly.
            string spacingAfterTheme = new string(' ', 63 - theme.Length);
            string spacingAfterFlunked = new string(' ', 48 - flunkedLetters.Length);

            // Check if there is a two digit value. In order to display the singular in the same position if the value is in the tens.
            bool spacingLengthDigits = secretWord.Length > 9;
            bool spacingHiddenDigits = hiddenLetters > 9;
            bool spacingBlankDigits = spaceTally > 9;
            bool spacingRevealedDigits = visibleCharacters > 9;
            bool spacingUsedGuessesDigits = usedGuesses > 9;

            // Check if the secret word is long. In order to properly display the secret word in a 120 character wide console.
            bool spacingSecretWord = secretWord.Length > 42;

            Console.WriteLine("");
            Console.WriteLine(" Theme: " + theme + spacingAfterTheme + hangingGraphics[0][flunkedLetters.Length]);
            Console.WriteLine("                                                                       " + hangingGraphics[1][flunkedLetters.Length]);
            Console.WriteLine(" Length:   " + (spacingLengthDigits ? "" : " ") + secretWord.Length + "                                                          " + hangingGraphics[2][flunkedLetters.Length]);
            Console.WriteLine(" Hidden:   " + (spacingHiddenDigits ? "" : " ") + hiddenLetters + "                                                          " + hangingGraphics[3][flunkedLetters.Length]);
            Console.WriteLine(" Blank:    " + (spacingBlankDigits ? "" : " ") + spaceTally + "                                                          " + hangingGraphics[4][flunkedLetters.Length]);
            Console.WriteLine(" Revealed: " + (spacingRevealedDigits ? "" : " ") + visibleCharacters + "                                                          " + hangingGraphics[5][flunkedLetters.Length]);
            Console.WriteLine("                                                                       " + hangingGraphics[6][flunkedLetters.Length]);
            Console.WriteLine(" Failed guesses: " + usedGuesses + " out of 10." + (spacingUsedGuessesDigits ? "" : " ") + "                                         " + hangingGraphics[7][flunkedLetters.Length]);
            Console.WriteLine("                                                                       " + hangingGraphics[8][flunkedLetters.Length]);
            Console.WriteLine(" Strucked out letters: " + flunkedLetters + spacingAfterFlunked + hangingGraphics[9][flunkedLetters.Length]);
            Console.WriteLine("");
            Console.Write(spacingSecretWord ? " Guess letter/line: " : " You can guess a letter or the whole wording: ");
            Console.Write(secretLetters);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write(spacingSecretWord ? "                    " : "                                              ");
        }

        static void GameOverText(string secretWord, char[] secretLetters, StringBuilder flunkedLetters, int hiddenLetters, int spaceTally, int visibleCharacters, string theme, string[][] hangingGraphics)
        {
            // Gets the first 32 characters from theme into a substring in order for the text to not go past the hanging G in the graphics.
            string shortTheme;
            if (theme.Length > 31)
            {
                shortTheme = theme.Substring(0, 30);
            }
            else
            {
                shortTheme = theme;
            }
            string spacingAfterShortTheme = new string(' ', 30 - theme.Length);
            string spacingAfterFlunked = new string(' ', 32 - flunkedLetters.Length);
            bool spacingLengthDigits = secretWord.Length > 9;
            bool spacingHiddenDigits = hiddenLetters > 9;
            bool spacingBlankDigits = spaceTally > 9;
            bool spacingRevealedDigits = visibleCharacters > 9;
            bool spacingSecretWord = secretWord.Length > 42;

            Console.WriteLine(@"     |)          (|          |)          (|          (|          |)          (|       ");
            Console.WriteLine(@"     (|          |)          (|          |)          |)          (|          |)       ");
            Console.WriteLine(@"     |)__        (|__        |)__        (|          (|__        |)__        (|__     ");
            Console.WriteLine(@"     /  /\       /  /\       /  /\       |)          /  /\       /  /\       /  /\    ");
            Console.WriteLine(@"    /  /:/      /  /::\     /  /::|      (|         /  /::|     /  /::\     /  /::|   ");
            Console.WriteLine(@"   /  /:/      /  /:/\:\   /  /:|:|      |)        /  /:|:|    /  /:/\:\   /  /:|:|   ");
            Console.WriteLine(@"  /  /::\ ___ /  /::\ \:\ /  /:/|:|__    (|       /  /:/|:|__ /  /::\ \:\ /  /:/|:|__ ");
            Console.WriteLine(@" /__/:/\:\  //__/:/\:\_\:/__/:/ |:| /\   |)      /__/:/ |::::/__/:/\:\_\:/__/:/ |:| /\");
            Console.WriteLine(@" \__\/  \:\/:\__\/  \:\/:\__\/  |:|/:/   (|      \__\/'  ~~/:\__\/  \:\/:\__\/  |:|/:/");
            Console.WriteLine(@"      \__\::/     \__\::/    |  |:/:/    |)            /  /:/     \__\::/    |  |:/:/ ");
            Console.WriteLine(@"      /  /:/      /  /:/     |_ |::/     (|           /  /:/      /  /:/     |_ |::/  ");
            Console.WriteLine(@"     /__/:/      /__/:/      /__/:/      |)__        /__/:/      /__/:/      /__/:/   ");
            Console.WriteLine(@"     \__\/       \__\/       \__\/       /  /\       \__\/       \__\/       \__\/    ");
            Console.WriteLine(@"                                        /  /::\         ");
            Console.WriteLine(@"                                       /  /:/\:\           ");
            Console.WriteLine(" Theme: " + shortTheme + spacingAfterShortTheme + @"/  /:/  \:\                      " + hangingGraphics[0][flunkedLetters.Length]);
            Console.WriteLine(@"                                     /__/:/_\_ \:\                     " + hangingGraphics[1][flunkedLetters.Length]);
            Console.WriteLine(" Length:   " + (spacingLengthDigits ? "" : " ") + secretWord.Length + @"                        \  \:\__/\_\/                     " + hangingGraphics[2][flunkedLetters.Length]);
            Console.WriteLine(" Hidden:   " + (spacingHiddenDigits ? "" : " ") + hiddenLetters + @"                         \  \:\ \:\                       " + hangingGraphics[3][flunkedLetters.Length]);
            Console.WriteLine(" Blank:    " + (spacingBlankDigits ? "" : " ") + spaceTally + @"                          \  \:\/:/                       " + hangingGraphics[4][flunkedLetters.Length]);
            Console.WriteLine(" Revealed: " + (spacingRevealedDigits ? "" : " ") + visibleCharacters + @"                           \  \::/                        " + hangingGraphics[5][flunkedLetters.Length]);
            Console.WriteLine(@"                                         \__\/ A M E                   " + hangingGraphics[6][flunkedLetters.Length]);
            Console.WriteLine(" Failed guesses: 10 out of 10." + @"                        O V E R !        " + hangingGraphics[7][flunkedLetters.Length]);
            Console.WriteLine(@"                                                                       " + hangingGraphics[8][flunkedLetters.Length]);
            Console.WriteLine(" Strucked out letters: " + flunkedLetters + spacingAfterFlunked + "                " + hangingGraphics[9][flunkedLetters.Length]);
            Console.WriteLine("");
            Console.Write(spacingSecretWord ? " Didn't get 'em all " : " You were not able to get the whole wording:  ");
            Console.WriteLine(secretLetters);
            Console.Write(spacingSecretWord ? "                    " : "                                              ");
            Console.Write(secretWord);
            Console.WriteLine("");
        }

        static void CorrectWordText(string secretWord, char[] secretLetters, StringBuilder flunkedLetters, int usedGuesses, int hiddenLetters, int spaceTally, int visibleCharacters, string theme, string[][] hangingGraphics)
        {
            TitleHangman();

            string spacingAfterTheme = new string(' ', 63 - theme.Length);
            string spacingAfterFlunked = new string(' ', 48 - flunkedLetters.Length);
            bool spacingLengthDigits = secretWord.Length > 9;
            bool spacingHiddenDigits = hiddenLetters > 9;
            bool spacingBlankDigits = spaceTally > 9;
            bool spacingRevealedDigits = visibleCharacters > 9;
            bool spacingUsedGuessesDigits = usedGuesses > 9;
            bool spacingSecretWord = secretWord.Length > 42;

            Console.WriteLine("");
            Console.WriteLine(" Theme: " + theme + spacingAfterTheme + hangingGraphics[0][flunkedLetters.Length]);
            Console.WriteLine("                                                                       " + hangingGraphics[1][flunkedLetters.Length]);
            Console.WriteLine(" Length:   " + (spacingLengthDigits ? "" : " ") + secretWord.Length + "                                                          " + hangingGraphics[2][flunkedLetters.Length]);
            Console.WriteLine(" Hidden:   " + (spacingHiddenDigits ? "" : " ") + hiddenLetters + "                                                          " + hangingGraphics[3][flunkedLetters.Length]);
            Console.WriteLine(" Blank:    " + (spacingBlankDigits ? "" : " ") + spaceTally + "                                                          " + hangingGraphics[4][flunkedLetters.Length]);
            Console.WriteLine(" Revealed: " + (spacingRevealedDigits ? "" : " ") + visibleCharacters + "                                                          " + hangingGraphics[5][flunkedLetters.Length]);
            Console.WriteLine("                                                                       " + hangingGraphics[6][flunkedLetters.Length]);
            Console.WriteLine(" Failed guesses: " + usedGuesses + " out of 10." + (spacingUsedGuessesDigits ? "" : " ") + "                                         " + hangingGraphics[7][flunkedLetters.Length]);
            Console.WriteLine("                                                                       " + hangingGraphics[8][flunkedLetters.Length]);
            Console.WriteLine(" Strucked out letters: " + flunkedLetters + spacingAfterFlunked + hangingGraphics[9][flunkedLetters.Length]);
            Console.WriteLine("");
            Console.Write(spacingSecretWord ? " Winner! Got 'em all" : " Winner!      You revealed the whole wording: ");
            Console.Write(secretWord);
            Console.WriteLine("");
            Console.WriteLine("");
        }

        static void PlayOrQuitText()
        {
            Console.Write(" Type P and press Enter to Play.                      Type Q and press Enter to Quit.");
            Console.WriteLine("");
            Console.Write(" ");
        }

        static void TitleHangman()
        {
            Console.WriteLine(@"     |)          (|          |)          |)          (|          |)          (|       ");
            Console.WriteLine(@"     (|          |)          (|          (|          |)          (|          |)       ");
            Console.WriteLine(@"     |)__        (|__        |)__        |)__        (|__        |)__        (|__     ");
            Console.WriteLine(@"     /  /\       /  /\       /  /\       /  /\       /  /\       /  /\       /  /\    ");
            Console.WriteLine(@"    /  /:/      /  /::\     /  /::|     /  /::\     /  /::|     /  /::\     /  /::|   ");
            Console.WriteLine(@"   /  /:/      /  /:/\:\   /  /:|:|    /  /:/\:\   /  /:|:|    /  /:/\:\   /  /:|:|   ");
            Console.WriteLine(@"  /  /::\ ___ /  /::\ \:\ /  /:/|:|__ /  /:/  \:\ /  /:/|:|__ /  /::\ \:\ /  /:/|:|__ ");
            Console.WriteLine(@" /__/:/\:\  //__/:/\:\_\:/__/:/ |:| //__/:/_\_ \:/__/:/ |::::/__/:/\:\_\:/__/:/ |:| /\");
            Console.WriteLine(@" \__\/  \:\/:\__\/  \:\/:\__\/  |:|/:\  \:\__/\_\\__\/'  ~~/:\__\/  \:\/:\__\/  |:|/:/");
            Console.WriteLine(@"      \__\::/     \__\::/    |  |:/:/ \  \:\ \:\       /  /:/     \__\::/    |  |:/:/ ");
            Console.WriteLine(@"      /  /:/      /  /:/     |_ |::/   \  \:\/:/      /  /:/      /  /:/     |_ |::/  ");
            Console.WriteLine(@"     /__/:/      /__/:/      /__/:/     \  \::/      /__/:/      /__/:/      /__/:/   ");
            Console.WriteLine(@"     \__\/       \__\/       \__\/       \__\/       \__\/       \__\/       \__\/    ");
            Console.WriteLine("");
        }

        static string[][] HangingGraphics(string[][] hangingGraphics)
        {
            // Fills the jagged array hangingGraphics with string arrays for the hanging scene grapphics.
            hangingGraphics[0] = new string[] { @"       ■══════╗", @"       ■══════╗", @"       ■══════╗", @"       ■══════╗", @"       ■══════╗", @"       ■══════╗", @"       ■══════╗", @"       ■══════╗", @"       ■══════╗", @"       ■══════╗", @"       ■══════╗" };
            hangingGraphics[1] = new string[] { @"              ║", @"              ║", @"              ║", @"              ║", @"              ║", @"              ║", @"              ║", @"              ║", @"              ║", @"       )      ║", @"       │      ║" };
            hangingGraphics[2] = new string[] { @"              ║", @"              ║", @"              ║", @"              ║", @"              ║", @"              ║", @"        O     ║", @"      \O/     ║", @"      \█      ║", @"      (█/     ║", @"       │      ║" };
            hangingGraphics[3] = new string[] { @"              ║", @"              ║", @"              ║", @"              ║", @"           O  ║", @"          O   ║", @"       /│\    ║", @"       │      ║", @"       │\     ║", @"      /│      ║", @"       │      ║" };
            hangingGraphics[4] = new string[] { @"              ║", @"              ║", @"             O║", @"           \O ║", @"          /│  ║", @"         /│\  ║", @"       /      ║", @"      / \     ║", @"      / \     ║", @"      / \     ║", @"       █      ║" };
            hangingGraphics[5] = new string[] { @"╔═══+═════+══=╣", @"╔═══+═════+══=╣", @"╔═══+═════+══=╣", @"╔═══+═════+══=╣", @"╔═══+═════+══=╣", @"╔═══+═════+══=╣", @"╔═══+═════+══=╣", @"╔═══+═════+══=╣", @"╔═══+═════+══=╣", @"╔═══+═════+══=╣", @"╔═══+ /│\ +══=╣" };
            hangingGraphics[6] = new string[] { @"║             ║", @"║             ║", @"║             ║", @"║             ║", @"║             ║", @"║             ║", @"║             ║", @"║             ║", @"║             ║", @"║             ║", @"║     / \ ║   ║" };
            hangingGraphics[7] = new string[] { @"║             ║", @" O O O O O O O ", @" O O/O O O O O ", @"║O O O O O O O║", @" O O O O O\O O ", @" O/O O O O O O ", @" O O O O\O O\O ", @" O/O O O O\O O ", @" O O\O O/O O\O ", @" O\O/O\O O/O O ", @"\O/O/O/O\O/O/O/" };
            hangingGraphics[8] = new string[] { @"╚=============╝", @"/│/│/│/│\│\│\│\", @"/│/│/│/│\│\│\│\", @"/│/│/│/│\│\│\│\", @"/│/│/│/│\│\│\│\", @"/│/│/│/│\│\│\│\", @"/│/│/│/│\│\│\│\", @"/│/│/│/│\│\│\│\", @"/│/│/│/│\│\│\│\", @"/│/│/│/│\│\│\│\", @"/│/│/│/│\│\│\│\" };
            hangingGraphics[9] = new string[] { @" # # # # # # # ", @"//\/\/\/\/\\/\\", @"//\/\/\/\/\\/\\", @"//\/\/\/\/\\/\\", @"//\/\/\/\/\\/\\", @"//\/\/\/\/\\/\\", @"//\/\/\/\/\\/\\", @"//\/\/\/\/\\/\\", @"//\/\/\/\/\\/\\", @"//\/\/\/\/\\/\\", @"//\/\/\/\/\\/\\" };
            return hangingGraphics;
        }

        static void GuessLetter(char inputGuess, string secretWord, char[] secretLetters, ref int hiddenLetters, ref int visibleCharacters)
        {
            // Replaceses the hidden letter (_) with the correct letter of the secret word if the player guessed correct.
            int i = 0;
            foreach (var secretWordLetter in secretWord)
            {
                if (inputGuess == secretWordLetter)
                {
                    secretLetters[i] = inputGuess;
                    hiddenLetters--;
                    visibleCharacters++;
                }
                i++;
            }
            Console.Clear();
        }

        static void AllCorrectLetters(string secretWord, char[] secretLetters)
        {
            // Replaceses all the hidden letters (_) with the correct letters of the secret word.
            int i = 0;
            foreach (var secretWordLetter in secretWord)
            {
                secretLetters[i] = secretWordLetter;
                i++;
            }
        }

        static int HideWord(char[] secretLettersStart, string secretWord, ref int letterTally, ref int spaceTally)
        {
            // Hides all english letters. Also tally the count of English letters and the count of spaces in this loop.
            Regex englishLetters = new Regex(@"[a-zA-Z]");
            int i = 0;
            foreach (var secretWordLetter in secretWord)
            {

                if (englishLetters.IsMatch("" + secretWordLetter))
                {
                    secretLettersStart[i] = '_';
                    letterTally++;
                }
                else
                {
                    if (secretWordLetter == ' ')
                    {
                        spaceTally++;
                    }
                    secretLettersStart[i] = secretWordLetter;
                }
                i++;
            }      
            return letterTally;
        }

        static string[] GetWords(ref string theme)
        {
            theme = "Philosophers";
            string[] defaultWordCollection = { "GAUTAMA BUDDHA", "THALES", "ANAXIMANDER", "PYTHAGORAS", "CONFUCIUS", "HERACLITUS", "PARMENIDES", "EMPEDOCLES", "SOCRATES", "DEMOCRITUS", "PLATO", "ARISTOTLE", "DIOGENES", "LAO-TZU"
            , "PYRRHON OF ELIS", "EPICURUS", "ANTISTHENES", "SUN TZU", "XENOPHANES", "HYPATIA", "MARCUS AURELIUS", "YAQUB IBN ISHAQ AS-SABAH AL-KINDI", "AVICENNA", "ABU NASR MUHAMMAD AL-FARABI", "SAINT THOMAS AQUINAS"
            , "WILLIAM OF OCKHAM", "NICCOLO MACHIAVELLI", "FRANCIS BACON", "RENÉ DESCARTES", "JOHN LOCKE", "BENEDICT DE SPINOZA", "VOLTAIRE", "DAVID HUME","JEAN-JACQUES ROUSSEAU", "IMMANUEL KANT", "JOHN STUART MILL"
            , "GEORG WILHELM FRIEDRICH HEGEL", "KARL MARX", "SØREN KIERKEGAARD", "FRIEDRICH NIETZSCHE", "MIKHAIL BAKUNIN", "JOHN DEWEY", "BERTRAND RUSSEL", "AYN RAND", "MICHEL FOUCAULT", "SIMONE DE BEAUVOIR", "NOAM CHOMSKY"};
            
            // Checks if there is a words.txt file.
            if (System.IO.File.Exists(@"words.txt"))
            {
                // Uploads words.txt but only continue to process the file if there is more than one line of text in it.
                string[] rawUploadedWordCollection = System.IO.File.ReadAllLines(@"words.txt");
                if (rawUploadedWordCollection != null && rawUploadedWordCollection.Length > 1)
                {
                    // Uses up to the first 63 characters in the first line to set the theme.
                    if (rawUploadedWordCollection[0].Length < 64)
                    {
                        theme = rawUploadedWordCollection[0];
                    }
                    else
                    {
                        theme = rawUploadedWordCollection[0].Substring(0, 64);
                    }

                    // Skips the first line in (that is reserved for the theme) in the loop. Then changes all letters to upper case and cut down any string to its first 99 characters if needed.
                    string[] uploadedWordCollection = new string[rawUploadedWordCollection.Length - 1];
                    int i = 0;
                    foreach (var word in rawUploadedWordCollection.Skip(1))
                    {
                        if (word.Length < 100)
                        {
                            uploadedWordCollection[i] = word.ToUpper();
                        }
                        else
                        {
                            uploadedWordCollection[i] = word.Substring(0, 99).ToUpper();
                        }
                        i++;
                    }
                    Console.WriteLine("");
                    Console.WriteLine(" A selection of names have been uploaded from a words.txt file.");
                    Console.WriteLine(" Theme: " + theme + ".");
                    Console.WriteLine("");
                    return uploadedWordCollection;
                }
            }
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(" There's no words.txt file (at least not with proper structured content) in the folder.");
            Console.WriteLine(" The theme should be described on line one and then use a line per each secret wording.");
            Console.WriteLine("");
            Console.WriteLine(" Playing with the default option. Theme: " + theme + ".");
            Console.WriteLine("");
            return defaultWordCollection;
        }
    }
}