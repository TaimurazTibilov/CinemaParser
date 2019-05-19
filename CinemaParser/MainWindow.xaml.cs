using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace CinemaParser
{
    public partial class MainWindow : Window
    {
        List<string> headerRow = new List<string>() { "ROWNUM", "CommonName","FullName","ShortName",
            "ChiefOrg","AdmArea", "District","Address","ChiefName","ChiefPosition","PublicPhone","Fax",
            "Email","WorkingHours","ClarificationOfWorkingHours","WebSite","OKPO","INN","NumberOfHalls",
            "TotalSeatsAmount","X_WGS","Y_WGS","GLOBALID"};
        string myPath = String.Empty;
        List<Area> allAreas = new List<Area>();
        ObservableCollection<Cinema> cinemas = new ObservableCollection<Cinema>();
        bool isSaved = true;

        public MainWindow()
        {
            InitializeComponent();
            Cinemas.ItemsSource = cinemas;
            cinemas.CollectionChanged += Cinemas_CollectionChanged;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFileSaved())
                return;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV file (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    cinemas.Clear();
                    Cinemas.ItemsSource = cinemas;
                    using (StreamReader myStream = new StreamReader(openFileDialog.OpenFile(),Encoding.UTF8))
                    {
                        int num = 0;
                        CSVParser.CSVReader reader = new CSVParser.CSVReader();
                        foreach (var row in reader.Read(myStream.ReadToEnd()))
                        {
                            if (!int.TryParse(row[0], out int numb))
                                continue;
                            num++;
                            Cinema cinema = new Cinema();
                            cinema.Rownum = num;
                            cinema.CommonName = row[1];
                            cinema.FullName = row[2];
                            cinema.ShortName = row[3];
                            cinema.ChiefOrg = row[4];
                            cinema.Area = new Area(row[5],row[6]);
                            cinema.AdmArea = row[5];
                            cinema.District = row[6];
                            cinema.Address = row[7];
                            cinema.ChiefName = row[8];
                            cinema.ChiefPosition = row[9];
                            cinema.PublicPhone = row[10];
                            cinema.Fax = row[11];
                            cinema.Email = row[12];
                            cinema.WorkingHours = row[13];
                            cinema.ClarificationOfWorkingHours = row[14];
                            cinema.WebSite = row[15];
                            cinema.Okpo = row[16];
                            cinema.Inn = row[17];
                            cinema.NumberOfHalls = row[18];
                            cinema.TotalSeatsAmount = row[19];
                            cinema.X_WGS = row[20];
                            cinema.Y_WGS = row[21];
                            cinema.GlobalID = row[22];
                            cinemas.Add(cinema);
                            WriteArea(row[5], row[6]);
                        }
                        Cinemas.ItemsSource = cinemas;
                        cinemas.CollectionChanged += Cinemas_CollectionChanged;
                        isSaved = true;
                        myPath = openFileDialog.FileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void Renumerate(ObservableCollection<Cinema> cinemas)
        {
            int num = cinemas.Count - 1;
            foreach (var cinema in cinemas)
            {
                cinema.Rownum = cinemas.Count - num--;
            }
        }        

        private void Cinemas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Renumerate(cinemas);
            foreach (var c in cinemas)
            {
                WriteArea(c.AdmArea, c.District);
            }
            isSaved = false;
        }

        private void CreateNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (IsFileSaved())
            {
                cinemas = new ObservableCollection<Cinema>();
                Cinemas.ItemsSource = cinemas;
                cinemas.CollectionChanged += Cinemas_CollectionChanged;
                myPath = string.Empty;
            }
        }

        private bool IsFileSaved()
        {
            if (isSaved)
                return true;
            if (MessageBox.Show("File is not saved. Do you want to continue?", "Warning!", MessageBoxButton.OKCancel) != MessageBoxResult.Cancel)
                return true;
            return false;
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (myPath == string.Empty)
            {
                SaveAs_Click(sender, e);
                return;
            }
            try
            {
                if (File.Exists(myPath))
                {
                    new FileInfo(myPath).Delete();
                }
                else
                {
                    if (MessageBox.Show("File is not found. Do you want to create new?", "Warning!", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                        return;
                }
                using (StreamWriter myStream = new StreamWriter(new FileStream(myPath, FileMode.OpenOrCreate), Encoding.UTF8))
                {
                    CSVParser.CSVWriter writer = new CSVParser.CSVWriter();
                    writer.AddRow(headerRow);
                    Renumerate((ObservableCollection<Cinema>)Cinemas.ItemsSource);
                    foreach (var cinema in (ObservableCollection<Cinema>)Cinemas.ItemsSource)
                    {
                        writer.AddRow(cinema.GetInfo());
                    }
                    myStream.Write(writer.Write());
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "CSV file (*.csv)|*.csv";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter myStream = new StreamWriter(saveFile.OpenFile(), Encoding.UTF8))
                    {
                        CSVParser.CSVWriter writer = new CSVParser.CSVWriter();
                        writer.AddRow(headerRow);
                        Renumerate((ObservableCollection<Cinema>)Cinemas.ItemsSource);
                        foreach (var cinema in (ObservableCollection<Cinema>)Cinemas.ItemsSource)
                        {
                            writer.AddRow(cinema.GetInfo());
                        }
                        myStream.Write(writer.Write());
                        isSaved = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void AddField_Click(object sender, RoutedEventArgs e)
        {
            cinemas.Add(new Cinema());
            Cinemas.ItemsSource = cinemas;
            isSaved = false;
        }

        private void SortingAreabyArea()
        {
            allAreas.Sort((x, y) =>
            {
                if (x.GetNumberOfDistricts() > y.GetNumberOfDistricts())
                    return 1;
                if (x.GetNumberOfDistricts() < y.GetNumberOfDistricts())
                    return -1;
                return 0;
            });
        }

        private void WriteArea(string area, string district)
        {
            foreach (var are in allAreas)
            {
                if (are.AdmArea == area)
                {
                    foreach (var dis in are.Districts)
                    {
                        if (dis == district)
                            return;
                    }
                    are.Districts.Add(district);
                    return;
                }
            }
            var ar = new Area(area, district);
            ar.Districts.Add(district);
            allAreas.Add(ar);
            return;
        }

        private void Sortbyarea_Click(object sender, RoutedEventArgs e)
        {
            isSaved = false;
            List<Cinema> cinem;            
            foreach (var cinema in cinemas)
            {
                WriteArea(cinema.AdmArea, cinema.District);
            }
            SortingAreabyArea();
            cinem = cinemas.ToList<Cinema>();
            cinem.Sort((x, y) =>
            {
                if (x.AdmArea == y.AdmArea)
                    return 0;
                else
                {
                    foreach (var ar in allAreas)
                    {
                        if (x.AdmArea == ar.AdmArea)
                            return 1;
                        if (y.AdmArea == ar.AdmArea)
                            return -1;
                    }
                }
                return -1;
            });
            cinemas = new ObservableCollection<Cinema>(cinem);
            Cinemas.ItemsSource = cinemas;
            cinemas.CollectionChanged += Cinemas_CollectionChanged;
            Dis.Text = string.Empty;
            Area.Text = string.Empty;
            number.Text = string.Empty;
        }

        private void Sortbyname_Click(object sender, RoutedEventArgs e)
        {
            isSaved = false;
            List<Cinema> cinem;           
            cinem = cinemas.ToList<Cinema>();
            cinem.Sort((x, y) =>
            {
                return (string.Compare(x.FullName, y.FullName));
            });
            cinemas = new ObservableCollection<Cinema>(cinem);
            Cinemas.ItemsSource = cinemas;
            cinemas.CollectionChanged += Cinemas_CollectionChanged;
            Dis.Text = string.Empty;
            Area.Text = string.Empty;
            number.Text = string.Empty;
        }

        private void Sortbydis_Click(object sender, RoutedEventArgs e)
        {
            isSaved = false;
            List<Cinema> cinem;                       
            cinem = cinemas.ToList<Cinema>();
            cinem.Sort((x, y) =>
            {
                return (string.Compare(x.District, y.District));
            });
            cinemas = new ObservableCollection<Cinema>(cinem);
            Cinemas.ItemsSource = cinemas;
            cinemas.CollectionChanged += Cinemas_CollectionChanged;
            Dis.Text = string.Empty;
            Area.Text = string.Empty;
            number.Text = string.Empty;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isSaved = false;
            uint num = 0;
            if (number.Text == string.Empty)
            {
                Dis.Text = string.Empty;
                Area.Text = string.Empty;
                Cinemas.ItemsSource = cinemas;
            }
            if (uint.TryParse(number.Text, out num) && (num <= cinemas.Count))
            {
                var c = new ObservableCollection<Cinema>();
                for (int i = 0; i < num; i++)
                {
                    c.Add(cinemas[i]);
                }
                Cinemas.ItemsSource = c;
                c.CollectionChanged += Cinemas_CollectionChanged;
            }
            else
            {
                number.Text = String.Empty;
            }
            Dis.Text = string.Empty;
            Area.Text = string.Empty;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            isSaved = false;
            number.Text = string.Empty;
            if ((Dis.Text == string.Empty) && (Area.Text == string.Empty))
            {
                Cinemas.ItemsSource = cinemas;
            }
            else
            {
                Cinemas.ItemsSource = IsContains(Area.Text, Dis.Text);
            }
            
        }

        private ObservableCollection<Cinema> IsContains(string area, string dist)
        {
            var c = new ObservableCollection<Cinema>();
            if (area == string.Empty)
            {
                foreach (var cin in cinemas)
                {
                    if ((cin.District.Length >= dist.Length) && cin.District.Substring(0, dist.Length).ToLower() == dist.ToLower())
                    {
                        c.Add(cin);
                    }
                }
                c.CollectionChanged += Cinemas_CollectionChanged;
                return c;
            }
            foreach (var cin in cinemas)
            {
                if ((cin.AdmArea.Length >= area.Length) && cin.AdmArea.Substring(0, area.Length).ToLower() == area.ToLower())
                {
                    if (dist == string.Empty)
                    {
                        c.Add(cin);
                        continue;
                    }
                    if ((cin.District.Length >= dist.Length) && cin.District.Substring(0, dist.Length).ToLower() == dist.ToLower())
                    {
                        c.Add(cin);
                    }
                }
            }
            c.CollectionChanged += Cinemas_CollectionChanged;
            return c;
        }

        private void Defaultsort_Click(object sender, RoutedEventArgs e)
        {
            List<Cinema> cinem;
            cinem = cinemas.ToList<Cinema>();
            cinem.Sort((x, y) =>
            {
                if (x.Rownum == 0)
                    return 1;
                if (y.Rownum == 0)
                    return -1;
                if (x.Rownum > y.Rownum)
                    return 1;
                if (x.Rownum < y.Rownum)
                    return -1;
                return 0;
            });
            cinemas = new ObservableCollection<Cinema>(cinem);
            Renumerate(cinemas);
            Cinemas.ItemsSource = cinemas;
            cinemas.CollectionChanged += Cinemas_CollectionChanged;
            Dis.Text = string.Empty;
            Area.Text = string.Empty;
            number.Text = string.Empty;
            return;
        }

        private void Addtofile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "CSV file (*.csv)|*.csv";
            saveFile.OverwritePrompt = false;
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter myStream = new StreamWriter(saveFile.FileName, true, Encoding.UTF8))
                    {
                        CSVParser.CSVWriter writer = new CSVParser.CSVWriter();
                        writer.AddRow(headerRow);
                        Renumerate((ObservableCollection<Cinema>)Cinemas.ItemsSource);
                        foreach (var cinema in (ObservableCollection<Cinema>)Cinemas.ItemsSource)
                        {
                            writer.AddRow(cinema.GetInfo());
                        }
                        myStream.Write(writer.Write());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}


/*
 private void DisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)DisList.SelectedItem == "Все")
            {
                AddToBox();
                return;
            }
            AreaList.SelectedItem = "Все";
            var c = new ObservableCollection<Cinema>();
            foreach (var cin in cinemas)
            {
                if (cin.District == (string)DisList.SelectedItem)
                    c.Add(cin);
            }
            Cinemas.ItemsSource = c;
            c.CollectionChanged += Cinemas_CollectionChanged;
            AddToBox();
        }

        private void AreaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)AreaList.SelectedItem == "Все")
            {
                AddToBox();
                return;
            }
            DisList.Items.Clear();
            foreach (var a in allAreas)
            {
                if (a.AdmArea == (string)AreaList.SelectedItem)
                {
                    foreach (var d in a.Districts)
                    {
                        DisList.Items.Add(d);
                    }
                }
            }
            var c = new ObservableCollection<Cinema>();
            foreach (var cin in cinemas)
            {
                if (cin.AdmArea == (string)AreaList.SelectedItem)
                    c.Add(cin);
            }
            Cinemas.ItemsSource = c;
            c.CollectionChanged += Cinemas_CollectionChanged;
            AddToBox();
        }
 */
