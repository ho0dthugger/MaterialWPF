using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaterialWPF.File;
using MaterialWPF.Pages.User;

namespace MaterialWPF.Pages.User
{
    public partial class PageUser : Page
    {
        private MaterialEntities dbContext;

        public PageUser()
        {
            InitializeComponent();
            dbContext = new MaterialEntities();
            LoadCastings();

            // Подписываемся на событие Click для кнопки "Закрыть приложение"
            btnClose.Click += btnClose_Click;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void LoadCastings()
        {
            lstCastings.ItemsSource = dbContext.Заготовки.ToList();
        }

        private void lstCastings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCastings.SelectedItem != null)
            {
                Заготовки selectedCasting = (Заготовки)lstCastings.SelectedItem;
                // Load required materials and check availability
            }
        }

        private void StartProduction_Click(object sender, RoutedEventArgs e)
        {
            if (lstCastings.SelectedItem != null)
            {
                NavigationService.Navigate(new PageProductionProgress((Заготовки)lstCastings.SelectedItem));
            }
            else
            {
                MessageBox.Show("Выберите заготовку для начала производства.");
            }
        }

        private void ViewMaterials_Click(object sender, RoutedEventArgs e)
        {
            if (lstCastings.SelectedItem != null)
            {
                NavigationService.Navigate(new PageMaterials((Заготовки)lstCastings.SelectedItem));
            }
            else
            {
                MessageBox.Show("Выберите заготовку для просмотра материалов.");
            }
        }

        private void ViewCustomers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageViewCustomers());
        }

        private void ViewDefects_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageViewDefects());
        }

        private void ViewFinishedProducts_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageViewFinishedProducts());
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageLogin());
        }
    }
}