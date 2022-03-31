using System.Text;

namespace Hangman
{
    class Program
    {
        static void Main()
        {
            bool runGame = true;
            while (runGame)
            {
                runGame = MainMenu();
            }
        }

        private static bool MainMenu()
        {
            Console.WriteLine("Type P and press Enter to play.");
            Console.WriteLine("Type Q and press Enter to quit.");
            char playOrQuit = char.Parse(Console.ReadLine().ToUpper());
            if (playOrQuit == 'P')
            {
                // Words in an array of strings
                string[] words = { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY" };

                // The secret word randomly chosen from an array of Strings
                Random randomNumber = new Random();
                int indexNumber = randomNumber.Next(0, words.Length);
                string hiddenWord = words[indexNumber];

                // The correct letters inside a char array. 
                int hiddenWordLength = words[indexNumber].Length;
                char[] hiddenLetters = new char[hiddenWordLength];

                // Unrevealed letters represented by a lower dash(_).
                StartCondition(hiddenLetters);

                // The incorrect letters the player has guessed, should be put inside a StringBuilder and presented to the player after each guess
                StringBuilder flunkedLetters = new StringBuilder();

                // The player has 10 guesses to complete the word before losing the game.
                for (int usedGuesses = 0; usedGuesses < 10; usedGuesses++)
                {
                    var winCheck = new string(hiddenLetters);
                    if (winCheck == hiddenWord)
                    {
                        CorrectWordGuess(hiddenWord, hiddenLetters);
                        return true;
                    }
                    string turnStatus = "ongoing";
                    while (turnStatus == "ongoing")
                    {
                        turnStatus = GameDisplay(hiddenWord, hiddenLetters, flunkedLetters, usedGuesses);
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("You have no more guesses. Game over! ");
                Console.WriteLine(""); 
                Console.WriteLine("");
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string GameDisplay(string hiddenWord, char[] hiddenLetters, StringBuilder flunkedLetters, int usedGuesses)
        {
            Console.WriteLine(hiddenWord);
            Console.WriteLine("");
            Console.WriteLine("Used guesses: " + usedGuesses + " out of 10.");
            Console.WriteLine("");
            Console.WriteLine("Incorrect letters: " + flunkedLetters);
            Console.WriteLine("");

            // The player can make two type of guesses; letter or word.
            Console.Write("You can guess a letter or the hidden word: ");
            Console.Write(hiddenLetters);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("                                           ");
            string inputGuess = (Console.ReadLine().ToUpper());
            int inputLength = inputGuess.Length;
            if (inputLength != 0)
            {
                // Guesses a specific letter.
                if (inputLength == 1)
                {
                    var usedLetterCheck = new string(hiddenLetters) + flunkedLetters;
                    if (!usedLetterCheck.Contains(inputGuess))
                    {
                        if (hiddenWord.Contains(inputGuess))
                        {
                            // If player guess a letter that occurs in the word, the program update by inserting the letter in the correct position(s).
                            GuessLetter(char.Parse(inputGuess), hiddenWord, hiddenLetters);
                            return "completed";
                        }
                        else
                        {
                            flunkedLetters.Append(inputGuess);
                            return "completed";
                        }
                    }
                    // If the player guesses the same letter twice, the program will not consume a guess.
                    else
                    {
                        return "ongoing";
                    }
                }
                else
                {
                    if (inputLength == hiddenWord.Length)
                    {
                        // Guess for the whole word. If the guess is correct player wins the game and the whole word is revealed. If the word is incorrect nothing should get revealed.
                        if (inputGuess == hiddenWord)
                        {
                            // Fill hidden letters with the correct hidden word when player guess the right word.
                            int i = 0;
                            foreach (var hiddenWordLetter in hiddenWord)
                            {
                                i++;
                                hiddenLetters[i - 1] = hiddenWordLetter;
                            }
                            return "completed";
                        }
                        else
                        {
                            return "completed";
                        }
                    }
                    else
                    {
                        Console.WriteLine("");
                        Console.WriteLine("It is a " + hiddenWord.Length + " letter word. Try again.");
                        Console.WriteLine("");
                        return "ongoing";
                    }
                }
            }
            else
            {
                return "ongoing";
            }
        }

        static void CorrectWordGuess(string hiddenWord, char[] hiddenLetters)
        {
            Console.WriteLine("");
            Console.WriteLine("You got the complete word!                 " + hiddenWord);
            Console.WriteLine("");
            Console.WriteLine("");
        }

        static void GuessLetter(char inputGuess, string hiddenWord, char[] hiddenLetters)
        {
            int i = 0;
            foreach (var hiddenWordLetter in hiddenWord)
            {
                i++;
                if (inputGuess == hiddenWordLetter)
                    hiddenLetters[i - 1] = inputGuess;
            }
        }

        static void CorrectLetters(char inputGuess, string hiddenWord, char[] hiddenLetters)
        {
            int i = 0;
            foreach (var hiddenWordLetter in hiddenWord)
            {
                i++;
                hiddenLetters[i - 1] = hiddenWordLetter;
            }
        }

        static void StartCondition(char[] hiddenLettersStart)
        {
            for (int i = 0; i < hiddenLettersStart.Length; i++)
            {
                hiddenLettersStart[i] = '_';
            }
        }
    }
}


// (1) Words in an array of strings
// (2) The secret word should be randomly chosen from an array of Strings
// (9) The player has 10 guesses to complete the word before losing the game.
// (5) The player can make two type of guesses; letter or word.
// (4) Guess for a specific letter. If player guess a letter that occurs in the word, the program should update by inserting the letter in the correct position(s).
// (6) Guess for the whole word. The player type in a word he/she thinks is the word. If the guess is correct player wins the game and the whole word is revealed. If the word is incorrect nothing should get revealed.
// (X) If the player guesses the same letter twice, the program will not consume a guess.
// (7) The incorrect letters the player has guessed, should be put inside a StringBuilder and presented to the player after each guess
// (3) The correct letters should be put inside a char array. Unrevealed letters need to be represented by a lower dash(_).

// (optional) You unit tests need to have at least 50% coverage.
// (optional) Read in the words from a text file with Comma-separated values and then store them in an array or list of Strings.
