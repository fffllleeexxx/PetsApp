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

namespace PetsApp
{
    public partial class MainWindow : Window
    {
        private Users currentUser;
        private IQueryable<Pet> petsQuery;

        public MainWindow(Users user)
        {
            InitializeComponent();
            currentUser = user;
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new PetsModelContainer())
            {
                petsQuery = context.Pet.AsQueryable();

                if (currentUser.Role == "Админ")
                {
                    petsQuery = context.Pet.AsQueryable();
                }
                else if (currentUser.Role == "Ра")
                {
                    petsQuery = context.Pet.Where(p => p.Name == "Ра").AsQueryable();
                }
                else if (currentUser.Role == "Нуби")
                {
                    petsQuery = context.Pet.Where(p => p.Name == "Нуби").AsQueryable();
                }

                ApplySearch();
                ApplySort();
            }
        }

        private void ApplySearch()
        {
            using (var context = new PetsModelContainer())
            {
                var query = context.Pet.AsQueryable();

                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query = query.Where(p => p.Description.Contains(txtSearch.Text));
                }

                petsQuery = query;
                ApplySort();
            }
        }

        private void ApplySort()
        {
            using (var context = new PetsModelContainer())
            {
                var query = petsQuery;

                switch (cmbSort.SelectedIndex)
                {
                    case 0: // Name (A-Z)
                        query = query.OrderBy(p => p.Name);
                        break;
                    case 1: // Name (Z-A)
                        query = query.OrderByDescending(p => p.Name);
                        break;
                    case 2: // Description (A-Z)
                        query = query.OrderBy(p => p.Description);
                        break;
                    case 3: // Description (Z-A)
                        query = query.OrderByDescending(p => p.Description);
                        break;
                }

                petsQuery = query;
                DisplayPets();
            }
        }

        private void DisplayPets()
        {
            petStackPanel.Children.Clear();

            using (var context = new PetsModelContainer())
            {
                var pets = petsQuery.ToList();

                foreach (var pet in pets)
                {
                    var petPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(0, 50, 0, 0) // Отступ в 50 пикселей между записями
                    };

                    var headerPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal
                    };

                    var nameTextBlock = new TextBlock
                    {
                        Text = pet.Name,
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 10, 0)
                    };

                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri(pet.ImagePath, UriKind.Absolute)),
                        Width = 100,
                        Height = 100,
                        Margin = new Thickness(200, 0, 0, 0) // Отступ в 200 пикселей
                    };

                    headerPanel.Children.Add(nameTextBlock);
                    headerPanel.Children.Add(image);

                    var descriptionTextBlock = new TextBlock
                    {
                        Text = pet.Description,
                        FontSize = 14,
                        Margin = new Thickness(0, 20, 0, 0) // Отступ в 20 пикселей между заголовком и описанием
                    };

                    petPanel.Children.Add(headerPanel);
                    petPanel.Children.Add(descriptionTextBlock);

                    petStackPanel.Children.Add(petPanel);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearch();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySort();
        }
    }
}
