using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
//program
namespace TicTacToe
{
    class Program
    {
        static void DrawBoard(char[,] board, player player1, player player2)
        {
            string score = player1.Name + " " + player1.Score + "/" + player2.Score + " " + player2.Name;
            Console.Clear();
            Console.WriteLine(score);
            Console.WriteLine();
            for (int raw = 0; raw < 3; raw++)
                Console.WriteLine(board[raw, 0] + " | " + board[raw, 1] + " | " + board[raw, 2]);
        }
        static bool Repeat()
        {
            while (true)
            {
                Console.WriteLine("Play again?(y/n)");
                string repeat = Console.ReadLine();
                if (repeat == "n" || repeat == "N")
                    return false;
                else
                    return true;
            }
        }
        static bool IsValidMove(int move, char[,] board)
        {
            move -= 1;
            int col = move % 3;
            int raw = move / 3;
            if (board[raw, col] == ' ')
                return true;
            else
                return false;
        }
        static bool IsTie(char[,] board)
        {
            for (int raw = 0; raw < 3; raw++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[raw, col] == ' ')
                        return false;
                }
            }
            return true;
        }
        static bool HasWon(char[,] board, char player)
        {
            for (int num = 0; num < 3; num++)
            {
                if (board[num, 0] == player && board[num, 1] == player && board[num, 2] == player)
                    return true;
                if (board[0, num] == player && board[1, num] == player && board[2, num] == player)
                    return true;
            }
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
                return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
                return true;
            else
                return false;
        }
        static char[,] GetMove(ref char[,] board, int move, char symbol)
        {
            move -= 1;
            int col = move % 3;
            int raw = move / 3;
            board[raw, col] = symbol;
            return board;
        }
        static (int, bool) BestMove(char[,] board, char sign)
        {
            int move = 0;
            for (int num = 0; num < 2; num++)
            {
                for (int num2 = 0; num2 < 3; num2++)
                {
                    if (board[num, num2] == sign && board[num + 1, num2] == sign)
                    {
                        move = 7 + num2 - num * 6;
                        if (IsValidMove(move, board))
                            return (move, true);
                    }
                    if (board[num2, num] == sign && board[num2, num + 1] == sign)
                    {
                        move = (num2 + 1) * 3 - 2 * num;
                        if (IsValidMove(move, board))
                            return (move, true);
                    }
                }
            }
            for (int num = 0; num < 3; num++)
            {
                if (board[0, num] == sign && board[2, num] == sign)
                {
                    move = 4 + num;
                    if (IsValidMove(move, board))
                        return (move, true);
                }
                if (board[num, 0] == sign && board[num, 2] == sign)
                {
                    move = 2 + num * 3;
                    if (IsValidMove(move, board))
                        return (move, true);
                }
            }
            if (board[1, 1] == sign && board[2, 0] == sign)
            {
                move = 1;
                if (IsValidMove(move, board))
                    return (move, true);
            }
            if (board[1, 1] == sign && board[2, 2] == sign)
            {
                move = 3;
                if (IsValidMove(move, board))
                    return (move, true);
            }
            if (board[0, 0] == sign && board[2, 2] == sign)
            {
                move = 5;
                if (IsValidMove(move, board))
                    return (move, true);
            }
            if (board[0, 2] == sign && board[2, 0] == sign)
            {
                move = 5;
                if (IsValidMove(move, board))
                    return (move, true);
            }
            if (board[0, 2] == sign && board[1, 1] == sign)
            {
                move = 7;
                if (IsValidMove(move, board))
                    return (move, true);
            }
            if (board[0, 0] == sign && board[1, 1] == sign)
            {
                move = 9;
                if (IsValidMove(move, board))
                    return (move, true);
            }
            return (move, false);
        }
        static int BotMove(ref char[,] board, char sign)
        {
            int move = 0;
            bool bestmove = false;
            if (sign == 'O')
            {
                (move, bestmove) = BestMove(board, 'O');
                if (!bestmove)
                {
                    (move, bestmove) = BestMove(board, 'X');
                }
                else
                    return move;
                if (bestmove)
                    return move;
            } else
            {
                (move, bestmove) = BestMove(board, 'X');
                if (!bestmove)
                {
                    (move, bestmove) = BestMove(board, 'O');
                }
                else
                    return move;
                if (bestmove)
                    return move;
            }
            while (true)
            {
                Random rand = new Random();
                move = rand.Next(1, 10);
                if (IsValidMove(move, board))
                    return move;
            }
        }
        static bool IsGameOver(char[,] board, char player, player player1, player player2)
        {
            if (HasWon(board, player))
            {

                if (player == 'X')
                {
                    player1.Score += 1;
                    DrawBoard(board, player1, player2);
                    Console.WriteLine("Congratulations!!!");
                    Console.WriteLine(player1.Name + " won the game");
                    return true;
                }
                else if (player == 'O')
                {
                    player2.Score += 1;
                    DrawBoard(board, player1, player2);
                    Console.WriteLine("Congratulations!!!");
                    Console.WriteLine(player2.Name + " won the game");
                    return true;
                }
            }
            if (IsTie(board))
            {
                Console.WriteLine("It's tie");
                return true;
            }
            return false;
        }
        static int IsPlayerMoveValid(char[,] board, player player1, player player2, string name)
        {
            while (true)
            {
                Console.WriteLine(name);
                if (int.TryParse(Console.ReadLine(), out int move))
                {
                    if (move <= 9 && move >= 0.1)
                    {
                        if (IsValidMove(move, board))
                            return move;
                        else
                        {
                            DrawBoard(board, player1, player2);
                            Console.WriteLine("This space is already taken");
                        }
                    }
                    else
                    {
                        DrawBoard(board, player1, player2);
                        Console.WriteLine("Invalid cell number, try again");
                    }
                }
                else
                {
                    DrawBoard(board, player1, player2);
                    Console.WriteLine("Invalid cell number, try again ");
                }
            }
        }
        class player
        {
            public string Name { get; set; }
            public int Score { get; set; }
            public char Sign { get; set; }
            public player(string name, int score, char sign)
            {
                Name = name;
                Score = score;
                Sign = sign;
            }
        }
        static void Main(string[] args)
        {
            Random rand = new Random();
            int symbol = rand.Next(1, 3);
            player player1 = new player("Player 1", 0, 'X');
            player player2 = new player("Player 2", 0, 'O');
            string mode = "";
            int move = -1;
            char[,] ExampleBoard =
                {
                    {'1', '2', '3'},
                    {'4', '5', '6'},
                    {'7', '8', '9'},
                };
            DrawBoard(ExampleBoard, player1, player2);
            Console.WriteLine("Welcome to the Tic-Tac-Toe game, this is how the board look like");
            Console.WriteLine("Do you want to play versus friend or bot?(f/b)");
            while (true)
            {
                mode = Console.ReadLine();
                if (mode == "f" || mode == "b")
                    break;
                else
                {
                    DrawBoard(ExampleBoard, player1, player2);
                    Console.WriteLine("Welcome to the Tic-Tac-Toe game, this is how the board look like");
                    Console.WriteLine("Incorrect input, f/b?");
                }
            }
            if (mode == "f")
            {
                if (symbol == 1)
                {
                    Console.Write("Player's 1 name: ");
                    player1 = new player(Console.ReadLine(), 0, 'X');
                    Console.Write("Player's 2 name: ");
                    player2 = new player(Console.ReadLine(), 0, 'O');
                }
                else
                {
                    Console.Write("Player's 1 name: ");
                    player2 = new player(Console.ReadLine(), 0, 'O');
                    Console.Write("Player's 2 name: ");
                    player1 = new player(Console.ReadLine(), 0, 'X');
                }
                while (true)
                {
                    char[,] board =
                    {
                        {' ', ' ', ' '},
                        {' ', ' ', ' '},
                        {' ', ' ', ' '},
                    };
                    while (true)
                    {
                        DrawBoard(board, player1, player2);
                        move = IsPlayerMoveValid(board, player1, player2, player1.Name);
                        GetMove(ref board, move, player1.Sign);
                        DrawBoard(board, player1, player2);
                        if (IsGameOver(board, player1.Sign, player1, player2))
                            break;
                        DrawBoard(board, player1, player2);
                        if (IsGameOver(board, player2.Sign, player1, player2))
                            break;
                        move = IsPlayerMoveValid(board, player1, player2, player2.Name);
                        GetMove(ref board, move, player2.Sign);
                        DrawBoard(board, player1, player2);
                        if (IsGameOver(board, player2.Sign, player1, player2))
                            break;
                        DrawBoard(board, player1, player2);
                    }
                    symbol = rand.Next(1, 3);
                    if (!Repeat())
                        break;
                }
            }
            else if (mode == "b")
            {
                if (symbol == 1)
                {
                    Console.Write("Your name: ");
                    player1 = new player(Console.ReadLine(), 0, 'X');
                    player2 = new player("Bot", 0, 'O');
                }
                else
                {
                    Console.Write("Your name: ");
                    player2 = new player(Console.ReadLine(), 0, 'O');
                    player1 = new player("Bot", 0, 'X');
                }
                while (true)
                {
                    char[,] board =
                    {
                        {' ', ' ', ' '},
                        {' ', ' ', ' '},
                        {' ', ' ', ' '},
                    };
                    if (player1.Name == "Bot")
                    {
                        while (true)
                        {
                            DrawBoard(board, player1, player2);
                            move = BotMove(ref board, player1.Sign);
                            GetMove(ref board, move, player1.Sign);
                            DrawBoard(board, player1, player2);
                            if (IsGameOver(board, player1.Sign, player1, player2))
                                break;
                            DrawBoard(board, player1, player2);
                            if (IsGameOver(board, player2.Sign, player1, player2))
                                break;
                            move = IsPlayerMoveValid(board, player1, player2, player2.Name);
                            GetMove(ref board, move, player2.Sign);
                            DrawBoard(board, player1, player2);
                            if (IsGameOver(board, player2.Sign, player1, player2))
                                break;
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            DrawBoard(board, player1, player2);
                            move = IsPlayerMoveValid(board, player1, player2, player1.Name);
                            GetMove(ref board, move, player1.Sign);
                            DrawBoard(board, player1, player2);
                            if (IsGameOver(board, player1.Sign, player1, player2))
                                break;
                            DrawBoard(board, player1, player2);
                            if (IsGameOver(board, player2.Sign, player1, player2))
                                break;
                            move = BotMove(ref board, player2.Sign);
                            GetMove(ref board, move, player2.Sign);
                            DrawBoard(board, player1, player2);
                            if (IsGameOver(board, player2.Sign, player1, player2))
                                break;
                        }
                    }
                    if (!Repeat())
                        break;
                }
            }
            Console.ReadKey();
        }
    }
}