using System;

namespace Hangman
{
    class Program
    {
        static void Main()
        {
            // Words in an array of strings
            string[] words = { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY" };

            // The secret word randomly chosen from an array of Strings
            Random randomNumber = new Random();
            int indexNumber = randomNumber.Next(0, words.Length);
            string hiddenWord = words[indexNumber];





            // Display
            Console.WriteLine(hiddenWord);
            Console.ReadLine();


        }
    }
}


// (1) Words in an array of strings
// (2) The secret word should be randomly chosen from an array of Strings
// The player has 10 guesses to complete the word before losing the game. The player can make two type of guesses; letter or word.
// Guess for a specific letter. If player guess a letter that occurs in the word, the program should update by inserting the letter in the correct position(s).
// Guess for the whole word. The player type in a word he/she thinks is the word. If the guess is correct player wins the game and the whole word is revealed. If the word is incorrect nothing should get reveale
// If the player guesses the same letter twice, the program will not consume a guess.
// The incorrect letters the player has guessed, should be put inside a StringBuilder and presented to the player after each guess
// The correct letters should be put inside a char array. Unrevealed letters need to be represented by a lower dash(_).

// (optional) You unit tests need to have at least 50% coverage.
// (optional) Read in the words from a text file with Comma-separated values and then store them in an array or list of Strings.

