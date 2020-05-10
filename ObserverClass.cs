using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace TicTacToe
{
    abstract class Iobserver
    {
        public abstract void notice(string s);
    }


    class observer : Iobserver
    {
        override public void notice(string statuss)
        {
            string connStr = "server=localhost;user=root;database=gamedb;password=1111;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

    
            string req = "SELECT MAX(idstatus) FROM game_status_log",requ= "SELECT MAX(id) FROM game";
            MySqlCommand command2 = new MySqlCommand(req, conn), command3 = new MySqlCommand(requ, conn);
            int id, idgame;
            if (command2.ExecuteScalar().ToString() == "") { id = 0; }
            else
            {
                id = int.Parse(command2.ExecuteScalar().ToString());
            }
            idgame = int.Parse(command3.ExecuteScalar().ToString());
            string query = $"INSERT INTO game_status_log (idstatus, idgame, status) VALUES ({id + 1}, {idgame}, '{statuss}')";
            MySqlCommand command1 = new MySqlCommand(query, conn);
            command1.ExecuteNonQuery();

            Console.Clear();


            conn.Close();
        }
    }
    class fakeobserver : Iobserver
    {
        override public void notice(string s) { /*Console.WriteLine("01010101010"); Console.ReadKey(); */}
    }
}
