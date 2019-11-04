using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Description: Aids in the operation of the finch robot. Manage connections and give simple tasks.
    // Application Type: Console
    // Author: Alex Suarez
    // Dated Created: 9/30/2019
    // Last Modified: 10/23/2019
    //
    // **************************************************

    class Program
    {
        enum Command
        {
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            GETTEMP,
            GETLIGHT,
            PLAYBEEP,
            MARCH,
            DONE
        }
        static void Main(string[] args)
        {
            DisplayWelcomeScreen();

            DisplayMenuScreen();

            DisplayClosingScreen();
        }

        #region HELPER METHODS
        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            DisplayHeader("Finch Control");
            Console.WriteLine("This program will help you manage the operation of your finch.");
            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            DisplayHeader("Thank you for using Finch Control!");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Display continue prompt.
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Console.WriteLine();
        }

        /// <summary>
        /// Clears the screen and displays a header. (header)
        /// </summary>
        static void DisplayHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        /// <summary>
        /// Displays an error message to the user.
        /// </summary>
        static void DoErrorMessage(string extraText = "")
        {
            Console.Write("Invalid input. ");
            Console.WriteLine(extraText);
            Console.WriteLine();
        }

        /// <summary>
        /// Prompts the user for a valid integer input. Looped. (prompt)
        /// </summary>
        static int GetInteger(string prompt, string prompt2 = "")
        {
            bool didParse = false;
            string userInput;
            int result;
            do
            {
                Console.Write(prompt);
                Console.WriteLine(prompt2);
                Console.WriteLine();
                userInput = Console.ReadLine();
                didParse = Int32.TryParse(userInput, out result);
                if (didParse)
                {
                    Int32.TryParse(userInput, out result);
                    Console.WriteLine();
                }
                else
                {
                    DoErrorMessage();
                }
            } while (!didParse);
            return result;
        }

        /// <summary>
        /// Prompts the user for a valid double input. Looped. (prompt)
        /// </summary>
        static double GetDouble(string prompt, string prompt2 = "")
        {
            bool didParse = false;
            string userInput;
            double result;
            do
            {
                Console.Write(prompt);
                Console.WriteLine(prompt2);
                Console.WriteLine();
                userInput = Console.ReadLine();
                didParse = Double.TryParse(userInput, out result);
                if (didParse)
                {
                    Double.TryParse(userInput, out result);
                    Console.WriteLine();
                }
                else
                {
                    DoErrorMessage();
                }
            } while (!didParse);
            return result;
        }

        /// <summary>
        /// Prompts the user for a valid command. Looped. (prompt)
        /// </summary>
        static Command GetCommand(string prompt, string prompt2 = "")
        {
            bool didParse = false;
            string userInput;
            Command result;
            do
            {
                Console.Write(prompt);
                Console.Write(prompt2 + " >>");
                userInput = Console.ReadLine().ToUpper();
                didParse = Enum.TryParse(userInput, out result);
                if (didParse)
                {
                    Enum.TryParse(userInput, out result);
                    Console.WriteLine();
                }
                else
                {
                    DoErrorMessage("Valid inputs are: MoveForward, MoveBackward, StopMotors, Wait, TurnRight, TurnLeft, LEDOn, LEDOff");
                    Console.WriteLine("GetTemp, GetLight, PlayBeep, March, Done");
                }
            } while (!didParse);
            return result;
        }

        static bool GetBool(string prompt, string prompt2 = "")
        {
            bool output = false;
            bool inputValidated;
            ConsoleKeyInfo inputKey;
            char userInput;
            Console.Write(prompt);
            Console.WriteLine(prompt2);
            Console.WriteLine();
            do
            {
                inputKey = Console.ReadKey();
                userInput = inputKey.KeyChar;
                Console.WriteLine();
                switch (userInput)
                {
                    case 'Y':
                    case 'y':
                        inputValidated = true;
                        output = true;
                        break;
                    case 'N':
                    case 'n':
                        inputValidated = true;
                        output = false;
                        break;
                    default:
                        inputValidated = false;
                        DoErrorMessage("Valid inputs are Y or N.");
                        break;
                }
            } while (!inputValidated);
            return output;
        }
        #endregion

        #region Standalone Methods

        /// <summary>
        /// Displays the menu screen.
        /// </summary>
        static void DisplayMenuScreen()
        {
            Finch finchRobot = new Finch();

            char menuChoice;
            ConsoleKeyInfo menuKey;
            bool quitApplication = false;
            bool robotConnected = false;

            do
            {
                DisplayHeader("Main Menu");
                Console.WriteLine();
                Console.WriteLine("Please select a menu option.");
                Console.WriteLine();
                Console.WriteLine("1. Connect Finch Robot");
                Console.WriteLine("2. Talent Show");
                Console.WriteLine("3. Data Recorder");
                Console.WriteLine("4. Alarm System");
                Console.WriteLine("5. User Programming");
                Console.WriteLine("6. Disconnect Finch Robot");
                Console.WriteLine("7. Quit");

                menuKey = Console.ReadKey();
                Console.WriteLine();
                menuChoice = menuKey.KeyChar;
                switch (menuChoice)
                {
                    case '1':
                    case 'a':
                        if (robotConnected == true)
                        {
                            Console.WriteLine("The finch robot is already connected.");
                            DisplayContinuePrompt();
                            break;
                        }
                        else
                        {
                            robotConnected = DisplayConnectFinchRobot(finchRobot);
                            break;
                        }
                    case '2':
                    case 'b':
                        if (robotConnected == false)
                        {
                            Console.WriteLine("The finch robot is not connected. Please connect first.");
                            DisplayContinuePrompt();
                            break;
                        }
                        else
                        {
                            DisplayTalentMenuScreen(finchRobot);
                            break;
                        }
                    case '3':
                    case 'c':
                        if (robotConnected == false)
                        {
                            Console.WriteLine("The finch robot is not connected. Please connect first.");
                            DisplayContinuePrompt();
                            break;
                        }
                        else
                        {
                            DisplayDataMenuScreen(finchRobot);
                            break;
                        }
                    case '4':
                    case 'd':
                        if (robotConnected == false)
                        {
                            Console.WriteLine("The finch robot is not connected. Please connect first.");
                            DisplayContinuePrompt();
                            break;
                        }
                        else
                        {
                            DisplayAlarmMenuScreen(finchRobot);
                            break;
                        }
                    case '5':
                    case 'e':
                        if (robotConnected == false)
                        {
                            Console.WriteLine("The finch robot is not connected. Please connect first.");
                            DisplayContinuePrompt();
                            break;
                        }
                        else
                        {
                            DisplayUPMenuScreen(finchRobot);
                        }
                        break;
                    case '6':
                    case 'f':
                        if (robotConnected == false)
                        {
                            Console.WriteLine("The finch robot is not connected. Please connect first.");
                            DisplayContinuePrompt();
                            break;
                        }
                        else
                        {
                            robotConnected = DisplayDisconnectFinchRobot(finchRobot);
                            break;
                        }
                    case '7':
                    case 'g':
                    case 'q':
                        if (robotConnected)
                        {
                            Console.WriteLine("Please disconnect the finch before exiting.");
                            DisplayContinuePrompt();
                            break;
                        }
                        else
                        {
                            quitApplication = true;
                            break;
                        }
                    default:
                        DoErrorMessage();
                        break;
                }
            } while (!quitApplication);
        }

        /// <summary>
        /// Menu selection that connects the Finch robot to the application.
        /// </summary>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            bool robotConnected;
            int connectionsAttempted = 0;

            do
            {
                DisplayHeader("Connection with Finch Robot");
                Console.WriteLine("About to connect to Finch robot. Please ensure all connections are secure.");
                DisplayContinuePrompt();

                robotConnected = finchRobot.connect();

                if (robotConnected)
                {
                    Console.WriteLine("Connected.");
                    finchRobot.setLED(0, 255, 0);
                    PlayTripletBeep(finchRobot, 500, 500);
                    DisplayContinuePrompt();
                }
                else
                {
                    Console.WriteLine("Could not connect.");
                    connectionsAttempted += 1;
                    Console.WriteLine($"Please try again - you may attempt {3 - connectionsAttempted} more times.");
                    if (connectionsAttempted == 5) Console.WriteLine("Returning to main menu.");
                    DisplayContinuePrompt();
                }
            } while (!robotConnected && connectionsAttempted < 3);
            return robotConnected;
        }

        /// <summary>
        /// Menu selection that disconnects the Finch from the program.
        /// </summary>
        static bool DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            bool robotConnected;
            bool verifiedDisconnect;
            DisplayHeader("Disconnection with Finch Robot");

            verifiedDisconnect = GetBool("You're about to disconnect your finch robot. Would you like to continue? [Y/N]");
            if (verifiedDisconnect)
            {
                Console.WriteLine("Disconnecting finch.");
                finchRobot.disConnect();
                robotConnected = false;
            }
            else
            {
                Console.WriteLine("Did not disconnect. Returning to menu.");
                robotConnected = true;
            }
            DisplayContinuePrompt();
            return robotConnected;
        }

        /// <summary>
        /// Plays three confirmation beeps.
        /// </summary>
        static void PlayTripletBeep(Finch finchRobot, int finchNote, int duration)
        {
            for (int i = 1; i < 4; i++)
            {
                finchRobot.noteOn(finchNote);
                finchRobot.wait(duration * 3 / 12);
                finchRobot.noteOff();
                finchRobot.wait(duration / 12);
            }
        }

        /// <summary>
        /// Signs "OK" in morse code. Total duration = duration(11/6)
        /// </summary>
        static void PlayOKBeep(Finch finchRobot, int finchNote, int duration)
        {
            for (int i = 1; i <= 3; i++)
            {
                finchRobot.noteOn(finchNote);
                finchRobot.wait(duration * 3 / 12);
                finchRobot.noteOff();
                finchRobot.wait(duration / 12);
            }
            finchRobot.wait(duration / 12);
            for (int i = 1; i <= 3; i++)
            {
                finchRobot.noteOn(finchNote);
                if (i == 2) finchRobot.wait(duration / 12); else finchRobot.wait(duration * 3 / 12);
                finchRobot.noteOff();
                finchRobot.wait(duration / 12);
            }
            finchRobot.setLED(0, 255, 0);
        }

        #endregion

        #region TalentShowFunctions

        /// <summary>
        /// Displays the talent menu screen.
        /// </summary>
        static void DisplayTalentMenuScreen(Finch finchRobot)
        {
            char menuChoice;
            ConsoleKeyInfo menuKey;
            bool quitMenu = false;

            do
            {
                DisplayHeader("Talent Show Selection");
                Console.WriteLine();
                Console.WriteLine("Please select a talent show option.");
                Console.WriteLine();
                Console.WriteLine("1. LED Sequence");
                Console.WriteLine("2. Finch Song");
                Console.WriteLine("3. T Drive Sequence");
                Console.WriteLine("4. Rotation Sequence");
                Console.WriteLine("5. Quit");

                menuKey = Console.ReadKey();
                Console.WriteLine();
                menuChoice = menuKey.KeyChar;
                switch (menuChoice)
                {
                    case '1':
                    case 'a':
                        TalentShowLED(finchRobot);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '2':
                    case 'b':
                        TalentShowSong(finchRobot);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '3':
                    case 'c':
                        TalentShowDrive(finchRobot);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '4':
                    case 'd':
                        TalentShowRotate(finchRobot);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '5':
                    case 'e':
                    case 'q':
                        quitMenu = true;
                        break;
                    default:
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// Gets an LED sequence from the user, and displays it.
        /// </summary>
        static void TalentShowLED(Finch finchRobot)
        {
            DisplayHeader("Talent Show LED");
            Console.WriteLine("The Finch will let you set the color of its LED.");
            DisplayContinuePrompt();
            GetFinchLED(finchRobot);
        }

        static void TalentShowSong(Finch finchRobot)
        {
            DisplayHeader("Talent Show Song");
            Console.WriteLine("The Finch will play a short tune with some lights.");
            DisplayContinuePrompt();
            FinchSong(finchRobot, 180);
        }

        /// <summary>
        /// Drives the finch in a T-shape.
        /// </summary>
        static void TalentShowDrive(Finch finchRobot)
        {
            DisplayHeader("Talent Show Drive");
            Console.WriteLine("The Finch will now drive and turn.");
            DisplayContinuePrompt();
            FinchTDrive(finchRobot);
        }

        /// <summary>
        /// Alternatively rotates the finch.
        /// </summary>
        static void TalentShowRotate(Finch finchRobot)
        {
            DisplayHeader("Talent Show Rotate");
            Console.WriteLine("The Finch will now start turning in clockwise circles.");
            DisplayContinuePrompt();
            FinchRotate(finchRobot);
        }

        /// <summary>
        /// Prompts the user for an RGB value, and assigns it to the Finch's LED indicator. Can set multiple.
        /// </summary>
        static void GetFinchLED(Finch finchRobot)
        {
            int[] arrayLED = new int[16];
            int forLoopStatus = 0;
            bool repeatExtras = false;
            DisplayHeader("Talent Show LED");
            Console.WriteLine("The Finch will let you set the color of its LED.");
            Console.WriteLine();
            for (int i = 0; i <= 11; i++)
            {
                string inputLED = "null";
                switch (i)
                {
                    case 0:
                    case 4:
                    case 8:
                        inputLED = "red LED. (0-255)";
                        break;
                    case 1:
                    case 5:
                    case 9:
                        inputLED = "green LED. (0-255)";
                        break;
                    case 2:
                    case 6:
                    case 10:
                        inputLED = "blue LED. (0-255)";
                        break;
                    case 3:
                    case 7:
                    case 11:
                        inputLED = "light's duration. (in ms)";
                        break;
                    default:
                        break;
                }
                arrayLED[i] = (int)GetInteger("Set the value of the ", inputLED);
                if (i != 3 && i != 7 && i != 11 && arrayLED[i] >= 256) arrayLED[i] = 255;
                if (i != 3 && i != 7 && i != 11 && arrayLED[i] < 0) arrayLED[i] = 0;
                if ((i == 3 || i == 7 || i == 11) && arrayLED[i] < 0) arrayLED[i] = 0;
                if (i == 3 || i == 7)
                {
                    repeatExtras = GetBool("Would you like to add another light to the sequence? (Y/N)");
                    if (repeatExtras)
                    {
                        DisplayHeader("Talent Show LED");
                        Console.WriteLine("The Finch will let you set the color of its LED.");
                        Console.WriteLine();
                    }
                    if (!repeatExtras)
                    {
                        forLoopStatus = i;
                        break;
                    }
                }
                if (i == 11) forLoopStatus = i;
            }
            DisplayContinuePrompt();
            finchRobot.setLED(arrayLED[0], arrayLED[1], arrayLED[2]);
            Console.WriteLine($"Setting LED to {arrayLED[0]}, {arrayLED[1]}, {arrayLED[2]} (RGB) for {arrayLED[3]}ms");
            finchRobot.wait(arrayLED[3]);
            if (forLoopStatus > 3)
            {
                finchRobot.setLED(arrayLED[4], arrayLED[5], arrayLED[6]);
                Console.WriteLine($"Setting LED to {arrayLED[4]}, {arrayLED[5]}, {arrayLED[6]} (RGB) for {arrayLED[7]}ms");
                finchRobot.wait(arrayLED[7]);
                if (forLoopStatus > 7)
                {
                    finchRobot.setLED(arrayLED[8], arrayLED[9], arrayLED[10]);
                    Console.WriteLine($"Setting LED to {arrayLED[8]}, {arrayLED[9]}, {arrayLED[10]} (RGB) for {arrayLED[11]}ms");
                    finchRobot.wait(arrayLED[11]);
                }
            }
            finchRobot.setLED(0, 0, 0);
        }

        /// <summary>
        /// Plays "Electric Zoo" with attached quote.
        /// </summary>
        static void FinchSong(Finch finchRobot, int BPM)
        {
            int beat = (int)60000 / BPM;
            beat = 60000 / BPM;

            for (int i = 0; i < 2; i++)
            {

                if (i == 0)
                {
                    finchRobot.setLED(255, 0, 0);
                    Console.WriteLine("Bee-boo-boo-bop, boo-boo-beep.");
                    Console.WriteLine("Nah man, you're thinking 'Bee-boo-boo-bop, boo-boo-bop'");
                    Console.WriteLine("Bee-boo-boo-boo-boo-bop?");
                    Console.WriteLine("bee-boo-boo-bop...");
                    Console.WriteLine("boo-boo-bee-bop? Not bee-boo-boo-beep? Bop? Beep?! Boo-boo-bop?!");
                }
                else if (i == 1)
                {
                    finchRobot.setLED(0, 0, 255);
                }
                else
                {
                    finchRobot.setLED(0, 0, 0);
                }
                finchRobot.noteOn(831); //Ab5, 8th note
                finchRobot.wait(beat / 4);
                finchRobot.noteOff();
                finchRobot.wait(beat / 4);

                finchRobot.noteOn(831); //Ab5, 8th note
                finchRobot.wait(beat / 4);
                finchRobot.noteOff();
                finchRobot.wait(beat / 4);

                finchRobot.noteOn(466); //Bb4, 8th note
                finchRobot.wait(beat / 4);
                finchRobot.noteOff();
                finchRobot.wait(beat / 4);

                finchRobot.noteOn(523); //C5, 8th note
                finchRobot.wait(beat / 4);
                finchRobot.noteOff();
                finchRobot.wait(beat / 4);

                finchRobot.wait(beat / 2); //8th rest

                finchRobot.noteOn(349); //F4, 8th note
                finchRobot.wait(beat / 4);
                finchRobot.noteOff();
                finchRobot.wait(beat / 4);

                finchRobot.noteOn(1046); //C6, 8th note
                finchRobot.wait(beat / 4);
                finchRobot.noteOff();
                finchRobot.wait(beat / 4);

                finchRobot.wait(beat / 2); //8th rest
                finchRobot.wait(4 * beat); //measure rest

                finchRobot.setLED(0, 0, 0);
            }

        }

        /// <summary>
        /// Moves the Finch at the given speed, for the given time.
        /// </summary>
        static void FinchMoveStraight(Finch finchRobot, int speed, int duration)
        {
            finchRobot.setMotors(speed, speed);
            finchRobot.wait(duration);
            finchRobot.setMotors(0, 0);
        }

        /// <summary>
        /// Causes the Finch to turn 90* to the left.
        /// </summary>
        static void FinchTurnLeft(Finch finchRobot)
        {
            finchRobot.setMotors(-75, 75);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);
        }

        /// <summary>
        /// Causes the Finch to turn 90* to the right.
        /// </summary>
        static void FinchTurnRight(Finch finchRobot)
        {
            finchRobot.setMotors(75, -75);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);
        }

        /// <summary>
        /// Drives the finch in a T-shape
        /// </summary>
        static void FinchTDrive(Finch finchRobot)
        {
            FinchMoveStraight(finchRobot, 75, 2000);
            FinchTurnRight(finchRobot);
            FinchMoveStraight(finchRobot, 75, 2000);
            FinchTurnLeft(finchRobot);
            FinchTurnLeft(finchRobot);
            FinchMoveStraight(finchRobot, 100, 3000);
            FinchTurnRight(finchRobot);
            FinchTurnRight(finchRobot);
            FinchMoveStraight(finchRobot, 75, 2000);
            FinchTurnRight(finchRobot);
            FinchMoveStraight(finchRobot, 50, 6000);
        }

        /// <summary>
        /// Rotates the Finch clockwise/counterclockwise alternatively.
        /// </summary>
        static void FinchRotate(Finch finchRobot)
        {
            string userInput;
            int currentLeftMotor;
            finchRobot.setMotors(75, 0);
            currentLeftMotor = 75;
            Console.WriteLine();
            Console.WriteLine("Type \"swap\" to change the finch's direction of rotation.");
            Console.WriteLine("Type \"stop\"  or enter nothing to stop the finch.");
            do
            {
                userInput = Console.ReadLine().ToLower();
                if (userInput == "swap")
                {
                    if (currentLeftMotor == 75)
                    {
                        finchRobot.setMotors(-75, 75);
                        currentLeftMotor = 0;
                    }
                    else
                    {
                        finchRobot.setMotors(75, -75);
                        currentLeftMotor = 75;
                    }
                }
                else if (userInput == "stop" || userInput == "")
                {
                    finchRobot.setMotors(0, 0);
                    userInput = "stop";
                }
            } while (userInput != "stop");
            Console.WriteLine("The finch should now be stopped.");
        }

        #endregion

        #region DataRecorderFunctions

        /// <summary>
        /// Displays the data recorder menu screen.
        /// </summary>
        static void DisplayDataMenuScreen(Finch finchRobot)
        {
            char menuChoice;
            ConsoleKeyInfo menuKey;
            bool quitMenu = false;

            do
            {
                DisplayHeader("Data Recorder Selection");
                Console.WriteLine();
                Console.WriteLine("Please select a data option.");
                Console.WriteLine();
                Console.WriteLine("1. Temperature Recorder");
                Console.WriteLine("2. Light Recorder");
                Console.WriteLine("3. Finch Gyroscope");
                Console.WriteLine("4. Quit");

                menuKey = Console.ReadKey();
                Console.WriteLine();
                menuChoice = menuKey.KeyChar;
                switch (menuChoice)
                {
                    case '1':
                    case 'a':
                        DataRecorderFrequency(finchRobot, 1);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '2':
                    case 'b':
                        DataRecorderFrequency(finchRobot, 2);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '3':
                    case 'c':
                        DataRecorderGyroscope(finchRobot);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '4':
                    case 'd':
                    case 'q':
                        quitMenu = true;
                        break;
                    default:
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// Gets the parameters for data recording, and passes it to the recorder. Passes recorder data to reporter.
        /// </summary>
        static void DataRecorderFrequency(Finch finchRobot, int dataType)
        {
            bool reassuredInput = true;
            int arrayLength;
            double frequencyCount;
            double lengthCount;
            double[] dataPoints;

            DisplayHeader("Frequency Recorder");
            frequencyCount = GetDouble("How much time should there be between readings? (In whole seconds)");
            if (frequencyCount <= 0)
            {
                Console.WriteLine("Could not set value to something less than or equal to 0. Value set to 0.1.");
                frequencyCount = 0.1;
            }
            Console.WriteLine($"Getting data every {frequencyCount} seconds.");
            Console.WriteLine();
            do
            {
                lengthCount = GetDouble("How many seconds would you like to collect?");
                if (lengthCount >= 10) reassuredInput = GetBool($"Are you sure you'd like to record {lengthCount} seconds of data? [Y/N]");
            } while (!reassuredInput);
            if (lengthCount <= 0)
            {
                Console.WriteLine("Could not set value to something less than or equal to 0. Value set to 0.1.");
                lengthCount = 0.1;
            }
            Console.WriteLine($"Collecting {lengthCount} seconds worth of data.");
            arrayLength = (int)(lengthCount / frequencyCount);
            dataPoints = new double[arrayLength];
            Console.WriteLine();
            Console.WriteLine($"Expect {arrayLength} points of data.");
            DisplayContinuePrompt();

            DisplayHeader("Data Recorder");
            dataPoints = GetSensorData(finchRobot, frequencyCount, lengthCount, dataType);

            DisplayHeader("Data Report");
            DisplaySensorData(dataPoints, dataType);
        }

        /// <summary>
        /// NYI
        /// </summary>
        static void DataRecorderGyroscope(Finch finchRobot)
        {
            //todo Data Recorder Gyroscope
            DisplayHeader("Gyroscope");
            Console.WriteLine("Module under development.");
        }

        /// <summary>
        /// Gets data from the finch sensors based on passed parameters. Data types Temperature = 1, Light = 2.
        /// </summary>
        static double[] GetSensorData(Finch finchRobot, double frequencyCount, double lengthCount, int dataType)
        {
            int arrayLength = (int)(lengthCount / frequencyCount);
            int leftSensor;
            int rightSensor;

            double[] dataPoints = new double[arrayLength];
            Console.WriteLine($"Getting a reading every {frequencyCount} seconds for {lengthCount} seconds.");
            Console.WriteLine($"Expect {arrayLength} readings.");
            DisplayContinuePrompt();

            for (int i = 1; i <= arrayLength; i++)
            {
                if (dataType == 1)
                {
                    dataPoints[i - 1] = finchRobot.getTemperature();
                    Console.WriteLine($"Got reading of {dataPoints[i - 1]}");
                }
                if (dataType == 2)
                {
                    leftSensor = finchRobot.getLeftLightSensor();
                    rightSensor = finchRobot.getRightLightSensor();
                    dataPoints[i - 1] = (leftSensor + rightSensor) / 2;
                    Console.WriteLine($"Got reading of {dataPoints[i - 1]}");
                }
                finchRobot.wait((int)(frequencyCount * 1000));
            }
            Console.WriteLine();
            Console.WriteLine("Data Recording Complete");
            Console.WriteLine();
            DisplayContinuePrompt();
            return dataPoints;
        }

        static void DisplaySensorData(double[] dataPoints, int dataType)
        {
            double average = 0;
            double averageFarenheit = 0;
            for (int i = 1; i <= dataPoints.Length; i++)
            {
                average = average + dataPoints[i - 1];
                if (dataType == 1) averageFarenheit = averageFarenheit + ((dataPoints[i - 1] + 32) * 9 / 5);
            }
            average = average / dataPoints.Length;
            if (dataType == 1) averageFarenheit = averageFarenheit / dataPoints.Length;

            Console.Write("{0,-10} {1,-10} {2,-15}", "Trial #", "Reading", "From Average");
            if (dataType == 1)
            {
                Console.WriteLine("{0,-20} {1,-25}", "Farenheit Reading", "Farenheit From Average");
            }
            else Console.WriteLine();
            string dashes = new String('-', 24);
            Console.WriteLine(dashes);
            for (int i = 1; i <= dataPoints.Length; i++)
            {
                Console.Write("{0,-10} {1,-10:N2} {2,-15:N2}", "#" + i, dataPoints[i - 1], dataPoints[i - 1] - average);
                if (dataType == 1)
                {
                    Console.WriteLine("{0,-20:N2} {1,-25:N2}", (dataPoints[i - 1] + 32) * 9 / 5, ((dataPoints[i - 1] + 32) * 9 / 5) - averageFarenheit);
                }
                else Console.WriteLine();
            }
            Console.Write("{0,-20} {1,-15:N2}", "Average:", average);
            if (dataType == 1)
            {
                Console.WriteLine("{0, -20} {1, -25:N2}", "Average Farenheit:", averageFarenheit);
            }
            else Console.WriteLine();
        }

        #endregion

        #region AlarmSystemFunctions

        /// <summary>
        /// Displays the alarm menu screen.
        /// </summary>
        static void DisplayAlarmMenuScreen(Finch finchRobot)
        {
            char menuChoice;
            ConsoleKeyInfo menuKey;
            bool quitMenu = false;

            do
            {
                DisplayHeader("Alarm System Selection");
                Console.WriteLine();
                Console.WriteLine("Please select a data option.");
                Console.WriteLine();
                Console.WriteLine("1. Temperature Alarm");
                Console.WriteLine("2. Light Alarm");
                Console.WriteLine("3. Temperature & Light Alarm");
                Console.WriteLine("4. Gyroscope");
                Console.WriteLine("5. Quit");

                menuKey = Console.ReadKey();
                Console.WriteLine();
                menuChoice = menuKey.KeyChar;
                switch (menuChoice)
                {
                    case '1':
                    case 'a':
                        AlarmSystemFrequency(finchRobot, 1);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '2':
                    case 'b':
                        AlarmSystemFrequency(finchRobot, 2);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '3':
                    case 'c':
                        AlarmSystemFrequency(finchRobot, 3);
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '4':
                    case 'd':
                        //todo Alarm System Gyroscope
                        Console.WriteLine("Module under development.");
                        DisplayContinuePrompt();
                        PlayOKBeep(finchRobot, 361, 1000);
                        break;
                    case '5':
                    case 'e':
                    case 'q':
                        quitMenu = true;
                        break;
                    default:
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// Gets the parameters for an alarm system, and passes it to the monitor.
        /// Datatypes: 1 = temperature, 2 = light, 3 = both
        /// </summary>
        static void AlarmSystemFrequency(Finch finchRobot, int dataType)
        {
            int thresholdExceeded;
            bool reassuredInput = true;
            double frequencyCount;
            double lengthCount;
            double targetValue1 = 10;
            double thresholdUpper1 = 1;
            double thresholdLower1 = 1;
            double targetValue2 = 10;
            double thresholdUpper2 = 1;
            double thresholdLower2 = 1;

            DisplayHeader("Alarm Settings");
            frequencyCount = GetDouble("How much time should there be between readings? (In whole seconds)");
            if (frequencyCount <= 0)
            {
                Console.WriteLine("Could not set value to something less than or equal to 0. Value set to 0.1.");
                frequencyCount = 0.1;
            }
            Console.WriteLine($"Getting input every {frequencyCount} seconds.");

            Console.WriteLine();
            do
            {
                lengthCount = GetDouble("How many seconds would you like to collect?");
                if (lengthCount >= 10) reassuredInput = GetBool($"Are you sure you'd like to record  for {lengthCount} seconds? [Y/N]");
            } while (!reassuredInput);
            if (lengthCount <= 0)
            {
                Console.WriteLine("Could not set value to something less than or equal to 0. Value set to 0.1.");
                lengthCount = 0.1;
            }
            Console.WriteLine($"Monitoring for {lengthCount} seconds.");
            Console.WriteLine();
            DisplayContinuePrompt();

            DisplayHeader("Threshold Input");
            if (dataType == 1 || dataType == 3)
            {
                thresholdUpper1 = finchRobot.getTemperature();
                Console.WriteLine($"The current temperature is {thresholdUpper1:F2}*C.");
                targetValue1 = GetDouble("What is the desired temperature?");
                thresholdUpper1 = GetDouble("How far above this temperature is it allowed to be?");
                thresholdLower1 = GetDouble("How far below this temperature is it allowed to be?");
            }
            if (dataType == 2 || dataType == 3)
            {
                thresholdUpper2 = ((finchRobot.getLeftLightSensor() + finchRobot.getLeftLightSensor()) / 2);
                Console.WriteLine($"The current light from the sensor is {thresholdUpper2}.");
                targetValue2 = GetDouble("What is the desired light value?");
                thresholdUpper2 = GetDouble("How far above this value is desired?");
                thresholdLower2 = GetDouble("How far below this value is desired?");
            }
            thresholdExceeded = AlarmSystemMonitor(finchRobot, frequencyCount, lengthCount,
                thresholdUpper1, thresholdLower1, targetValue1, thresholdUpper2, thresholdLower2, targetValue2, dataType);
            switch (thresholdExceeded)
            {
                case 0:
                    Console.WriteLine("No threshold exceeded - timeout occured.");
                    break;
                case 1:
                    Console.WriteLine("Threshold exceeded - temperature exceeded upper limit.");
                    break;
                case 2:
                    Console.WriteLine("Threshold exceeded - temperature exceeded lower limit.");
                    break;
                case 3:
                    Console.WriteLine("Threshold exceeded - light exceeded upper limit.");
                    break;
                case 4:
                    Console.WriteLine("Threshold exceeded - light exceeded lower limit.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Receives parameters for an alarm system, and executes a monitor based on them.
        /// Datatypes: 1 = temperature, 2 = light, 3 = both
        /// </summary>
        static int AlarmSystemMonitor(Finch finchRobot, double frequencyCount, double lengthCount,
            double thresholdUpper1, double thresholdLower1, double targetValue1,
            double thresholdUpper2, double thresholdLower2, double targetValue2, int dataType)
        {
            int returnType = 0;
            double monitorValue1 = 10;
            double monitorValue2 = 10;
            double timeElapsed = 0;

            finchRobot.setLED(0, 255, 0);
            if (dataType == 1)
            {
                do
                {
                    monitorValue1 = finchRobot.getTemperature();
                    DisplayHeader("Alarm Monitor");
                    Console.WriteLine($"Current temperature value: {monitorValue1:F2}. Current runtime: {timeElapsed:F2}.");
                    Console.WriteLine($"Upper Limit: {targetValue1 + thresholdUpper1}. " +
                        $"Lower Limit: {targetValue1 - thresholdLower1}.");
                    finchRobot.wait((int)(1000 * frequencyCount));
                    timeElapsed += frequencyCount;
                } while (monitorValue1 > (targetValue1 - thresholdLower1) && monitorValue1 < (targetValue1 + thresholdUpper1)
                       && timeElapsed < lengthCount);
            }
            if (dataType == 2)
            {
                do
                {
                    monitorValue2 = (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2;
                    DisplayHeader("Alarm Monitor");
                    Console.WriteLine($"Current light value: {monitorValue2}. Current runtime: {timeElapsed:F2}.");
                    Console.WriteLine($"Upper Limit: {targetValue2 + thresholdUpper2}. " +
                        $"Lower Limit: {targetValue2 - thresholdLower2}.");
                    finchRobot.wait((int)(1000 * frequencyCount));
                    timeElapsed += frequencyCount;
                } while (monitorValue2 > (targetValue2 - thresholdLower2) && monitorValue2 < (targetValue2 + thresholdUpper2)
                       && timeElapsed < lengthCount);
            }
            if (dataType == 3)
            {
                do
                {
                    monitorValue1 = finchRobot.getTemperature();
                    monitorValue2 = (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2;
                    DisplayHeader("Alarm Monitor");
                    Console.WriteLine($"Current temperature value: {monitorValue1:F2}. Current light value: {monitorValue2}." +
                        " Current runtime: {0:F2}.", timeElapsed);
                    Console.WriteLine($"Upper Limit Temperature: {targetValue1 + thresholdUpper1}. " +
                        $"Lower Limit Temperature: {targetValue1 - thresholdLower1}.");
                    Console.WriteLine($"Upper Limit Light: {targetValue2 + thresholdUpper2}. " +
                        $"Lower Limit Light: {targetValue2 - thresholdLower2}.");
                    finchRobot.wait((int)(1000 * frequencyCount));
                    timeElapsed += frequencyCount;
                } while (monitorValue1 > (targetValue1 - thresholdLower1) && monitorValue1 < (targetValue1 + thresholdUpper1)
                    && monitorValue2 > (targetValue2 - thresholdLower2) && monitorValue2 < (targetValue2 + thresholdUpper2)
                    && timeElapsed < lengthCount);
            }

            if (timeElapsed < lengthCount) finchRobot.setLED(255, 0, 0);
            if (monitorValue1 > (targetValue1 + thresholdUpper1)) returnType = 1;
            if (monitorValue1 < (targetValue1 - thresholdLower1)) returnType = 2;
            if (monitorValue2 > (targetValue2 + thresholdUpper2)) returnType = 3;
            if (monitorValue2 < (targetValue2 - thresholdLower2)) returnType = 4;
            return returnType;
        }
        #endregion

        #region UserProgrammingFunctions

        /// <summary>
        /// Displays the User Programming menu screen.
        /// </summary>
        static void DisplayUPMenuScreen(Finch finchRobot)
        {
            char menuChoice;
            ConsoleKeyInfo menuKey;
            bool quitMenu = false;

            (int, int, int) commandParameters = (0, 0, 0);
            string[] commandListNames = { "---", "---", "---", "---" };
            List<Command>[] commandList = new List<Command>[4];
            for (int i = 0; i < 4; i++)
            {
                commandList[i] = new List<Command>();
                commandList[i].Add(Command.DONE);
            }

            (List<Command>[], string[]) toupleUnpacker;

            do
            {
                DisplayHeader("User Programming Selection");
                Console.WriteLine();
                Console.WriteLine("Please select a User Programming option.");
                Console.WriteLine();
                Console.WriteLine("1. Set Command Parameters");
                Console.WriteLine("2. Create Command List");
                Console.WriteLine("3. View Command List");
                Console.WriteLine("4. Execute Command List");
                Console.WriteLine("5. Save Command List");
                Console.WriteLine("6. Load Command List");
                Console.WriteLine("7. Quit");

                menuKey = Console.ReadKey();
                Console.WriteLine();
                menuChoice = menuKey.KeyChar;
                switch (menuChoice)
                {
                    case '1':
                    case 'a':
                        commandParameters = GetCommandParameters();
                        DisplayContinuePrompt();
                        break;
                    case '2':
                    case 'b':
                        toupleUnpacker = UPCreateCommandList(commandList, commandListNames);
                        commandList = toupleUnpacker.Item1;
                        commandListNames = toupleUnpacker.Item2;
                        break;
                    case '3':
                    case 'c':
                        UPViewCommandList(commandList, commandListNames);
                        break;
                    case '4':
                    case 'd':
                        if (commandParameters.Item1 == 0 || commandParameters.Item3 == 0)
                        {
                            Console.WriteLine(
                                );
                            Console.WriteLine("Command parameters have not been set or are invalid. Please reset parameters.");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            UPExecuteCommandList(finchRobot, commandList, commandListNames, commandParameters);
                        }
                        break;
                    case '5':
                    case 'e':
                        WriteCommandsData(commandList, commandListNames);
                        break;
                    case '6':
                    case 'f':
                        DisplayHeader("Loading Commands List");
                        Console.WriteLine("You are about to overwrite your current command list.");
                        if (GetBool("Would you like to continue? [Y/N]"))
                        {
                            toupleUnpacker = ReadCommandsData();
                            commandList = toupleUnpacker.Item1;
                            commandListNames = toupleUnpacker.Item2;
                        }
                        break;
                    case '7':
                    case 'g':
                    case 'q':
                        quitMenu = true;
                        break;
                    default:
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// Gets the parameters to use for all UP commands.
        /// </summary>
        static (int motorSpeed, int ledBrightness, int waitMS) GetCommandParameters()
        {
            bool inputConfirmed = true;
            (int motorSpeed, int ledBrightness, int waitMS) commandParameters;
            DisplayHeader("Command Parameters");
            commandParameters.motorSpeed = GetInteger("Enter motor speed [0-100]: ");
            if (commandParameters.motorSpeed > 100)
            {
                Console.WriteLine("Input out of range: setting value to 100.");
                commandParameters.motorSpeed = 100;
            }
            else if (commandParameters.motorSpeed < 0)
            {
                Console.WriteLine("Input out of range: setting value to 0.");
                commandParameters.motorSpeed = 0;
            }
            commandParameters.ledBrightness = GetInteger("Enter LED brightness [0-255]: ");
            if (commandParameters.ledBrightness > 255)
            {
                Console.WriteLine("Input out of range: setting value to 255.");
                commandParameters.motorSpeed = 255;
            }
            else if (commandParameters.ledBrightness < 0)
            {
                Console.WriteLine("Input out of range: setting value to 0.");
                commandParameters.motorSpeed = 0;
            }
            do
            {
                commandParameters.waitMS = GetInteger("Enter wait time [ms]: ");
                if (commandParameters.motorSpeed >= 10000)
                {
                    inputConfirmed = GetBool("Are you sure you'd like to set wait to 10 seconds or more? [Y/N]");
                }
                else if (commandParameters.motorSpeed < 0)
                {
                    Console.WriteLine("Input out of range: setting value to 0.");
                    commandParameters.motorSpeed = 0;
                }
            } while (!inputConfirmed);

            return commandParameters;
        }

        /// <summary>
        /// Displays the menu for overwriting command lists.
        /// </summary>
        static (List<Command>[], string[]) UPCreateCommandList(List<Command>[] commandList, string[] commandListNames)
        {
            char menuChoice;
            ConsoleKeyInfo menuKey;
            bool quitMenu = false;

            (List<Command>, string) toupleUnpacker;
            do
            {
                DisplayHeader("Create New List");
                Console.WriteLine();
                Console.WriteLine("Please select a list to overwrite.");
                Console.WriteLine();
                Console.WriteLine("1. " + commandListNames[0]);
                Console.WriteLine("2. " + commandListNames[1]);
                Console.WriteLine("3. " + commandListNames[2]);
                Console.WriteLine("4. " + commandListNames[3]);
                Console.WriteLine("5. Quit");

                menuKey = Console.ReadKey();
                Console.WriteLine();
                menuChoice = menuKey.KeyChar;
                switch (menuChoice)
                {
                    case '1':
                    case 'a':
                        toupleUnpacker = UPGetCommandList();
                        commandList[0] = toupleUnpacker.Item1;
                        commandListNames[0] = toupleUnpacker.Item2;
                        DisplayContinuePrompt();
                        break;
                    case '2':
                    case 'b':
                        toupleUnpacker = UPGetCommandList();
                        commandList[1] = toupleUnpacker.Item1;
                        commandListNames[1] = toupleUnpacker.Item2;
                        DisplayContinuePrompt();
                        break;
                    case '3':
                    case 'c':
                        toupleUnpacker = UPGetCommandList();
                        commandList[2] = toupleUnpacker.Item1;
                        commandListNames[2] = toupleUnpacker.Item2;
                        DisplayContinuePrompt();
                        break;
                    case '4':
                    case 'd':
                        toupleUnpacker = UPGetCommandList();
                        commandList[3] = toupleUnpacker.Item1;
                        commandListNames[3] = toupleUnpacker.Item2;
                        DisplayContinuePrompt();
                        break;
                    case '5':
                    case 'e':
                    case 'q':
                        quitMenu = true;
                        break;
                    default:
                        break;
                }
            } while (!quitMenu);

            return (commandList, commandListNames);
        }

        /// <summary>
        /// Prompts the user to create a new command list.
        /// </summary>
        static (List<Command>, string) UPGetCommandList()
        {
            //todo Modify the UPGetCommandList method to return a touple of parameters.
            bool listBreak = false;
            List<Command> commandList = new List<Command>();
            string commandListName;
            DisplayHeader("New Command List");
            Console.WriteLine("Valid Commands Are:");
            Console.WriteLine("MoveForward, MoveBackward, StopMotors, Wait, TurnRight, TurnLeft, LEDOn, LEDOff");
            Console.WriteLine("GetTemp, GetLight, PlayBeep, March, Done");
            Console.WriteLine("Enter command \"Done\" to finish the list.");
            do
            {
                commandList.Add(GetCommand("Enter a command to add to the list:"));
                if (commandList.Contains(Command.DONE)) listBreak = true;
            } while (!listBreak);
            Console.Write("Enter a name for this command list: >>");
            commandListName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine($"Creating list \"{commandListName}\" of inputs: ");
            foreach (Command command in commandList)
            {
                if (command == Command.DONE) Console.WriteLine(command);
                if (command != Command.DONE) Console.Write(command + ", ");
            }
            return (commandList, commandListName);
        }

        /// <summary>
        /// View all created command lists.
        /// </summary>
        static void UPViewCommandList(List<Command>[] commandList, string[] commandListNames)
        {
            char menuChoice;
            ConsoleKeyInfo menuKey;
            bool quitMenu = false;

            do
            {
                DisplayHeader("View Command List");
                Console.WriteLine();
                Console.WriteLine("Please select a list to view.");
                Console.WriteLine();
                Console.WriteLine("1. " + commandListNames[0]);
                Console.WriteLine("2. " + commandListNames[1]);
                Console.WriteLine("3. " + commandListNames[2]);
                Console.WriteLine("4. " + commandListNames[3]);
                Console.WriteLine("5. Quit");

                menuKey = Console.ReadKey();
                Console.WriteLine();
                menuChoice = menuKey.KeyChar;
                if (menuChoice != '5' && menuChoice != 'e' && menuChoice != 'q')
                {
                    Console.WriteLine();
                    Console.WriteLine("This list contains the commands:");
                    Console.WriteLine();
                }
                switch (menuChoice)
                {
                    case '1':
                    case 'a':
                        if (commandList[0] != null)
                        {
                            foreach (Command command in commandList[0])
                            {
                                Console.Write(command + ", ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Command list is empty.");
                        }
                        DisplayContinuePrompt();
                        break;
                    case '2':
                    case 'b':
                        if (commandList[1] != null)
                        {
                            foreach (Command command in commandList[1])
                            {
                                Console.Write(command + ", ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Command list is empty.");
                        }
                        DisplayContinuePrompt();
                        break;
                    case '3':
                    case 'c':
                        if (commandList[2] != null)
                        {
                            foreach (Command command in commandList[2])
                            {
                                Console.Write(command + ", ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Command list is empty.");
                        }
                        DisplayContinuePrompt();
                        break;
                    case '4':
                    case 'd':
                        if (commandList[0] != null)
                        {
                            foreach (Command command in commandList[0])
                            {
                                Console.Write(command + ", ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Command list is empty.");
                        }
                        DisplayContinuePrompt();
                        break;
                    case '5':
                    case 'e':
                    case 'q':
                        quitMenu = true;
                        break;
                    default:
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// Displays the menu to choose which command list to execute.
        /// </summary>
        static void UPExecuteCommandList(Finch finchRobot, List<Command>[] commandList, string[] commandListNames,
            (int, int, int) commandParameters)
        {
            char menuChoice;
            ConsoleKeyInfo menuKey;
            bool quitMenu = false;

            do
            {
                DisplayHeader("View Command List");
                Console.WriteLine();
                Console.WriteLine("Please select a list to execute.");
                Console.WriteLine();
                Console.WriteLine("1. " + commandListNames[0]);
                Console.WriteLine("2. " + commandListNames[1]);
                Console.WriteLine("3. " + commandListNames[2]);
                Console.WriteLine("4. " + commandListNames[3]);
                Console.WriteLine("5. Quit");

                menuKey = Console.ReadKey();
                Console.WriteLine();
                menuChoice = menuKey.KeyChar;
                switch (menuChoice)
                {
                    case '1':
                    case 'a':
                        if (commandList[0] != null)
                        {
                            UPExecute(finchRobot, commandList[0], commandListNames[0], commandParameters);
                            DisplayContinuePrompt();
                            PlayOKBeep(finchRobot, 361, 1000);
                        }
                        else
                        {
                            Console.WriteLine("Command list is empty. No code to execute.");
                        }
                        break;
                    case '2':
                    case 'b':
                        if (commandList[1] != null)
                        {
                            UPExecute(finchRobot, commandList[1], commandListNames[1], commandParameters);
                            DisplayContinuePrompt();
                            PlayOKBeep(finchRobot, 361, 1000);
                        }
                        else
                        {
                            Console.WriteLine("Command list is empty. No code to execute.");
                        }
                        break;
                    case '3':
                    case 'c':
                        if (commandList[1] != null)
                        {
                            UPExecute(finchRobot, commandList[2], commandListNames[2], commandParameters);
                            DisplayContinuePrompt();
                            PlayOKBeep(finchRobot, 361, 1000);
                        }
                        else
                        {
                            Console.WriteLine("Command list is empty. No code to execute.");
                        }
                        break;
                    case '4':
                    case 'd':
                        if (commandList[1] != null)
                        {
                            UPExecute(finchRobot, commandList[3], commandListNames[3], commandParameters);
                            DisplayContinuePrompt();
                            PlayOKBeep(finchRobot, 361, 1000);
                        }
                        else
                        {
                            Console.WriteLine("Command list is empty. No code to execute.");
                        }
                        break;
                    case '5':
                    case 'e':
                    case 'q':
                        quitMenu = true;
                        break;
                    default:
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// Executes a command list, according to the commands it contains.
        /// </summary>
        static void UPExecute(Finch finchRobot, List<Command> commandList, string commandListName,
            (int motorSpeed, int ledBrightness, int waitMS) commandParameters)
        {
            bool doRepeats = false;
            DisplayHeader("Execute Command List");
            Console.WriteLine($"The finch is about to execute the command list \"{commandListName}\".");
            Console.WriteLine("Overview of command list:");
            Console.WriteLine();
            foreach (Command command in commandList)
            {
                Console.Write(command.ToString() + ", ");
            }
            DisplayContinuePrompt();

            do
            {
                foreach (Command command in commandList)
                {
                    switch (command)
                    {
                        case Command.MOVEFORWARD:
                            finchRobot.setMotors(commandParameters.motorSpeed, commandParameters.motorSpeed);
                            Console.WriteLine("Executing command \"MOVEFORWARD\"");
                            finchRobot.wait(commandParameters.waitMS);
                            break;
                        case Command.MOVEBACKWARD:
                            finchRobot.setMotors(-commandParameters.motorSpeed, -commandParameters.motorSpeed);
                            Console.WriteLine("Executing command \"MOVEBACKWARD\"");
                            finchRobot.wait(commandParameters.waitMS);
                            break;
                        case Command.STOPMOTORS:
                            finchRobot.setMotors(0, 0);
                            Console.WriteLine("Executing command \"STOPMOTORS\"");
                            break;
                        case Command.WAIT:
                            Console.WriteLine("Executing command \"WAIT\"");
                            finchRobot.wait(commandParameters.waitMS);
                            break;
                        case Command.TURNRIGHT:
                            Console.WriteLine("Executing command \"TURNRIGHT\"");
                            finchRobot.setMotors(commandParameters.motorSpeed, -commandParameters.motorSpeed);
                            finchRobot.wait(commandParameters.waitMS);
                            break;
                        case Command.TURNLEFT:
                            Console.WriteLine("Executing command \"TURNLEFT\"");
                            finchRobot.setMotors(-commandParameters.motorSpeed, commandParameters.motorSpeed);
                            finchRobot.wait(commandParameters.waitMS);
                            break;
                        case Command.LEDON:
                            Console.WriteLine("Executing command \"LEDON\"");
                            finchRobot.setLED(commandParameters.ledBrightness, commandParameters.ledBrightness, commandParameters.ledBrightness);
                            break;
                        case Command.LEDOFF:
                            Console.WriteLine("Executing command \"LEDOFF\"");
                            finchRobot.setLED(0, 0, 0);
                            break;
                        case Command.GETTEMP:
                            Console.WriteLine("Ambient temperature: {0:d2}", finchRobot.getTemperature());
                            break;
                        case Command.GETLIGHT:
                            Console.WriteLine("Light sensor readout: {0:d2)",
                                (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2);
                            break;
                        case Command.PLAYBEEP:
                            finchRobot.noteOn(361);
                            finchRobot.wait(commandParameters.waitMS * 1000);
                            finchRobot.noteOff();
                            break;
                        case Command.MARCH:
                            Console.WriteLine("Executing command \"MOVEFORWARD\"");
                            finchRobot.setLED(0, 255, 0);
                            for (int i = 0; i < 4; i++)
                            {
                                finchRobot.noteOn(361);
                                finchRobot.setMotors(commandParameters.motorSpeed, commandParameters.motorSpeed);
                                finchRobot.wait(commandParameters.waitMS / 4);
                                finchRobot.noteOff();
                                finchRobot.setMotors(0, 0);
                                finchRobot.wait(commandParameters.waitMS / 4);
                            }
                            break;
                        case Command.DONE:
                            Console.WriteLine("End of commands.");
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine("");
                doRepeats = GetBool("Would you like to repeat the command? [Y/N]");
            } while (doRepeats);

        }
        #endregion

        #region Persistence

        /// <summary>
        /// Overwrites the previous Commands.txt file with a new saved command list.
        /// </summary>
        /// <param name="Commands"></param>
        /// <param name="stringNames"></param>
        static void WriteCommandsData(List<Command>[] Commands, string[] stringNames)
        {
            DisplayHeader("Saving Commands List");
            Console.WriteLine("You are about to overwrite your saved command list.");
            if (GetBool("Would you like to continue? [Y/N]"))
            {
                string dataPath = @"Data\Commands.txt";
                List<string> writeStrings = new List<string>();
                StringBuilder sb = new StringBuilder();
                File.Delete(dataPath);
                for (int i = 0; i < 4; i++)
                {
                    foreach (Command command in Commands[i])
                    {
                        if (command == Command.DONE) sb = sb.Append(command.ToString());
                        if (command != Command.DONE) sb = sb.Append(command.ToString() + ", ");
                    }
                    writeStrings.Add(sb.ToString());
                    Console.WriteLine($"Wrote command list \"{sb.ToString()}\".");
                    sb = sb.Clear();
                }
                foreach (string strings in stringNames)
                {
                    writeStrings.Add(strings);
                    Console.WriteLine($"Wrote command name \"{strings}\".");
                }
                File.WriteAllLines(dataPath, writeStrings);
                Console.WriteLine("Write complete.");
                DisplayContinuePrompt();
            }

        }

        /// <summary>
        /// Reads from the Commands.txt file to overwrite the current command list.
        /// </summary>
        static (List<Command>[], string[]) ReadCommandsData()
        {

            List<Command>[] returnListArray = new List<Command>[4];
            for (int i = 0; i < 4; i++)
            {
                returnListArray[i] = new List<Command>();
            }
            string[] returnNames = new string[4];
            string[] splitStrings;
            string dataPath = @"Data\Commands.txt";
            string[] textLines = File.ReadAllLines(dataPath);
            Command commandElement;
            for (int i = 0; i < 4; i++)
            {
                splitStrings = textLines[i].Split(',');
                foreach (string command in splitStrings)
                {
                    Enum.TryParse(command, true, out commandElement);
                    returnListArray[i].Add(commandElement);
                    Console.WriteLine($"Built command {commandElement} into list #{i + 1}");
                }
            }
            for (int i = 4; i < 8; i++)
            {
                returnNames[i - 4] = textLines[i];
                Console.WriteLine($"Built name \"{textLines[i]}\" into list #{i - 4}");
            }
            Console.WriteLine("Read complete.");
            DisplayContinuePrompt();
            return (returnListArray, returnNames);
        }
        #endregion
    }
}
