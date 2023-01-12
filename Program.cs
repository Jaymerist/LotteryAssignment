/*
 * Purpose: Design and write a modularized menu-driven program that allows the user to play Lotto MAX with EXTRA or to play Lotto 6/49 with EXTRA.
 * Input: menuNumber lottoMaxNumbers, lotto649Numbers
 * Output: lotto649Matches, lottoMaxMatches, lottoExtraMatches, lottoExtraPrize, lotto649Prize, lottoMaxPrize
 * Author: Mihiri Kamiss
 * Section: A02
 * Instructor: Allan Anderson
 * Last Modified: November 23, 2022
 */

namespace LottoAssignment3
{
    internal class Program
    {
        const int LottoMAXSize = 8,
          Lotto649Size = 7,
          LottoExtraSize = 7,
          LottoMAXMax = 50,
          Lotto649Max = 49;

        /*---------Menu--------------*/
        static void Main(string[] args)
        {
            //declare variables
            bool continueDisplay = true;
            int menuOption;
            int[] lottoMaxNumbers = new int[LottoMAXSize],
                lotto649Numbers = new int[Lotto649Size],
                lottoExtraNumbers = new int[LottoExtraSize];

            //initial lottery numbers
            lottoMaxNumbers = GenerateNumberSet(lottoMaxNumbers, LottoMAXMax, LottoMAXSize);
            lotto649Numbers = GenerateNumberSet(lotto649Numbers, Lotto649Max, Lotto649Size);
            lottoExtraNumbers = GenerateExtraNumber();

            //display menu until user chooses to exit program 
            do
            {
                //display menu 
                MenuDisplay();

                //prompt for option
                menuOption = GetSafeInt("Enter your menu number option: ", 5);

                //switch statement: call each lotery method spending on the option chosen.
                switch (menuOption)
                {
                    case 0:
                        continueDisplay = false;
                        Console.WriteLine("Goodbye, see you again some day...");
                        break;

                    case 1:
                        lottoMaxNumbers = ChangeLotteyNumbers(lottoMaxNumbers, LottoMAXSize, LottoMAXMax, "Lotto MAX");
                        break;

                    case 2:
                        lotto649Numbers = ChangeLotteyNumbers(lotto649Numbers, Lotto649Size, Lotto649Max, "Lotto 6/49");
                        break;

                    case 3:
                        lottoExtraNumbers = ChangeExtraNumber(lottoExtraNumbers);
                        break;

                    case 4:
                        PlayLotto(lottoMaxNumbers, lottoExtraNumbers, LottoMAXSize, LottoMAXMax, "Lotto MAX");
                        break;

                    case 5:
                        PlayLotto(lotto649Numbers, lottoExtraNumbers, Lotto649Size, Lotto649Max, "Lotto 6/49");
                        break;

                    default:
                        Console.WriteLine("Selected menu option is invalid...please try again\n");
                        break;
                }
            } while (continueDisplay != false);

            Console.ReadLine();

        }//end of Main

        static void MenuDisplay()
        {
            Console.WriteLine("|------------------------------------------------|");
            Console.WriteLine("| CPSC1012 Lotto Centre                          |");
            Console.WriteLine("|------------------------------------------------|");
            Console.WriteLine("| 1. Change Lotto MAX winning numbers            |");
            Console.WriteLine("| 2. Change Lotto 6/49 winning numbers           |");
            Console.WriteLine("| 3. Change Lotto EXTRA winning numbers          |");
            Console.WriteLine("| 4. Play Lotto MAX                              |");
            Console.WriteLine("| 5. Play Lotto 6/49                             |");
            Console.WriteLine("| 0. Exit Program                                |");
            Console.WriteLine("|------------------------------------------------|");
        }//end of MenuDisplay

        /*---------CREATE ARRAYS---------*/
        static int[] GenerateNumberSet(int[] lotteryNumbers, int max, int size)
        {
            //random number generator
            Random random = new Random();
            int randomNum;
            bool uniqueNumber;


            for (int number = 0; number < size; number++)
            {
                do
                {
                    //check if random number generated is unique using SearchArray
                    randomNum = random.Next(1, max);
                    if (SearchArray(lotteryNumbers, size, randomNum) > -1)
                    {
                        //add random number to array if unique, continue for loop
                        uniqueNumber = true;
                        lotteryNumbers[number] = randomNum;
                    }
                    else
                    {
                        uniqueNumber = false;
                    }
                } while (uniqueNumber == false);
            }
            return lotteryNumbers;
        }//end of GenerateArray

        //-------------CHANGE LOTTERY NUMBERS------------

        static int[] ChangeLotteyNumbers(int[] lotteryNumbers, int size, int max, string lotteryName)
        {
            char option;
            //Display current numbers
            Console.WriteLine($"The current winning {lotteryName} numbers are: ");
            DisplayLottoNumbers(lotteryNumbers, size);

            //prompt to generate or manually input numbers
            option = GetSafeOption("\nWould you like to generate or enter the winning numbers (g/e): ");

            //prompt to generate or manually input numbers
            if (option == 'g')
            {
                GenerateNumberSet(lotteryNumbers, max, size);
            }
            else
            {
                lotteryNumbers = ChooseNumbers(size, max);
            }

            //Display the array
            Console.WriteLine($"The new winning {lotteryName} numbers are: ");
            DisplayLottoNumbers(lotteryNumbers, size);

            Console.WriteLine("\n");

            return lotteryNumbers;
        }//end of ChangeLotteryNumbers

        static int[] ChooseNumbers(int size, int max)
        {
            int[] lotteryNumbers = new int[size];

            for (int index = 0; index < (size - 1); index++)
            {
                lotteryNumbers[index] = GetUniqueInt($"Enter number {index + 1}: ", lotteryNumbers, size, max);
            }

            //input bonus number

            lotteryNumbers[size - 1] = GetUniqueInt($"Enter bonus number: ", lotteryNumbers, size, max);

            return lotteryNumbers;
        }//ChooseNumbers

        //------------Lotto EXTRA Generation-----------
        static int[] ChangeExtraNumber(int[] lottoExtraNumbers)
        {
            Console.Write("\nThe current Lotto EXTRA number is: ");
            for (int index = 0; index < 7; index++)
            {
                Console.Write($"{lottoExtraNumbers[index]}");
            }

            lottoExtraNumbers = GenerateExtraNumber();

            Console.Write("\nThe new Lotto EXTRA number is: ");
            for (int index = 0; index < 7; index++)
            {
                Console.Write($"{lottoExtraNumbers[index]}");
            }

            Console.WriteLine("\n");
            return lottoExtraNumbers;
        }//end of ChangeExtraNumber

        static int[] GenerateExtraNumber()
        {
            Random random = new Random();
            int[] number = new int[LottoExtraSize];

            for (int index = 0; index < LottoExtraSize; index++)
            {
                number[index] = random.Next(1, 9);
            }

            return number;

        }//end of GenerateExtraNumber

        //------------PLAY LOTTO GAMES-----------------

        static void PlayLotto(int[] winningNumbers, int[] winningExtra, int size, int max, string lottoName)
        {
            //declare variables
            int[] playerNumbers = new int[size - 1],
                playerExtra = new int[LottoExtraSize];
            int matches,
                extraMatches;
            bool bonusMatch = false;

            //create player numbers
            playerNumbers = GenerateNumberSet(playerNumbers, max, (size - 1));
            playerExtra = GenerateExtraNumber();

            //display winning numbers
            Console.Write($"The current {lottoName} winning numbers are: ");
            DisplayLottoNumbers(winningNumbers, size);
            Console.Write($" (Bonus: {winningExtra[size - 2]})");
            Console.Write($"\nThe current Lotto EXTRA winning number is: ");
            DisplayExtraNumber(winningExtra);
            Console.WriteLine("\n");

            //display player numbers
            Console.Write($"Your {lottoName} quick pick numbers are: ");
            DisplayLottoNumbers(playerNumbers, size);
            Console.Write($"\nYour current Lotto EXTRA number is: ");
            DisplayExtraNumber(playerExtra);
            Console.WriteLine("\n");

            //find matches
            matches = FindMatches(playerNumbers, winningNumbers, size);
            bonusMatch = FindBonusMatch(playerNumbers, winningNumbers, size, matches);
            extraMatches = LottoExtraMatches(winningExtra, playerExtra);

            //display results
            //call each prize pool based on which lotto is currently playing
            Console.WriteLine($"\nYour Lotto MAX match: {matches}/{size - 1}");
            if (size == LottoMAXSize)
            {
                Console.WriteLine($"Your Lotto Max prize: {LottoMaxPrizes(matches, bonusMatch)}");
            }
            else
            {
                Console.WriteLine($"Your Lotto 6/49 prize: {Lotto649Prizes(matches, bonusMatch)}");
            }
            Console.WriteLine($"Your Lotto EXTRA match: Last {extraMatches} numbers");
            Console.WriteLine($"Your Lotto EXTRA prize: {LottoExtraPrizes(extraMatches)}");


        }//end of PlayLotto

        //----------COMPARE STRING METHODS-------------
        static int FindMatches(int[] playerNumbers, int[] winningNumbers, int size)
        {
            int matches = 0,
                cycle = 0;
            bool scanComplete = false;

            //compare each number in winningNumbers to one playerNumber at a time

            do
            {
                for (int index = 0; index < (size - 1); index++)
                {
                    if (winningNumbers[index] == playerNumbers[cycle])
                    {
                        matches++;
                        index = size;
                    }
                }

                //move to next player number to compare to
                cycle++;

                //if all numbers have been compared, exit do while loop
                if (cycle == size - 1)
                {
                    scanComplete = true;
                }

            } while (scanComplete == false);

            return matches;
        }//end of FindMatches
        static bool FindBonusMatch(int[] playerNumbers, int[] winningNumbers, int size, int matches)
        {
            int cycle = 0,
                matchesWithBonus = 0;
            bool scanComplete = false,
                bonusMatch = false;

            //compare each number in winningNumbers to one playerNumber at a time
            do
            {
                for (int index = 0; index < size; index++)
                {
                    if (winningNumbers[index] == playerNumbers[cycle])
                    {
                        matchesWithBonus++;
                        index = size;
                    }
                }

                //move to next player number to compare to
                cycle++;
                //if matches with bonus > matches, exit do while loop
                if (matchesWithBonus > matches)
                {
                    scanComplete = true;
                    bonusMatch = true;
                }

                //if all numbers have been compared, exit do while loop
                if (cycle == (size - 1))
                {
                    scanComplete = true;
                }

            } while (scanComplete == false);

            return bonusMatch;
        }
        static int LottoExtraMatches(int[] winningExtra, int[] playerExtra)
        {
            int matches = 0,
                cycle = LottoExtraSize - 1;

            //compare each digit in winningExtra to one playerExtra digit at a time
            for (int index = 6; index > -1; index--)
            {
                if (winningExtra[index] == playerExtra[cycle])
                {
                    matches++;
                    cycle--;
                }
                else
                {
                    //if the numbers do not match at any point, the for loop will close
                    index = -1;
                }
            }
            return matches;
        }//end of LottoExtraMatches

        //---------------PRIZE POOLS----------------

        //# of matches (and bonus match true/false if applicable) passes in to the appropriate lotto prize statements and returns prize string
        static string LottoExtraPrizes(int matches)
        {
            string prize;

            switch (matches)
            {
                case 7:
                    prize = "$250,000";
                    break;
                case 6:
                    prize = "$100,000";
                    break;
                case 5:
                    prize = "$1,000";
                    break;
                case 4:
                    prize = "$100";
                    break;
                case 3:
                    prize = "$50";
                    break;
                case 2:
                    prize = "$10";
                    break;
                case 1:
                    prize = "$2";
                    break;
                default:
                    prize = "$0";
                    break;
            }

            return prize;
        }//end of LottoExtraPrizes
        static string LottoMaxPrizes(int matches, bool bonusMatch)
        {
            string prize;

            if (matches == 7)
            {
                prize = "Win or share Jackpot of at least $10 Million or 87.25% of Pools Fund";
            }
            else if (matches == 6 && bonusMatch == true)
            {
                prize = "Share of 2.5% of Pools Fund";
            }
            else if (matches == 6)
            {
                prize = "Share of 2.5% of Pools Fund";
            }
            else if (matches == 5 && bonusMatch == true)
            {
                prize = "Share of 1.5% of Pools Fund";
            }
            else if (matches == 5)
            {
                prize = "Share of 3.5% of Pools Fund";
            }
            else if (matches == 4 && bonusMatch == true)
            {
                prize = "Share of 2.75% of Pools Fund";
            }
            else if (matches == 4)
            {
                prize = "$20";
            }
            else if (matches == 3 && bonusMatch == true)
            {
                prize = "$20";
            }
            else if (matches == 3)
            {
                prize = "Free Play";
            }
            else
            {
                prize = "$0";
            }

            return prize;
        }//end of LottoMaxPrizes
        static string Lotto649Prizes(int matches, bool bonusMatch)
        {
            string prize;

            if (matches == 6)
            {
                prize = "Win or share Jackpot (79.5% of the Pools Fund)";
            }
            else if (matches == 5 && bonusMatch == true)
            {
                prize = "Share of 6% of the Pools Fund";
            }
            else if (matches == 5)
            {
                prize = "Share of 5% of the Pools Fund";
            }
            else if (matches == 4)
            {
                prize = "Share of 9.5% of the Pools Fund";
            }
            else if (matches == 3)
            {
                prize = "$10";
            }
            else if (matches == 2 && bonusMatch == true)
            {
                prize = "$5";
            }
            else if (matches == 2)
            {
                prize = "Free Play";
            }
            else
            {
                prize = "$0";
            }

            return prize;
        }//end of Lotto649Prizes

        //---------VALIDATION/DISPLAY METHODS-----------
        static int GetSafeInt(string prompt, int max)
        {
            bool isValid = false;
            int number;
            do
            {
                try
                {
                    Console.Write(prompt);
                    number = int.Parse(Console.ReadLine());
                    if (number > max)
                    {
                        prompt = $"Please enter a number no larger than {max}: ";
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input ... please try again.");
                    number = 0;
                }
            } while (!isValid);
            return number;
        }//end of GetSafeInt

        static char GetSafeOption(string prompt)
        {
            bool isValid = false;
            char option;
            do
            {
                try
                {
                    Console.Write(prompt);
                    option = char.Parse(Console.ReadLine().ToLower());
                    if (option != 'g' && option != 'e')
                    {
                        Console.WriteLine("Enter a valid selection (g/e)");
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input ... please try again.");
                    option = 'n';
                }
            } while (!isValid);

            return option;
        }//end of GetSafeOption

        static int GetUniqueInt(string prompt, int[] lotteryNumbers, int size, int max)
        {
            int location,
                number;
            bool addComplete;

            do
            {
                addComplete = false;

                number = GetSafeInt(prompt, max);
                //only add number to array if number does not already exist. 
                location = SearchArray(lotteryNumbers, size, number);

                if (location > -1 && number > 0)
                {
                    addComplete = true;
                }
                else
                {
                    prompt = "Please enter a unique, positive number: ";
                }
            } while (addComplete == false);


            return number;

        }//end of GetSafeInt

        static int SearchArray(int[] lotteryNumbers, int size, int numberSearch)
        {

            int location = -1;
            for (int index = 0; index < size; index++)
            {
                //for loop ends once all numbers have been checked or matching number is found
                if (lotteryNumbers[index] != numberSearch)
                {
                    location = index;
                }
                else
                {
                    //if number does exist, return -1
                    location = -1;
                    index = size;
                }
            }

            return location;
        }//end of SearchArray

        static void DisplayLottoNumbers(int[] lotteryNumbers, int size)
        {
            //loop through each number in array and print
            for (int index = 0; index < (size - 1); index++)
            {
                Console.Write($"{lotteryNumbers[index]}");

                //if the number is not last, seperate with a comma
                if (index != (size - 2))
                {
                    Console.Write(", ");
                }
            }
        }//end of DisplayArray
        static void DisplayExtraNumber(int[] lotteryNumber)
        {
            //loop through each number in array and print
            for (int index = 0; index < (LottoExtraSize); index++)
            {
                Console.Write($"{lotteryNumber[index]}");
            }
        }
    }
}
