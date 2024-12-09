using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaterialWPF.File;

namespace MaterialWPF.Pages.User
{
    public partial class PageViewDefects : Page
    {
        private MaterialEntities dbContext;

        public PageViewDefects()
        {
            InitializeComponent();
            dbContext = new MaterialEntities();
            LoadDefects();

            // Подписываемся на событие Click для кнопки "Закрыть приложение"
            btnClose.Click += btnClose_Click;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void LoadDefects()
        {
            dgDefects.ItemsSource = dbContext.Брак.ToList();
        }

        private void StartProduction_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageUser());
        }

        private void ViewMaterials_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageUser());
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