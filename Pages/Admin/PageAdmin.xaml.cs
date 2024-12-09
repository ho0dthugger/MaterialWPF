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
using MaterialWPF.Pages.User;
using MaterialWPF.Pages.Admin;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Data.Entity;
using MaterialWPF.File;
using Microsoft.Win32;
using System.IO;

namespace MaterialWPF.Pages.Admin
{
    public partial class PageAdmin : Page
    {
        private MaterialEntities dbContext;

        public PageAdmin()
        {
            InitializeComponent();
            dbContext = new MaterialEntities();

            // Подписываемся на событие Click для кнопки "Закрыть приложение"
            btnClose.Click += btnClose_Click;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void ManageMaterials_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageManageMaterials());
        }

        private void ManageUsers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageManageUsers());
        }

        private void ViewProduction_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageViewProduction());
        }

        private void ViewCustomers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageViewCustomer());
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageLogin());
        }

        private void SavePDF_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные из базы данных
            var users = dbContext.Пользователи.Where(u => u.Роль != "Администратор").ToList();
            var orders = dbContext.Заказы.Include(o => o.Заказчики).Include(o => o.Заготовки).ToList();
            var materials = dbContext.Материалы.ToList();
            var products = dbContext.Заготовки.ToList();
            var defects = dbContext.Брак.Include(d => d.Производство).ToList();

            // Создаем PDF-документ
            PdfDocument document = new PdfDocument();

            // Добавляем страницу для пользователей
            PdfPage pageUsers = document.AddPage();
            XGraphics gfxUsers = XGraphics.FromPdfPage(pageUsers);
            XFont font = new XFont("Verdana", 8, PdfSharp.Drawing.XFontStyleEx.Regular);
            gfxUsers.DrawString("Список пользователей (кроме администратора)", font, XBrushes.Black, new XRect(50, 50, pageUsers.Width, pageUsers.Height), XStringFormats.TopLeft);
            int yUsers = 80;
            foreach (var user in users)
            {
                gfxUsers.DrawString($"ID: {user.ID_Пользователя}, Логин: {user.Логин}, Роль: {user.Роль}", font, XBrushes.Black, new XRect(50, yUsers, pageUsers.Width, pageUsers.Height), XStringFormats.TopLeft);
                yUsers += 20;
            }

            // Добавляем страницу для заказов
            PdfPage pageOrders = document.AddPage();
            XGraphics gfxOrders = XGraphics.FromPdfPage(pageOrders);
            gfxOrders.DrawString("Список заказов", font, XBrushes.Black, new XRect(50, 50, pageOrders.Width, pageOrders.Height), XStringFormats.TopLeft);
            int yOrders = 80;
            foreach (var order in orders)
            {
                foreach (var customer in order.Заказчики)
                {
                    gfxOrders.DrawString($"ID: {order.ID_Заказа}, Заказчик: {customer.Имя}, Адрес: {customer.Адрес}, Заказ: {order.Заготовки.Название}, Количество: {order.Количество}, Дата заказа: {order.Дата_Заказа}", font, XBrushes.Black, new XRect(50, yOrders, pageOrders.Width, pageOrders.Height), XStringFormats.TopLeft);
                    yOrders += 20;
                }
            }

            // Добавляем страницу для материалов
            PdfPage pageMaterials = document.AddPage();
            XGraphics gfxMaterials = XGraphics.FromPdfPage(pageMaterials);
            gfxMaterials.DrawString("Список материалов", font, XBrushes.Black, new XRect(50, 50, pageMaterials.Width, pageMaterials.Height), XStringFormats.TopLeft);
            int yMaterials = 80;
            foreach (var material in materials)
            {
                gfxMaterials.DrawString($"ID: {material.ID_Материала}, Название: {material.Название}, Количество: {material.Количество}", font, XBrushes.Black, new XRect(50, yMaterials, pageMaterials.Width, pageMaterials.Height), XStringFormats.TopLeft);
                yMaterials += 20;
            }

            // Добавляем страницу для продуктов
            PdfPage pageProducts = document.AddPage();
            XGraphics gfxProducts = XGraphics.FromPdfPage(pageProducts);
            gfxProducts.DrawString("Список продуктов", font, XBrushes.Black, new XRect(50, 50, pageProducts.Width, pageProducts.Height), XStringFormats.TopLeft);
            int yProducts = 80;
            foreach (var product in products)
            {
                gfxProducts.DrawString($"ID: {product.ID_Заготовки}, Название: {product.Название}", font, XBrushes.Black, new XRect(50, yProducts, pageProducts.Width, pageProducts.Height), XStringFormats.TopLeft);
                yProducts += 20;
            }

            // Добавляем страницу для бракованных продуктов
            PdfPage pageDefects = document.AddPage();
            XGraphics gfxDefects = XGraphics.FromPdfPage(pageDefects);
            gfxDefects.DrawString("Список бракованных продуктов", font, XBrushes.Black, new XRect(50, 50, pageDefects.Width, pageDefects.Height), XStringFormats.TopLeft);
            int yDefects = 80;
            foreach (var defect in defects)
            {
                gfxDefects.DrawString($"ID: {defect.ID_Брака}, Производство ID: {defect.ID_Производства}, Описание: {defect.Описание}", font, XBrushes.Black, new XRect(50, yDefects, pageDefects.Width, pageDefects.Height), XStringFormats.TopLeft);
                yDefects += 20;
            }

            // Открываем диалоговое окно для выбора места сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";
            saveFileDialog.FileName = "Report.pdf";

            if (saveFileDialog.ShowDialog() == true)
            {
                // Сохраняем PDF-документ
                document.Save(saveFileDialog.FileName);
                MessageBox.Show("PDF успешно сохранен!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}