using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaterialWPF.File;
using MaterialWPF.Pages.User;

namespace MaterialWPF.Pages.User
{
    public partial class PageMaterials : Page
    {
        private MaterialEntities dbContext;
        private Заготовки selectedCasting;

        public PageMaterials(Заготовки casting)
        {
            InitializeComponent();
            dbContext = new MaterialEntities();
            selectedCasting = casting;
            LoadMaterials();

            // Подписываемся на событие Click для кнопки "Закрыть приложение"
            btnClose.Click += btnClose_Click;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void LoadMaterials()
        {
            try
            {
                var materials = dbContext.МатериалыЗаготовок
                    .Where(m => m.ID_Заготовки == selectedCasting.ID_Заготовки)
                    .Select(m => new
                    {
                        ID = m.Материалы.ID_Материала,
                        Название = m.Материалы.Название,
                        НаСкладе = m.Материалы.Количество,
                        Требуется = m.Количество
                    })
                    .ToList();

                lstMaterials.ItemsSource = materials;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке материалов: " + ex.Message);
            }
        }

        private void StartProduction_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageProductionProgress(selectedCasting));
        }

        private void ViewMaterials_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageMaterials(selectedCasting));
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