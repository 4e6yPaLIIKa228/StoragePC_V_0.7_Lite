using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using YchetPer.Connection;
using System.Data;


namespace YchetPer
{
    /// <summary>
    /// Логика взаимодействия для AddTechnic.xaml
    /// </summary>
    public partial class AddTechnic : Window
    {
        DataTable dt1 = new DataTable("NumberKabs");
        public AddTechnic()
        {
            InitializeComponent();
            CbFill();
            //ComBoxKab();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CbFill();
            this.Close();

        }
        public void CbFill()  //Данные для комбобоксов 
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query1 = $@"SELECT * FROM Types"; // Типы
                    string query2 = $@"SELECT * FROM Conditions"; // Состояние
                    string query3 = $@"SELECT * FROM NumberKabs"; // Кабинеты
                    string query4 = $@"SELECT  Devices.Title FROM Devices"; // Названия
                    //----------------------------------------------
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, connection);
                    SQLiteCommand cmd4 = new SQLiteCommand(query4, connection);
                    //----------------------------------------------
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    SQLiteDataAdapter SDA3 = new SQLiteDataAdapter(cmd3);
                    SQLiteDataAdapter SDA4 = new SQLiteDataAdapter(cmd4);
                    //----------------------------------------------
                    DataTable dt1 = new DataTable("Types");
                    DataTable dt2 = new DataTable("Conditions");
                    DataTable dt3 = new DataTable("NumberKabs");
                    DataTable dt4 = new DataTable("Devices");
                    //----------------------------------------------
                    SDA1.Fill(dt1);
                    SDA2.Fill(dt2);
                    SDA3.Fill(dt3);
                    SDA4.Fill(dt4);
                    //----------------------------------------------

                    CbClass.ItemsSource = dt1.DefaultView;
                    CbClass.DisplayMemberPath = "Class";
                    CbClass.SelectedValuePath = "ID";
                    //----------------------------------------------
                    CbCondition.ItemsSource = dt2.DefaultView;
                    CbCondition.DisplayMemberPath = "Condition";
                    CbCondition.SelectedValuePath = "ID";
                    //----------------------------------------------
                    TbNumKab.ItemsSource = dt3.DefaultView;
                    TbNumKab.DisplayMemberPath = "NumKab";
                    TbNumKab.SelectedValuePath = "ID";
                    //////----------------------------------------------
                    //CbTitle.ItemsSource = dt4.DefaultView;
                    //CbTitle.DisplayMemberPath = "Title";
                    //CbTitle.SelectedValuePath = "ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

            private void BtnAdd_Click(object sender, RoutedEventArgs e) //Добавление
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                connection.Open();
                if (String.IsNullOrEmpty(TbTitle.Text) || String.IsNullOrEmpty(TbNumber.Text) || String.IsNullOrEmpty(StartWork.Text) || String.IsNullOrEmpty(CbClass.Text) || TbNumKab.SelectedIndex == -1 || CbCondition.SelectedIndex == -1)
                {
                    MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                int id, id2, id3;
                bool resultClass = int.TryParse(CbClass.SelectedValue.ToString(), out id);
                bool resultKab = int.TryParse(TbNumKab.SelectedValue.ToString(), out id2);
                bool resultCon = int.TryParse(CbCondition.SelectedValue.ToString(), out id3);
                var name = TbTitle.Text;
                var numkab = TbNumber.Text;
                var number = TbNumber.Text;
                var idtype = CbClass.Text;
                var idcon = CbCondition.Text;
                var startWork = StartWork.Text;

              
                    string query = $@"INSERT INTO Devices(IDType,IDKabuneta,Title,Number,IDCondition,StartWork) values ('{id}',{id2},'{name}','{number}','{id3}','{startWork}');";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Информация добавленна");
                        this.Close();
                    }

                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void BtnAddKab_Click(object sender, RoutedEventArgs e)
        {
            Eddit Edd = new Eddit();
            Edd.Owner = this;
            bool? result = Edd.ShowDialog();
            switch (result)
            {
                default:
                CbFill();
                break;
            }
        }

        private void BtnDellKab_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            CbFill();
        }
        public void Delete()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {


                if (TbNumKab.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите какой кабинет нужно удалить!!!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    
                    int IdKab;
                    bool NumKab = int.TryParse(TbNumKab.SelectedValue.ToString(), out IdKab);
                    try
                    {
                        string query1 = $@"DELETE FROM NumberKabs WHERE id =  '{IdKab}'";
                        connection.Open();
                        SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                        DataTable DT = new DataTable("NumberKabs");
                        cmd1.ExecuteNonQuery();
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }

                }
            }
        }  
    }
}
