using MaterialWPF.File;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MaterialWPF.Pages.User
{
    public partial class PageProductionProgress : Page
    {
        private MaterialEntities dbContext;
        private Random random;
        private Заготовки selectedCasting;
        private bool isProductionCompleted;

        public PageProductionProgress(Заготовки casting)
        {
            InitializeComponent();
            dbContext = new MaterialEntities();
            random = new Random();
            selectedCasting = casting;
            progressBar.Value = 0;
            isProductionCompleted = false;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Подписываемся на событие Click для кнопки "Закрыть приложение"
            btnClose.Click += btnClose_Click;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (progressBar.Value < 100)
            {
                progressBar.Value += 1;
                lblProgress.Content = progressBar.Value + "%";
            }
            else
            {
                if (!isProductionCompleted)
                {
                    isProductionCompleted = true;

                    // Генерация случайного процента брака и успеха
                    int defectPercentage = random.Next(0, 101); // 0-100%
                    bool isDefect = defectPercentage <= 25; // 25% шанс брака

                    // Запись результата производства в базу данных
                    Производство production = new Производство
                    {
                        ID_Заготовки = selectedCasting.ID_Заготовки,
                        Статус = isDefect ? "Брак" : "Готово",
                        Дата_Начала = DateTime.Now,
                        Дата_Завершения = DateTime.Now
                    };

                    dbContext.Производство.Add(production);
                    dbContext.SaveChanges();

                    if (isDefect)
                    {
                        Брак defect = new Брак
                        {
                            ID_Производства = production.ID_Производства,
                            Описание = "Бракованная заготовка"
                        };

                        dbContext.Брак.Add(defect);
                        dbContext.SaveChanges();
                    }

                    // Показ уведомления
                    MessageBoxResult result = MessageBox.Show($"Производство завершено.\nСтатус: {(isDefect ? "Брак" : "Успех")}", "Производство завершено", MessageBoxButton.OK);

                    // Переход на страницу с результатом
                    if (result == MessageBoxResult.OK)
                    {
                        if (isDefect)
                        {
                            NavigationService.Navigate(new PageViewDefects());
                        }
                        else
                        {
                            NavigationService.Navigate(new PageViewFinishedProducts());
                        }
                    }
                }
            }
        }
    }
}