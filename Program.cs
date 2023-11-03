using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ado1
{
    class Program
    {
        public void ReadData(SqlConnection conn, string com, int index1, int index2)
        {
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(com, conn);
                rdr = cmd.ExecuteReader();
                if (index2 != 0)
                {
                    while (rdr.Read())
                    {
                        Console.WriteLine(rdr[index1] + " " + rdr[index2]);
                    }
                }
                else
                {
                    while (rdr.Read())
                    {
                        Console.WriteLine(rdr[index1]);
                    }
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }

                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }
        public void ReadData2(SqlConnection conn, string com)
        {
            SqlDataReader rdr = null; ;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(com, conn);
                rdr = cmd.ExecuteReader();
                do
                {
                    while (rdr.Read())
                    {
                        Console.WriteLine(rdr[1]);
                    }
                    Console.WriteLine();
                } while (rdr.NextResult());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }
        public void Quant(int num, string com, SqlConnection conn)
        {
            char[] arr = com.ToArray();
            com.Replace(arr[62], Convert.ToChar(num));
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(com, conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("Кол-во книг: " + rdr[0]);
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }  // 62-й символ в запросе 
        static void Main(string[] args)
        {
            int num;
            string connect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connect);
            do
            {
                Console.Write("Введите номер задания: ");
                num = Convert.ToInt32(Console.ReadLine());
                Program program = new Program();
                switch (num)
                {
                    case 1:
                        string cmd1 = "SELECT * FROM Student, S_Cards WHERE Student.id = S_Cards.id_student AND S_Cards.date_in IS NULL;";
                        program.ReadData(connection, cmd1, 1, 2);
                        Console.WriteLine();
                        break;
                    case 2:
                        string cmd2 = "SELECT * FROM Author, Book WHERE Book.id = 3 AND Book.id_author = Author.id;";
                        program.ReadData(connection, cmd2, 1, 2);
                        Console.WriteLine();
                        break;
                    case 3:
                        string cmd3 = "SELECT * FROM Book b, S_Cards sc, T_Cards tc WHERE (b.id = sc.id_book OR b.id = tc.id_book) AND sc.date_in IS NOT NULL AND tc.date_in IS NOT NULL;";
                        program.ReadData(connection, cmd3, 1, 0);
                        Console.WriteLine();
                        break;
                    case 4:
                        string cmd4 = "SELECT b.name FROM Student s, S_Cards sc, Book b WHERE sc.id_book = b.id AND s.id = sc.id_student AND s.id = 2 AND sc.date_out IS NOT NULL;";
                        program.ReadData(connection, cmd4, 0, 0);
                        Console.WriteLine();
                        break;
                    case 5:
                        string cmd5 = "SELECT b.name FROM Student s, Teacher t, T_Cards tc, S_Cards sc, Book b WHERE s.id = sc.id_student AND t.id = tc.id_teacher AND b.id = sc.id_book AND b.id = tc.id_book AND(CAST(tc.date_out AS date) > '2019-04-21' AND CAST(tc.date_out AS date) > '2019-04-21');";
                        program.ReadData(connection, cmd5, 0, 0);
                        Console.WriteLine();
                        break;
                    case 6:
                        string cm6 = "SELECT * FROM Author, Book WHERE Book.id = 3 AND Book.id_author = Author.id; SELECT * FROM Book b, S_Cards sc, T_Cards tc WHERE (b.id = sc.id_book OR b.id = tc.id_book) AND sc.date_in IS NOT NULL AND tc.date_in IS NOT NULL;";
                        program.ReadData2(connection, cm6);
                        Console.WriteLine();
                        break;
                    case 7:
                        int n;
                        Console.Write("Введите id посетителя: ");
                        n = Convert.ToInt32(Console.ReadLine());
                        string cmd7 = "SELECT COUNT(b.id) FROM Student s, S_Cards sc, Book b WHERE s.id = 3 AND b.id = sc.id_book AND sc.id_student = s.id GROUP BY b.id;";
                        program.Quant(n, cmd7, connection);
                        Console.WriteLine();
                        break;
                }
            } while (num > 0 && num < 8);
            Console.WriteLine();
        }
    }
}