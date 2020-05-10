using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Security.Permissions;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace TicTacToe
{
    enum Symblos { X,O,EMPTY }

    abstract class IPlayer
    {
        public Symblos symb=new Symblos();
        public void chooseSymbol(int count) {
            List<string> X = new List<string>() { "x", "х", "крестик", "1" };
            List<string> O = new List<string>() { "o", "о", "нолик", "2" };
            Console.WriteLine($"Выберите символ за который будет играть игрок №{count} или оставьте всё как есть (p1-X p2-O)");
            string str = Console.ReadLine();
            if (X.Contains(str.ToLower())) { Console.WriteLine("Игрок теперь играет за крестик"); symb = Symblos.X; }
            else if (O.Contains(str.ToLower())) { Console.WriteLine("Игрок теперь играет за нолик"); symb = Symblos.O; }
        }
        public abstract void makeMove(ref Symblos[,] matr);
    }

    class Player : IPlayer
    {
        override public void makeMove(ref Symblos[,] matr)
        {
            int MatrSize = matr.GetLength(0);
            int pos1, pos2;
            string str;

            do
            {
                Console.WriteLine($"Введите номер строки в диапозоне от 1 до {MatrSize}");
                str = Console.ReadLine();
                if (!int.TryParse(str, out pos1)) { Console.WriteLine("Недопустимое значение номера строки"); continue; }
            } while (!int.TryParse(str, out pos1)||int.Parse(str) < 1 || int.Parse(str) > MatrSize);
            pos1 = int.Parse(str)-1;
            
            do
            {
                Console.WriteLine($"Введите номер столбца в диапозоне от 1 до {MatrSize}");
                str = Console.ReadLine();
                if (!int.TryParse(str, out pos2)) { Console.WriteLine("Недопустимое значение номера столбца"); continue; }
            } while (!int.TryParse(str, out pos2)|| int.Parse(str) < 1 || int.Parse(str) > MatrSize);
            pos2 = int.Parse(str)-1;

            while (matr[pos1, pos2] != Symblos.EMPTY)
            {
                Console.WriteLine("Эта ячейка уже занята, введите другую");
                do
                {
                    Console.WriteLine($"Введите номер строки в диапозоне от 1 до {MatrSize}");
                    str = Console.ReadLine();
                    if (!int.TryParse(str, out pos1)) { Console.WriteLine("Недопустимое значение номера строки"); continue; }
                } while (!int.TryParse(str, out pos1)||
                int.Parse(str) < 1 ||
                int.Parse(str) > MatrSize);
                pos1 = int.Parse(str)-1;

                do
                {
                    Console.WriteLine($"Введите номер столбца в диапозоне от 1 до {MatrSize}");
                    str = Console.ReadLine();
                    if (!int.TryParse(str, out pos2)||str=="") { Console.WriteLine("Недопустимое значение номера столбца"); continue; }
                } while (!int.TryParse(str, out pos1)||int.Parse(str) < 1 || int.Parse(str) > MatrSize);
                pos2 = int.Parse(str)-1;

            }

            matr[pos1, pos2] = symb;
        }
    }

    class Bot : IPlayer
    {
        override public void makeMove(ref Symblos[,] matr)
        {
            int MatrSize = matr.GetLength(0);
            int pos1, pos2;
            Random r = new Random();
            do
            {
                pos1 = r.Next(0, MatrSize);
                pos2 = r.Next(0, MatrSize);
                //Console.WriteLine($"{pos1}, {pos2}"); Console.ReadKey();
            } while (matr[pos1, pos2] != Symblos.EMPTY);

            matr[pos1, pos2] = symb;
           // Console.Write(matr[pos1, pos2] == Symblos.X ? "┃ Х ┃" : matr[pos1, pos2] == Symblos.O ? "┃ O ┃" : "┃   ┃"); Console.ReadKey();
        }

    /*    new public void chooseSymbol()
        {
            List<string> X = new List<string>() {"x", "х", "крестик","1"};
            List<string> O = new List<string>() {"o", "о", "нолик","2" };
            Console.WriteLine("Выберите символ за который будет играть игрок или предоставьте волю случаю");
            string str = Console.ReadLine();
            if (X.Contains(str.ToLower())) { Console.WriteLine("Игрок теперь играет за крестик"); symb = Symblos.X; }
            else if (O.Contains(str.ToLower())) { Console.WriteLine("Игрок теперь играет за нолик"); symb = Symblos.O; }
        }*/
    }

    class Game
    {
        IPlayer pl1, pl2;
        public Symblos[,] GameTable;
        int LengthOfLine;
        public string status;
        List<Iobserver> l = new List<Iobserver>();

        public void AddObs(Iobserver obs) { l.Add(obs); }
        public void NoticeAll() { foreach (Iobserver obs in l) { obs.notice(this.status); } }

        public Game()
        {

        }
        public Game(IPlayer p1, IPlayer p2, List<Iobserver> LO)
        {
            l = LO;
            if (true)
            {
                Console.WriteLine("\n\n");
                Console.WriteLine("\t\t╔════╗╔══╗╔═══╗╔════╗╔═══╗╔═══╗╔════╗╔═══╗╔═══╗");
                Console.WriteLine("\t\t║╔╗╔╗║╚╗╔╝║╔═╗║║╔╗╔╗║║╔═╗║║╔═╗║║╔╗╔╗║║╔═╗║║╔══╝");
                Console.WriteLine("\t\t╚╝║║╚╝ ║║ ║║ ╚╝╚╝║║╚╝║║ ║║║║ ╚╝╚╝║║╚╝║║ ║║║╚══╗");
                Console.WriteLine("\t\t  ║║   ║║ ║║ ╔╗  ║║  ║╚═╝║║║ ╔╗  ║║  ║║ ║║║╔══╝");
                Console.WriteLine("\t\t  ║║  ╔╝╚╗║╚═╝║  ║║  ║╔═╗║║╚═╝║  ║║  ║╚═╝║║╚══╗");
                Console.WriteLine("\t\t  ╚╝  ╚══╝╚═══╝  ╚╝  ╚╝─╚╝╚═══╝  ╚╝  ╚═══╝╚═══╝");
            }
            
            pl1 = p1; pl2 = p2;

            int MatrSize;
            string str;
            do {
                Console.WriteLine($"Введите размер игрового поля");
                str = Console.ReadLine();
                if (!int.TryParse(str, out MatrSize)) { Console.WriteLine("Недопустимое значение размера матрицы"); }
            } while (!int.TryParse(str, out MatrSize)||int.Parse(str) < 3);
            MatrSize = int.Parse(str);
            GameTable = new Symblos[MatrSize,MatrSize];
            NullGameTable();

            do
            {
                Console.WriteLine("Введите длину линии, не превышающую размер поля и не меньше трёх");
                str = Console.ReadLine();
                if (!int.TryParse(str, out LengthOfLine)) { Console.WriteLine("Недопустимый формат ввода"); }
            } while (!int.TryParse(str, out LengthOfLine)||LengthOfLine>MatrSize||LengthOfLine<3);

            
            status = "CREATED";
            NoticeAll();
            Console.Clear();
        }
       

        private void NullGameTable()
        {
            int MatrixSize = GameTable.GetLength(0);
            
            int i = 0, i0 = 0;
            while (i < MatrixSize)
            {
                while(i0 < MatrixSize)
                {
                    //Console.WriteLine($"i0={i0}");
                    GameTable[i, i0] = Symblos.EMPTY;
                    i0++;
                }i0 = 0;
                i++;
            }
        }

        private bool GameTableIsFull()
        {
            int i = 0, i0 = 0;
            int MatrixSize = GameTable.GetLength(0);
            while (i < MatrixSize)
            {
                while (i0 < MatrixSize)
                {
                    if (GameTable[i, i0] == Symblos.EMPTY) { return false; }
                    i0++;
                }i0 = 0;
                i++;
            }return true;
        }

        private Symblos IsAnyWin ()
        {
            int i = 0, i0 = 0;
            int MatrixSize = GameTable.GetLength(0);
            //horizontal
            while (i<MatrixSize)
            {
                while (i0 < MatrixSize-LengthOfLine+1)
                {
                    if (GameTable[i, i0] != Symblos.EMPTY) { int chkLine = 0;

                        while (chkLine!=LengthOfLine)
                        {
                            if (GameTable[i, i0 + chkLine] != GameTable[i, i0]) { break; }
                            chkLine++;
                        }
                        if (chkLine == LengthOfLine) { return GameTable[i, i0]; }
                    }
                    i0++;
                }i0 = 0;
                i++;
            }i = 0;

            //vertical
            while (i0 < MatrixSize)
            {
                while (i < MatrixSize - LengthOfLine + 1)
                {
                    if (GameTable[i, i0] != Symblos.EMPTY)
                    { int chkLine = 0;
                        while (chkLine != LengthOfLine)
                        {
                            if (GameTable[i + chkLine, i0] != GameTable[i, i0]) { break; }
                            chkLine++;
                        }
                        if (chkLine == LengthOfLine) { return GameTable[i, i0]; }
                    }
                    i++;
                }i = 0;
                i0++;
            }i0 = 0;

            //main diagonal
            while (i < MatrixSize - LengthOfLine+1)
            {
                while (i0 < MatrixSize - LengthOfLine + 1)
                {
                    if (GameTable[i, i0] != Symblos.EMPTY)
                    {
                        int chkLine = 0;
                        while (chkLine != LengthOfLine)
                        {
                            if (GameTable[i + chkLine, i0 + chkLine] != GameTable[i, i0]) { break; }
                            chkLine++;
                        }

                        if (chkLine == LengthOfLine) { return GameTable[i, i0]; }
                    }
                    
                    i0++;
                }i0 = 0;
                i++;
            }i = 0;

            //side diagonal
            while (i < MatrixSize - LengthOfLine+1)
            {
                while (MatrixSize-i0 > /*MatrixSize -*/ LengthOfLine-1)
                {
                    if (GameTable[i, MatrixSize- i0-1] != Symblos.EMPTY)
                    {
                        int chkLine = 0;
                        while (chkLine != LengthOfLine)
                        {
                            if (GameTable[i + chkLine, MatrixSize - i0-1 - chkLine] != GameTable[i,MatrixSize - i0-1]) { break; }
                            chkLine++;
                        }
                        if (chkLine == LengthOfLine) { return GameTable[i, MatrixSize- i0-1]; }
                    }
                    i0++;
                }
                i0 = 0;
                i++;
            }
            i = 0;

            return Symblos.EMPTY;
        }

        private void PrintTable()
        {
            Console.Clear();
            int MatrixSize = GameTable.GetLength(0);
            int i = 0, i0 = 0;
            while (i < MatrixSize)
            {
                while (i0 < MatrixSize)
                {
                    Console.Write("╭━━━╮");
                    i0++;
                }i0 = 0;
                Console.WriteLine();
                while (i0 < MatrixSize)
                {

                    // Console.Write(GameTable[i,i0]==Symblos.X? "┃ "+"Х"+" ┃": GameTable[i,i0]==Symblos.O? "┃ O ┃": "┃   ┃");
                    if (GameTable[i, i0] == Symblos.X)
                    {
                        Console.Write("┃ ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        Console.ResetColor();
                        Console.Write(" ┃");
                    }
                    else if (GameTable[i, i0] == Symblos.O)
                    {
                        Console.Write("┃ ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("O");
                        Console.ResetColor();
                        Console.Write(" ┃");
                    }
                    else { Console.Write("┃   ┃"); }
                    i0++;
                }
                i0 = 0;
                Console.WriteLine();
                while (i0 < MatrixSize)
                {
                    Console.Write("╰━━━╯");
                    i0++;
                }
                i0 = 0;
                Console.WriteLine();
                i++;
            }i = 0;
        }

        public void StartGame()
        {
            status = "ID~T IGRA";
            NoticeAll();
            while (IsAnyWin()==Symblos.EMPTY)
            {
                if (GameTableIsFull()) { break; } if (IsAnyWin() != Symblos.EMPTY) { break; }
                PrintTable(); if (pl1.symb == Symblos.X) { Console.ForegroundColor = ConsoleColor.Red; } else { Console.ForegroundColor = ConsoleColor.Blue; }
                pl1.makeMove(ref GameTable); Console.ResetColor();
                
                if (GameTableIsFull()) { break; } if (IsAnyWin() != Symblos.EMPTY) { break; }
                PrintTable(); if (pl2.symb == Symblos.X) { Console.ForegroundColor = ConsoleColor.Red; } else { Console.ForegroundColor = ConsoleColor.Blue; }
                pl2.makeMove(ref GameTable); Console.ResetColor();

            }

            PrintTable();
            if (IsAnyWin() == Symblos.X) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("\n\tПобедили крестики."); Console.ResetColor(); Console.ReadKey(); status = "FINISH"; NoticeAll(); Program.Ins(true,false); return;  }
            if (IsAnyWin() == Symblos.O) { Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine("Победили нолики."); Console.ResetColor(); Console.ReadKey(); status = "FINISH"; NoticeAll(); Program.Ins(false, true); return; }
            if (GameTableIsFull()) { Console.WriteLine("Поле полностью заполнено и сделать ход невозможно. Ничья.");Console.ReadKey(); status = "FINISH"; NoticeAll(); Program.Ins(true, true); return; }


        }
    }









    class Program
    {
        private static void Ins(IPlayer p1, IPlayer p2)
        {
            string connStr = "server=localhost;user=root;database=gamedb;password=1111;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();



            string types = "";
            if (p1.symb == Symblos.X)
            {
                if (p1 is Bot) { types += "'bot', "; } else { types += "'player', "; }
                if (p2 is Bot) { types += "'bot'"; } else { types += "'player'"; }
            }
            else
            {
                if (p2 is Bot) { types += "'bot', "; } else { types += "'player', "; }
                if (p1 is Bot) { types += "'bot'"; } else { types += "'player'"; }
            }
            string req = "SELECT MAX(id) FROM game";
            MySqlCommand command2 = new MySqlCommand(req, conn);
            string num = command2.ExecuteScalar().ToString();
            int id;
            if (num == "") { id = 0; }
            else
            {
                id = int.Parse(num);
            }
            string query = $"INSERT INTO game (id, playerX, playerO) VALUES ({id + 1}, " + types + ")";
            MySqlCommand command1 = new MySqlCommand(query, conn);
            command1.ExecuteNonQuery();

            Console.Clear();




            conn.Close();
        }

        public static void Ins(bool X_win, bool O_win)
        {
            string connStr = "server=localhost;user=root;database=gamedb;password=1111;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();


            string req = "SELECT MAX(id) FROM game";
            MySqlCommand command2 = new MySqlCommand(req, conn);
            int idgame = int.Parse(command2.ExecuteScalar().ToString());
            string query = $"INSERT INTO game_stats (idgame, playerX, playerO) VALUES ({idgame + 1}, ";
            if (X_win == O_win) { query += "'draw', 'draw')"; } else
            {
                if (X_win) { query += "'win', 'lose')"; } else { query += "'lose', 'win')"; }
            }
            MySqlCommand command1 = new MySqlCommand(query, conn);
            command1.ExecuteNonQuery();

            Console.Clear();




            conn.Close();
        }
        private static string Menu()
        {
            if (true)
            {
                Console.WriteLine("\n\n");
                Console.WriteLine("\t\t╔════╗╔══╗╔═══╗╔════╗╔═══╗╔═══╗╔════╗╔═══╗╔═══╗");
                Console.WriteLine("\t\t║╔╗╔╗║╚╗╔╝║╔═╗║║╔╗╔╗║║╔═╗║║╔═╗║║╔╗╔╗║║╔═╗║║╔══╝");
                Console.WriteLine("\t\t╚╝║║╚╝ ║║ ║║ ╚╝╚╝║║╚╝║║ ║║║║ ╚╝╚╝║║╚╝║║ ║║║╚══╗");
                Console.WriteLine("\t\t  ║║   ║║ ║║ ╔╗  ║║  ║╚═╝║║║ ╔╗  ║║  ║║ ║║║╔══╝");
                Console.WriteLine("\t\t  ║║  ╔╝╚╗║╚═╝║  ║║  ║╔═╗║║╚═╝║  ║║  ║╚═╝║║╚══╗");
                Console.WriteLine("\t\t  ╚╝  ╚══╝╚═══╝  ╚╝  ╚╝─╚╝╚═══╝  ╚╝  ╚═══╝╚═══╝");
            }
            List<string> Commands = new List<string>() { "play","exit" };
            string str = "";
            do
            {
                Console.WriteLine("\nВведите 'play', чтобы начать или 'exit' для выхода");
                str = Console.ReadLine();
            } while (!Commands.Contains(str));
            return str;

        }
        static void Main(string[] args)
        {











            ///////////////////////////////
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.Clear();
                string command = Menu();
                if (command == "exit") { Console.WriteLine("\n\t\t\t\tдо свидания"); Console.ReadKey(); return; }
                if (true)
                {
                    string typePlayer;
                    IPlayer pl1, pl2;
                    do
                    {
                        Console.WriteLine("Кто будет первым игроком? bot или player");
                        typePlayer = Console.ReadLine();
                    } while (typePlayer.ToLower() != "bot" && typePlayer.ToLower() != "player");
                    if (typePlayer.ToLower() == "bot") { pl1 = new Bot(); } else { pl1 = new Player(); }
                    do
                    {
                        Console.WriteLine("Кто будет вторым игроком? bot или player");
                        typePlayer = Console.ReadLine();
                    } while (typePlayer.ToLower() != "bot" && typePlayer.ToLower() != "player");
                    if (typePlayer.ToLower() == "bot") { pl2 = new Bot(); } else { pl2 = new Player(); }
                    pl1.symb = Symblos.X; pl2.symb = Symblos.O;
                    pl1.chooseSymbol(1); pl2.chooseSymbol(2);
                    while (pl1.symb == pl2.symb) { Console.WriteLine("Двое игроков имеют одинаковые знакие для игры, что недопустимо."); pl1.chooseSymbol(1); pl2.chooseSymbol(2); }
                    Ins(pl1, pl2);

                    List<Iobserver> l = new List<Iobserver>();
                    Iobserver obs = new observer();
                    l.Add(obs);
                    Game g = new Game(pl1, pl2,l);
                    




                    g.StartGame();

                }
            }
        }
    }
}

