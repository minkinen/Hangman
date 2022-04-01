using System.Text;
using System.IO;

namespace Hangman
{
    class Program
    {
        static void Main()
        {
                // Words in an array of strings
                string[] words = GetWords();
                bool runGame = true;
                while (runGame)
                {
                    runGame = GameDisplay(words);
                }
        }

        private static bool GameDisplay(string[] words)
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

                // The secret word randomly chosen from an array of Strings
                Random randomNumber = new Random();
                int indexNumber = randomNumber.Next(0, words.Length);
                string hiddenWord = words[indexNumber];

                // The correct letters inside a char array. 
                int hiddenWordLength = words[indexNumber].Length;
                char[] hiddenLetters = new char[hiddenWordLength];

                // Unrevealed letters represented by a lower dash(_).
                HideWord(hiddenLetters, hiddenWord);

                // The incorrect letters the player has guessed, should be put inside a StringBuilder and presented to the player after each guess
                StringBuilder flunkedLetters = new StringBuilder();

                // The player has 10 guesses to complete the word before losing the game.
                for (int usedGuesses = 0; usedGuesses < 10; usedGuesses++)
                {
                    var winCheck = new string(hiddenLetters);
                    if (winCheck == hiddenWord)
                    {
                        CorrectWordText(hiddenWord, hiddenLetters);
                        return true;
                    }
                    GameText(hiddenWord, hiddenLetters, flunkedLetters, usedGuesses);

                    // The player can make two type of guesses; letter or word.
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
                                    usedGuesses--;
                                }
                                else
                                {
                                    flunkedLetters.Append(inputGuess);
                                }
                            }
                            // If the player guesses the same letter twice, the program will not consume a guess.
                            else
                            {
                                usedGuesses--;
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
                                    AllCorrectLetters(hiddenWord, hiddenLetters);
                                    usedGuesses--;
                                }
                            }
                            else
                            {
                                IncorrectLengthText(hiddenWord);
                                usedGuesses--;
                            }
                        }
                    }
                    else
                    {
                        usedGuesses--;
                    }
                }
                GameOverText();
                return true;
            }
            else
            {
                return true;
            }
        }

        static void GameText(string hiddenWord, char[] hiddenLetters, StringBuilder flunkedLetters, int usedGuesses)
        {
            Console.WriteLine("");
            Console.WriteLine(" Used guesses: " + usedGuesses + " out of 10.");
            Console.WriteLine("");
            Console.WriteLine(" Struck out letters: " + flunkedLetters);
            Console.WriteLine("");
            Console.Write(" You can guess a letter or the hidden word: ");
            Console.Write(hiddenLetters);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("                                            ");
        }

        static void IncorrectLengthText(string hiddenWord)
        {
            Console.WriteLine("");
            Console.WriteLine(" Need a " + hiddenWord.Length + " letter word. Try again.");
            Console.WriteLine("");
        }

        static void GameOverText()
        {
            Console.WriteLine("");
            Console.WriteLine(" You have no more guesses. Game over! ");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        static void CorrectWordText(string hiddenWord, char[] hiddenLetters)
        {
            Console.WriteLine("");
            Console.WriteLine(" Winner! You got the complete word:         " + hiddenWord);
            Console.WriteLine("");
            Console.WriteLine("");
        }

        static void PlayOrQuitText()
        {
            Console.WriteLine("");
            Console.WriteLine(" Type P and press Enter to Play.");
            Console.WriteLine(" Type Q and press Enter to Quit.");
            Console.WriteLine("");
            Console.Write(" ");
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

        static void AllCorrectLetters(string hiddenWord, char[] hiddenLetters)
        {
            int i = 0;
            foreach (var hiddenWordLetter in hiddenWord)
            {
                i++;
                hiddenLetters[i - 1] = hiddenWordLetter;
            }
        }

        static void HideWord(char[] hiddenLettersStart, string hiddenWord)
        {
            int i = 0;
            foreach (var hiddenWordLetter in hiddenWord)
            {
                
                if ((hiddenWordLetter == ' ') || ('-' == hiddenWordLetter) || ('.' == hiddenWordLetter) || ('\'' == hiddenWordLetter))
                {
                    
                    hiddenLettersStart[i] = hiddenWordLetter;
                }
                else
                {
                    hiddenLettersStart[i] = '_';
                }
                i++;
            }
        }

        static string[] GetWords()
        {
            string[] weekDays = { "GAUTAMA BUDDHA", "THALES", "ANAXIMANDER", "PYTHAGORAS", "CONFUCIUS", "HERACLITUS", "PARMENIDES", "EMPEDOCLES", "SOCRATES", "DEMOCRITUS", "PLATO", "ARISTOTLE", "DIOGENES"
            , "PYRRHON OF ELIS", "EPICURUS", "ANTISTHENES", "SUN TZU", "XENOFANES", "MARCUS AURELIUS", "YAQUB IBN ISHAQ AS-SABAH AL-KINDI", "SAINT THOMAS AQUINAS", "WILLIAM OF OCKHAM", "NICCOLO MACHIAVELLI"
            , "FRANCIS BACON", "RENÉ DESCARTES", "JOHN LOCKE", "BENEDICT DE SPINOZA", "VOLTAIRE", "JEAN-JACQUES ROUSSEAU", "IMMANUEL KANT", "GEORG WILHELM FRIEDRICH HEGEL", "KARL MARX", "FRIEDRICH NIETZSCHE"
            , "MIKHAIL BAKUNIN", "BERTRAND RUSSEL", "NOAM CHOMSKY"};
            if (System.IO.File.Exists(@"words.txt"))
            {    
                string[] uploadedWordCollection = System.IO.File.ReadAllLines(@"words.txt");
                if (uploadedWordCollection != null && uploadedWordCollection.Length != 0)
                {
                    uploadedWordCollection = Array.ConvertAll(uploadedWordCollection, d => d.ToUpper());
                    Console.WriteLine("");
                    Console.WriteLine(" A selection of famous fictinonal characters have been uploaded from a words.txt file.\n");
                    Console.WriteLine(" Good luck!");
                    Console.WriteLine("");
                    return uploadedWordCollection;
                }
            }
            Console.WriteLine("");
            Console.WriteLine(" There is no words.txt file with any text.\n");
            Console.WriteLine(" Using the default option instead. With the theme philosophers.");
            Console.WriteLine("");
            return weekDays;
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

// (X) (optional) You unit tests need to have at least 50% coverage.
// (X) (optional) Read in the words from a text file with Comma-separated values and then store them in an array or list of Strings.
