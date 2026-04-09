using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TeplomasterPlus
{
    public partial class MainWindow : Window
    {
        // Простая модель заявки
        public class Request
        {
            public int Id { get; set; }
            public string Client { get; set; }
            public string Phone { get; set; }
            public string Equipment { get; set; }
            public string Status { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
        }

        // Список заявок (хранится в памяти)
        private List<Request> requests = new List<Request>();
        private int nextId = 1;

        public MainWindow()
        {
            InitializeComponent();
            LoadSampleData();
            RefreshGrid();
        }

        // Добавление тестовых данных
        private void LoadSampleData()
        {
            requests.Add(new Request
            {
                Id = nextId++,
                Client = "Сергеев Андрей",
                Phone = "+7 (922) 000-11-22",
                Equipment = "Котел Buderus",
                Status = "В работе",
                Description = "Котел не запускается, ошибка F28",
                Date = DateTime.Now
            });

            requests.Add(new Request
            {
                Id = nextId++,
                Client = "ООО Теплый дом",
                Phone = "+7 (495) 123-45-67",
                Equipment = "Насос Grundfos",
                Status = "Новая",
                Description = "Шумит при работе",
                Date = DateTime.Now
            });
        }

        // Обновление таблицы и статистики
        private void RefreshGrid()
        {
            RequestsGrid.ItemsSource = null;
            RequestsGrid.ItemsSource = requests.OrderByDescending(r => r.Date);

            // Обновляем статистику
            TxtTotal.Text = $"Всего: {requests.Count} заявок";
            TxtNewCount.Text = $"Новых: {requests.Count(r => r.Status == "Новая")}";
            TxtWorkCount.Text = $"В работе: {requests.Count(r => r.Status == "В работе")}";
            TxtDoneCount.Text = $"Выполнено: {requests.Count(r => r.Status == "Выполнена")}";
        }

        // Добавление новой заявки
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем заполнение обязательных полей
            if (string.IsNullOrWhiteSpace(TxtClient.Text))
            {
                MessageBox.Show("Введите ФИО клиента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtEquipment.Text))
            {
                MessageBox.Show("Введите оборудование!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаем новую заявку
            var newRequest = new Request
            {
                Id = nextId++,
                Client = TxtClient.Text.Trim(),
                Phone = TxtPhone.Text.Trim(),
                Equipment = TxtEquipment.Text.Trim(),
                Status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Description = TxtDescription.Text.Trim(),
                Date = DateTime.Now
            };

            requests.Add(newRequest);
            RefreshGrid();

            // Очищаем форму
            TxtClient.Clear();
            TxtPhone.Clear();
            TxtEquipment.Clear();
            TxtDescription.Clear();
            CmbStatus.SelectedIndex = 0;

            MessageBox.Show("Заявка добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Удаление заявки
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int id = (int)button.Tag;
                var request = requests.FirstOrDefault(r => r.Id == id);

                if (request != null)
                {
                    var result = MessageBox.Show($"Удалить заявку #{id} от {request.Client}?",
                        "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        requests.Remove(request);
                        RefreshGrid();
                        MessageBox.Show("Заявка удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}