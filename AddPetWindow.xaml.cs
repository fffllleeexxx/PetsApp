using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;


namespace PetsApp
{
    public partial class AddPetWindow : Window
    {
        public AddPetWindow()
        {
            InitializeComponent();
        }

        private void btnAddPet_Click(object sender, RoutedEventArgs e)
        {
            string name = (cmbPet.SelectedItem as ComboBoxItem).Content.ToString();
            string description = txtDescription.Text;
            string imagePath = txtImagePath.Text;

            if (!File.Exists(imagePath))
            {
                MessageBox.Show("The specified image file does not exist.");
                return;
            }

            using (var context = new PetsModelContainer())
            {
                var pet = new Pet
                {
                    Name = name,
                    Description = description,
                    ImagePath = imagePath
                };

                context.Pet.Add(pet);
                context.SaveChanges();
            }

            MessageBox.Show("Pet added successfully!");
            this.Close();
        }
    }
}
