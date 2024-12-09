using MaterialWPF.File;
using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Data.Entity;
using System.IO;

namespace MaterialWPF.Pages.Admin
{
    public partial class PageManageUsers : Page
    {
        private MaterialEntities dbContext;

        public PageManageUsers()
        {
            InitializeComponent();
            dbContext = new MaterialEntities();
            LoadUsers();

            // Подписываемся на событие Click для кнопки "Закрыть приложение"
            btnClose.Click += btnClose_Click;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void LoadUsers()
        {
            dgUsers.ItemsSource = dbContext.Пользователи.ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var user in dgUsers.ItemsSource.Cast<Пользователи>())
                {
                    if (user.ID_Пользователя == 0) // Новый пользователь
                    {
                        dbContext.Пользователи.Add(user);
                    }
                    else // Существующий пользователь
                    {
                        dbContext.Entry(user).State = EntityState.Modified;
                    }
                }

                dbContext.SaveChanges();
                MessageBox.Show("Изменения сохранены!");
                LoadUsers(); // Обновляем список пользователей
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem as Пользователи;
            if (selectedUser != null)
            {
                dbContext.Пользователи.Remove(selectedUser);
                dbContext.SaveChanges();
                MessageBox.Show("Пользователь удален!");
                LoadUsers(); // Обновляем список пользователей
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.");
            }
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
            XFont font = new XFont("Times New Roman", 8, PdfSharp.Drawing.XFontStyleEx.Regular);
            DrawTable(gfxUsers, font, "Список пользователей (кроме администратора)", users, new[] { "ID", "Логин", "Роль" }, (Пользователи user) => new[] { user.ID_Пользователя.ToString(), user.Логин, user.Роль });

            // Добавляем страницу для заказов
            PdfPage pageOrders = document.AddPage();
            XGraphics gfxOrders = XGraphics.FromPdfPage(pageOrders);
            DrawTable(gfxOrders, font, "Список заказов", orders, new[] { "ID", "Заказчик", "Адрес", "Заказ", "Количество", "Дата заказа" }, (Заказы order) =>
            {
                var customer = order.Заказчики.FirstOrDefault();
                return new[] { order.ID_Заказа.ToString(), customer?.Имя, customer?.Адрес, order.Заготовки.Название, order.Количество.ToString(), order.Дата_Заказа.ToString() };
            });

            // Добавляем страницу для материалов
            PdfPage pageMaterials = document.AddPage();
            XGraphics gfxMaterials = XGraphics.FromPdfPage(pageMaterials);
            DrawTable(gfxMaterials, font, "Список материалов", materials, new[] { "ID", "Название", "Количество" }, (Материалы material) => new[] { material.ID_Материала.ToString(), material.Название, material.Количество.ToString() });

            // Добавляем страницу для продуктов
            PdfPage pageProducts = document.AddPage();
            XGraphics gfxProducts = XGraphics.FromPdfPage(pageProducts);
            DrawTable(gfxProducts, font, "Список продуктов", products, new[] { "ID", "Название" }, (Заготовки product) => new[] { product.ID_Заготовки.ToString(), product.Название });

            // Добавляем страницу для бракованных продуктов
            PdfPage pageDefects = document.AddPage();
            XGraphics gfxDefects = XGraphics.FromPdfPage(pageDefects);
            DrawTable(gfxDefects, font, "Список бракованных продуктов", defects, new[] { "ID", "Производство ID", "Описание" }, (Брак defect) => new[] { defect.ID_Брака.ToString(), defect.ID_Производства.ToString(), defect.Описание });

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

        private void DrawTable<T>(XGraphics gfx, XFont font, string title, IEnumerable<T> data, string[] headers, Func<T, string[]> rowMapper)
        {
            int x = 50; // Начальная позиция по оси X
            int y = 50; // Начальная позиция по оси Y
            int rowHeight = 20; // Высота строки
            int padding = 5; // Отступ от границ ячеек

            gfx.DrawString(title, font, XBrushes.Black, new XRect(x, y, gfx.PageSize.Width, rowHeight), XStringFormats.TopLeft);
            y += rowHeight;

            // Вычисляем ширину столбцов
            int[] columnWidths = new int[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                columnWidths[i] = (int)gfx.MeasureString(headers[i], font).Width + 2 * padding;
            }

            foreach (var item in data)
            {
                string[] rowData = rowMapper(item);
                for (int i = 0; i < rowData.Length; i++)
                {
                    int textWidth = (int)gfx.MeasureString(rowData[i], font).Width + 2 * padding;
                    if (textWidth > columnWidths[i])
                    {
                        columnWidths[i] = textWidth;
                    }
                }
            }

            int totalWidth = columnWidths.Sum();

            // Отрисовка заголовков
            DrawRow(gfx, font, headers, columnWidths, ref x, ref y, rowHeight, padding);

            // Отрисовка данных
            foreach (var item in data)
            {
                string[] rowData = rowMapper(item);
                DrawRow(gfx, font, rowData, columnWidths, ref x, ref y, rowHeight, padding);
            }

            // Рисуем границы таблицы
            gfx.DrawRectangle(XPens.Black, x, y - data.Count() * rowHeight - rowHeight, totalWidth, data.Count() * rowHeight + rowHeight);
        }

        private void DrawRow(XGraphics gfx, XFont font, string[] rowData, int[] columnWidths, ref int x, ref int y, int rowHeight, int padding)
        {
            int totalWidth = columnWidths.Sum();

            // Рисуем верхнюю границу строки
            gfx.DrawLine(XPens.Black, x, y, x + totalWidth, y);

            for (int i = 0; i < rowData.Length; i++)
            {
                int textWidth = (int)gfx.MeasureString(rowData[i], font).Width;
                int columnWidth = columnWidths[i];

                // Рисуем текст в ячейке
                gfx.DrawString(rowData[i], font, XBrushes.Black, new XRect(x + padding, y + padding, columnWidth - 2 * padding, rowHeight - 2 * padding), XStringFormats.TopLeft);

                // Рисуем правую границу ячейки
                gfx.DrawLine(XPens.Black, x + columnWidth, y, x + columnWidth, y + rowHeight);

                x += columnWidth;
            }

            // Рисуем нижнюю границу строки
            gfx.DrawLine(XPens.Black, x - totalWidth, y + rowHeight, x, y + rowHeight);

            y += rowHeight;
            x = 50;
        }
    }
}