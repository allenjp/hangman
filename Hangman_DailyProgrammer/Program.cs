using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hangman_DailyProgrammer
{
    class Program
    {
        // Guess function
        // This function takes a letter and a word and returns a list of indexes representing
        // the positions of that letter in the word
        static List<int> Guess(char guess, string word)
        {
            List<int> letterIndexes = new List<int>();
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == guess)
                {
                    letterIndexes.Add(i);
                }
            }
            return letterIndexes;
        }

        static void Main(string[] args)
        {
            // Initialize an array of words for the program to choose from
            List<string> wordList = new List<string>();

            // Read the wordlist.txt file
            string line;
            try
            {
                using (StreamReader sr = new StreamReader("../../../wordlist.txt"))
                {
                    while((line = sr.ReadLine()) != null)
                    {
                        wordList.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            
            // Choose a random word to play the game with
            Random rnd = new Random();
            int rndWordIndex = rnd.Next(1, wordList.Count);
            string randomWord = wordList[rndWordIndex];

            // Remove special characters like apostrophes from the word
            randomWord = randomWord.Replace("'", "");

            // Initialize the gameboard with blank spaces
            for (int i = 0; i < randomWord.Length; i++)
            {
                Console.Write("_ ");
            }

            // Declare a bool indicating if the word has been solved
            bool solved = false;

            // Declare a bool indicating if the user is out of guesses
            bool dead = false;

            // Declare an int indicating the number of guesses the user has left
            int guessesLeft = 10;

            // Initialize the list of correct indexes
            List<int> correctGuesses = new List<int>();

            // Initialize the list of letters already guessed by user
            List<char> alreadyGuessed = new List<char>();

            while ((!solved) && (!dead))
            {
                // See if the user's guess is in the random word
                Console.WriteLine();
                Console.Write("Guess a letter: ");
                string uGuess = Console.ReadLine();

                // Make sure the user actually entered a letter and that only one letter was entered
                if (uGuess.Length != 1)
                {
                    Console.WriteLine("Please make sure you have entered exactly one letter");
                    Console.WriteLine();
                    Console.Write("Guess a letter: ");
                    uGuess = Console.ReadLine();
                }

                if (!Regex.IsMatch(uGuess, @"^[\p{L}]+$"))
                {
                    Console.WriteLine("Only alpha characters");
                    Console.WriteLine();
                    Console.Write("Guess a letter: ");
                    uGuess = Console.ReadLine();
                }


                char uGuessChar = uGuess[0];

                // Make sure letter hasn't already been guessed by user:
                if (alreadyGuessed.Contains(uGuessChar))
                {
                    Console.WriteLine(uGuessChar + " has already been guessed.  Pick a new letter.");

                    // Display the user's wrong guesses of the round
                    Console.Write("Guessed letters: ");

                    // Sort the already guessed array so it can be displayed alphabetically
                    alreadyGuessed.Sort();
                    for (int j = 0; j < alreadyGuessed.Count; j++)
                    {
                        Console.Write(alreadyGuessed[j] + " ");
                    }
                    Console.WriteLine();
                    Console.Write("Guess a letter: ");
                    uGuess = Console.ReadLine();
                }

                else
                {
                    // Add the guess to the alreadyGuessed list
                    alreadyGuessed.Add(uGuessChar);

                    // Establish the list of indexes that the user's guess matches in the game word
                    List<int> currGuessInd = Guess(uGuessChar, randomWord);

                    // If the user's guess is not in the word:
                    if (currGuessInd.Count == 0)
                    {
                        Console.WriteLine("Not in word.");

                        // Display the user's wrong guesses of the round
                        Console.Write("Guessed letters: ");

                        // Sort the already guessed array so it can be displayed alphabetically
                        alreadyGuessed.Sort();
                        for (int j = 0; j < alreadyGuessed.Count; j++)
                        {
                            Console.Write(alreadyGuessed[j] + " ");
                        }
                        Console.WriteLine();

                        guessesLeft--;

                        if (guessesLeft > 0)
                        {
                            Console.WriteLine("You have " + guessesLeft + " guesses left.");
                            Console.WriteLine();
                        }

                        else
                        {
                            Console.WriteLine("Out of guesses!  Game over!");
                            Console.WriteLine();
                            Console.WriteLine("The word was: ");
                            Console.WriteLine(randomWord);
                            dead = true;
                        }
                    }

                    // If the user's guess is in the word:
                    else
                    {
                        // Add the correct indexes from the current guess to the master list of correct guesses
                        for (int i = 0; i < currGuessInd.Count; i++)
                        {
                            correctGuesses.Add(currGuessInd[i]);

                            // If the number of correct guesses equals the length of the word
                            // The puzzle is solved
                            if (correctGuesses.Count == randomWord.Length)
                            {
                                Console.WriteLine("You solved the puzzle!");
                                solved = true;
                            }
                        }

                        // Display the word
                        // If the letter has not yet been guessed, diplay "_" in place of the letter
                        // If the letter has been guessed, display the letter
                        for (int i = 0; i < randomWord.Length; i++)
                        {
                            if (correctGuesses.Contains(i))
                            {
                                Console.Write(randomWord[i] + " ");
                            }
                            else
                            {
                                Console.Write("_ ");
                            }
                        }
                        Console.WriteLine();
                    }
                }                
            }
            Console.WriteLine("Play again? (Y / N)");
            string response = Console.ReadLine();
            if (response == "Y")
            {
                Console.Clear();

            }
            Console.ReadKey();
        }
    }
}
