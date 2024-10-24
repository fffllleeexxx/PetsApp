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
                var pets = context.Pet.ToList();

                if (currentUser.Role == "Админ")
                {
                    pets = context.Pet.ToList();
                }
                else if (currentUser.Role == "Ра")
                {
                    pets = context.Pet.Where(p => p.Name == "Ра").ToList();
                }
                else if (currentUser.Role == "Нуби")
                {
                    pets = context.Pet.Where(p => p.Name == "Нуби").ToList();
                }

                foreach (var pet in pets)
                {
                    var petPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 10, 0, 10)
                    };

                    var nameTextBlock = new TextBlock
                    {
                        Text = pet.Name,
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 10, 0)
                    };

                    var descriptionTextBlock = new TextBlock
                    {
                        Text = pet.Description,
                        FontSize = 14,
                        Margin = new Thickness(0, 0, 10, 0)
                    };

                    var imagePathTextBlock = new TextBlock
                    {
                        Text = pet.ImagePath,
                        FontSize = 12,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    petPanel.Children.Add(nameTextBlock);
                    petPanel.Children.Add(descriptionTextBlock);
                    petPanel.Children.Add(imagePathTextBlock);

                    petStackPanel.Children.Add(petPanel);
                }
            }
        }
    }
}
