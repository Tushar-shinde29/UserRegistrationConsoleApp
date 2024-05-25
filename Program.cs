using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistrationConsoleApp
{
    class Program
    {
        public SqlConnection con = null;
        public int role;
        public void DbConnection()
        {
            try
            {
                // Creating Connection  
                con = new SqlConnection("Server=DESKTOP-T1G8TS2\\SQLEXPRESS01;Database=user_registration;Integrated Security=True");
            }
            catch (Exception e)
            {
                Console.WriteLine("OOPs, something went wrong.\n" + e.Message);
                Console.ReadKey();
            }

        }
        static void Main(string[] args)
        {
            int choice=1;
            Program p1 = new Program();
            while (choice < 3)
            {
                Console.WriteLine("welcome to user registration \n enter your choice : \n 1.for new user registration \n 2.for existing user login ");
                choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 1)
                {
                    Console.WriteLine("welcome for registration");
                    Console.WriteLine("enter firstname");
                    String fname = Console.ReadLine();
                    Console.WriteLine("enter lastname");
                    String lname = Console.ReadLine();
                    Console.WriteLine("enter username");
                    String uname = Console.ReadLine();
                    Console.WriteLine("enter email");
                    String email = Console.ReadLine();
                    Console.WriteLine("enter mobileno");
                    String mobileno = Console.ReadLine();
                    Console.WriteLine("enter password");
                    String password = Console.ReadLine();
                    Console.WriteLine("enter address");
                    String address = Console.ReadLine();
                    Console.WriteLine("enter city");
                    String city = Console.ReadLine();
                    Console.WriteLine("enter state");
                    String state = Console.ReadLine();
                    String salt = "abcd";

                    try
                    {
                        
                        p1.DbConnection();
                        p1.con.Open();
                        // writing sql query  
                        


                        SqlCommand cm1 = new SqlCommand("exec check_user @username,@email", p1.con);
                        cm1.Parameters.AddWithValue("@username", uname);
                        cm1.Parameters.AddWithValue("@email", email);
                        SqlDataReader data = cm1.ExecuteReader();
                        if (data.HasRows)
                        {
                            Console.WriteLine("user already exists");
                            data.Close();
                            p1.con.Close();                       
                        }
                        else
                        {
                            data.Close();
                            SqlCommand cm = new SqlCommand("exec insert_record @fname,@lname,@username,@email,@mobileno,@password,@salt,@address,@city,@state", p1.con);
                            cm.Parameters.AddWithValue("@fname", fname);
                            cm.Parameters.AddWithValue("@lname", lname);
                            cm.Parameters.AddWithValue("@username", uname);
                            cm.Parameters.AddWithValue("@email", email);
                            cm.Parameters.AddWithValue("@mobileno", mobileno);
                            cm.Parameters.AddWithValue("@password", password);
                            cm.Parameters.AddWithValue("@salt", salt);
                            cm.Parameters.AddWithValue("@address", address);
                            cm.Parameters.AddWithValue("@city", city);
                            cm.Parameters.AddWithValue("@state", state);

                            // Executing the SQL query  
                            int num = cm.ExecuteNonQuery();
                        if (num > 0)
                        {
                            Console.WriteLine("Your registration is Completed press 2 for login");
                            choice = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine(choice);
                        }
                        p1.con.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("OOPs, something went wrong.\n" + e.Message);
                        Console.ReadKey();
                    }
                }
                else if (choice == 2)
                {
                    Console.WriteLine("welcome to login");
                    Console.WriteLine("enter username");
                    String uname = Console.ReadLine();
                    Console.WriteLine("enter email");
                    String email = Console.ReadLine();
                    Console.WriteLine("enter password");
                    String password = Console.ReadLine();
                    String salt = "abcd";
                    try
                    {

                    p1.DbConnection();
                    p1.con.Open();
                    SqlCommand cm1 = new SqlCommand("exec login_user @username,@email,@password,@salt", p1.con);
                    cm1.Parameters.AddWithValue("@password", password);
                    cm1.Parameters.AddWithValue("@username", uname);
                    cm1.Parameters.AddWithValue("@email", email);
                    cm1.Parameters.AddWithValue("@salt", salt);
                    SqlDataReader data = cm1.ExecuteReader();
                    if (!data.HasRows)
                    {
                            Console.WriteLine("enter correct credientials");
                    }
                    else
                    {
                            if (data.Read())
                            {
                                p1.role = Convert.ToInt32(data["role"].ToString());
                            }
                            data.Close();
                            if (p1.role == 2)
                            {
                                Console.WriteLine("-----Hello Admin----------");
                                int adminchoice = 1;

                                while (adminchoice <= 4 && adminchoice > 0)
                                {
                                    Console.WriteLine("what do you want enter choice \n 1.show all users \n 2.update user details \n 3.delete user \n 4.log out");
                                    adminchoice = Convert.ToInt32(Console.ReadLine());
                                    if (adminchoice == 1)
                                    {
                                        SqlCommand cm2 = new SqlCommand("exec show_all_user", p1.con);
                                        SqlDataReader Userdata = cm2.ExecuteReader();
                                        while (Userdata.Read())
                                        {
                                            Console.WriteLine("User id : " + Userdata["id"]);
                                            Console.WriteLine("your name : " + Userdata["firstname"] + " " + Userdata["lastname"]);
                                            Console.WriteLine("your mobile no : " + Userdata["mobileno"]);
                                            Console.WriteLine("your email ID : " + Userdata["email"]);
                                            Console.WriteLine("------------------------------------------------------");
                                        }
                                        Userdata.Close();
                                    }
                                    else if (adminchoice == 2)
                                    {

                                        Console.Write("enter user id which you wnat to update");
                                        int userid = Convert.ToInt32(Console.ReadLine());
                                        SqlCommand cm4 = new SqlCommand("exec show_one_user @id", p1.con);
                                        cm4.Parameters.AddWithValue("@id", userid);
                                        SqlDataReader Userdata = cm4.ExecuteReader();
                                        if (Userdata.HasRows)
                                        {

                                            while (Userdata.Read())
                                            {
                                                Console.WriteLine("User id : " + Userdata["id"]);
                                                Console.WriteLine("your name : " + Userdata["firstname"] + " " + Userdata["lastname"]);
                                                Console.WriteLine("your mobile no : " + Userdata["mobileno"]);
                                                Console.WriteLine("your email ID : " + Userdata["email"]);
                                                Console.WriteLine("your Adress : " + Userdata["address"]);
                                                Console.WriteLine("Your City : " + Userdata["city"]);
                                                Console.WriteLine("Your state : " + Userdata["state"]);
                                                Console.WriteLine("------------------------------------------------------");
                                            }
                                            Userdata.Close();

                                            Console.WriteLine("for update enter details");
                                            Console.WriteLine("enter firstname");
                                            String updatefname = Console.ReadLine();
                                            Console.WriteLine("enter lastname");
                                            String updatelname = Console.ReadLine();
                                            Console.WriteLine("enter mobileno");
                                            String updatemobileno = Console.ReadLine();
                                            Console.WriteLine("enter address");
                                            String updateaddress = Console.ReadLine();
                                            Console.WriteLine("enter city");
                                            String updatecity = Console.ReadLine();
                                            Console.WriteLine("enter state");
                                            String updatestate = Console.ReadLine();
                                            

                                            SqlCommand cm3 = new SqlCommand("exec update_record @fname,@lname,@mobileno,@address,@city,@state,@id", p1.con);
                                            cm3.Parameters.AddWithValue("@fname", updatefname);
                                            cm3.Parameters.AddWithValue("@lname", updatelname);
                                            cm3.Parameters.AddWithValue("@mobileno", updatemobileno);
                                            cm3.Parameters.AddWithValue("@address", updateaddress);
                                            cm3.Parameters.AddWithValue("@city", updatecity);
                                            cm3.Parameters.AddWithValue("@state", updatestate);
                                            cm3.Parameters.AddWithValue("@id", userid);

                                            // Executing the SQL query  
                                            int num = cm3.ExecuteNonQuery();

                                            if (num > 0)
                                            {
                                                Console.WriteLine("data updated succesfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("not updated");
                                            }
                                        }
                                        else
                                        {
                                            Userdata.Close();
                                            Console.WriteLine("enter valid id");
                                        }
                                    }
                                    else if(adminchoice == 3)
                                    {
                                        Console.Write("enter user id which you wnat to delete");
                                        int userid = Convert.ToInt32(Console.ReadLine());
                                        SqlCommand cm5 = new SqlCommand("exec delete_record @id", p1.con);
                                        cm5.Parameters.AddWithValue("@id", userid);
                                        int num = cm5.ExecuteNonQuery();
                                        

                                        if (num > 0)
                                        {
                                            Console.WriteLine("data deleted successfully");
                                        }
                                        else
                                        {
                                            Console.WriteLine("not deleted");
                                        }
                                    }
                                    else 
                                    {
                                        Console.WriteLine("you logout the session (Press Enter)");
                                        Console.ReadKey();
                                        Environment.Exit(1);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("helloo user");
                                Console.WriteLine("-------------------Welcome-----------------------");
                                data.Close();
                                data = cm1.ExecuteReader();
                                while (data.Read())
                                {
                                    Console.WriteLine("your name : " + data["firstname"] + " " + data["lastname"]);
                                    Console.WriteLine("your mobile no : " + data["mobileno"]);
                                    Console.WriteLine("your email ID : " + data["email"]);
                                    Console.WriteLine("your Adress : " + data["address"]);
                                    Console.WriteLine("Your City : " + data["city"]);
                                    Console.WriteLine("Your state : " + data["state"]);
                                    Console.WriteLine("-----------------Thank you--------------------");
                                    Console.ReadKey();
                                    Environment.Exit(1);
                                }
                            }
                    }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("OOPs, something went wrong.\n" + e.Message);
                        Console.ReadKey();
                    }

                }
                else
                {
                    Console.WriteLine("you enter wrong choice so run again code Thank you (Press Enter)");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }

        }
    }
}
