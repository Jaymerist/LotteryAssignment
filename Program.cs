using System.Drawing;
using System.Text.RegularExpressions;

namespace Assignment3_Lottery
{
    internal class Program
    {
        const int LottoMAXSize = 7;
        const int Lotto649Size = 6;
        const int LottoMAXMax = 50;
        const int Lotto649Max = 49;

        //-----------------------MENU-------------------------
        static void Main(string[] args)
        {
            //declare variables
            bool continueDisplay = true;
            int menuOption,
                ExtraMax,
                Extra649;
            int[] lottoMaxNumbers = new int[7],
                lotto649Numbers = new int[6],
                lottoExtraNumbers = new int[7];

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
                        break;

                    case 1:
                        lottoMaxNumbers = ChangeLotteyNumbers(lottoMaxNumbers, LottoMAXSize, LottoMAXMax, "Lotto MAX");
                        ExtraMax = RandomBonusNumber(lottoMaxNumbers, LottoMAXMax, LottoMAXSize);
                        break;

                    case 2:
                        lotto649Numbers = ChangeLotteyNumbers(lotto649Numbers, Lotto649Size, Lotto649Max, "Lotto 6/49");
                        Extra649 = RandomBonusNumber(lotto649Numbers, Lotto649Max, Lotto649Size);
                        break;

                    case 3:
                        lottoExtraNumbers = ChangeExtraNumber(lottoExtraNumbers);
                        break;

                    case 4:
                        //Call PlayLottoMAX
                        break;

                    case 5:
                        //call PlayLotto649
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

        //-----------------------LOTTO MAX--------------------------

        static void PlayLottoMax(int[] playerNumbers, int extraNumber, int max, int size)
        {
            //declare variables 
            int[] winningNumbers = new int[size];
            int matches,
                bonus;

            //generate winning numbers
            winningNumbers = GenerateNumberSet(winningNumbers, max, size);
            bonus = RandomBonusNumber(winningNumbers, max, size);

            //find matches between winning and player numbers
            matches = FindMatches(playerNumbers, winningNumbers, bonus, size);

            //find bonus match if all non-bonus numbers did not match
            if (matches != size)
            {
                BonusMatch(winningNumbers, bonus, size);
            }
            else
            {

            }
        }
        
        //-----------------------LOTTO 6/49-------------------------
        static int FindMatches(int[] playerNumbers, int[] winningNumbers, int bonus, int size)
        {
            int matches = 0,
                cycle = 0;
            bool scanComplete = false;

            do
            {
                for (int index = 0; index < size; index++)
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
                if(cycle == size)
                {
                    scanComplete = true;
                }
                
            } while (scanComplete == false);

            return matches;
        }//end of FindMatches

        static bool BonusMatch(int[] winningNumbers, int bonus, int size)
        {
            bool match = false;

            for (int index = 0; index < size; index++)
            {
                if (winningNumbers[index] == bonus)
                {
                    match = true;
                    index = size;
                }
            }

            return match;
        }
        //-----------------------LOTTO EXTRA-------------------------

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
            int[] number = new int[LottoMAXSize];

            for (int index = 0; index < LottoMAXSize; index++)
            {
                number[index] = random.Next(1, LottoMAXSize);
            }
           
            return number;

        }//end of GenerateExtraNumber

        //-----------------------Shared Methods-------------------------

        static int RandomBonusNumber(int[] lotteryNumbers, int max, int size)
        {
            Random random = new Random();
            int number;
            bool unique = false;

            do
            {
                number = random.Next(1, max);
                if (SearchArray(lotteryNumbers, size, number) == -1)
                {
                    unique = true;
                }
            } while (unique != false);

            return number;  
        }//end of RandomExtraNumber

        static int[] ChooseNumbers(int size, int max)
        {
            int[] lotteryNumbers = new int[size];

            for (int index = 0; index < size; index++)
            {
                lotteryNumbers[index] = GetUniqueInt($"Enter number {index+1}: ", lotteryNumbers, size, index, max);
            }

            return lotteryNumbers;
        }//ChooseNumbers
        static int[] ChangeLotteyNumbers(int[] lotteryNumbers, int size, int max, string lotteryName)
        {
            char option;
            //Display current numbers
            Console.WriteLine($"The current {lotteryName} numbers are: ");
            for (int index = 0; index < size; index++)
            {
                Console.Write($"{lotteryNumbers[index]}");
                if (index != (size - 1))
                {
                    Console.Write(", ");
                }
            }
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

            DisplayArray(lotteryNumbers, lotteryName, size);


            return lotteryNumbers;
        }//end of ChangeLotteryNumbers

        static void DisplayArray(int[] lotteryNumbers, string lotteryName, int size)
        {
            Console.Write($"\nThe new {lotteryName} numbers are: ");
            for (int index = 0; index < size; index++)
            {
                Console.Write($"{lotteryNumbers[index]}");
                if(index != (size - 1))
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine('\n');
        }//end of DisplayArray

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
                    if(number > max)
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

        static int GetUniqueInt(string prompt, int[] lotteryNumbers, int size, int index, int max)
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

                if (location > -1)
                {
                    addComplete = true;
                }
                else
                {
                    prompt = "Please enter a unique number: ";
                }
            }while(addComplete == false);
            

            return number;

        }//end of GetSafeInt

        static int SearchArray(int[] lotteryNumbers, int size, int numberSearch)
        {

            int location = -1;
            for (int index = 0; index < size; index++)
            {
                if (lotteryNumbers[index] != numberSearch)
                {
                    location = index;
                }
                else
                {
                    location = -1;
                    index = size;
                }
            }

            return location;
        }//end of SearchArray

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
                } while (uniqueNumber == false) ;
            }
            return lotteryNumbers;
        }//end of GenerateArray

        

    }
}
