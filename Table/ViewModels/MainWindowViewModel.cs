using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reactive;
using System.Text;
using Table.Models;

namespace Table.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<Student> _students = null!;

        private int _index;
        private string _newStudentName = null!;
        private int _selectedMath;
        private int _selectedRussian;
        private int _selectedProgramming;
        private int _selectedPhysicalculture;
        private int _selectedHistory;
        private List<int> _gradeChoices = new List<int> { 0, 1, 2 };
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged("Students");
            }
        }
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged("Index");
            }
        }

        public MainWindowViewModel()
        {
            Students = new ObservableCollection<Student>();
            AddStudentCommand = ReactiveCommand.Create(AddStudent);
            DeleteStudentCommand = ReactiveCommand.Create(DeleteStudent);
            SaveCommand = ReactiveCommand.Create(ExecuteSaveCommand);
            UploadCommand = ReactiveCommand.Create(ExecuteUploadCommand);

            // заполнение случайными данными
            Random rnd = new Random();
            string[] names = { "Иванов И. И.", "Николаев Н.Н.", "Ефремов К.В." };
            List<string> availableNames = new List<string>(names);

            for (int i = 0; i < names.Length; i++)
            {
                int index = rnd.Next(availableNames.Count); // выбираем случайный индекс из оставшихся имен
                string name = availableNames[index]; // выбираем случайное имя из оставшихся имен
                availableNames.Remove(name); // удаляем имя из списка доступных имен

                Student student = new Student();
                student.Name = name;
                student.Math = rnd.Next(1, 3);
                student.Russian = rnd.Next(1, 3);
                student.Programming = rnd.Next(1, 3);
                student.Physical_culture = rnd.Next(1, 3);
                student.History = rnd.Next(1, 3);
                student.Average_point = Math.Round((double)(student.Math + student.Russian + student.Programming + student.Physical_culture +
        student.History) / 5, 2);

                Students.Add(student);
            }
            // Вычисляем средний балл для каждого предмета
            double sumMath = 0, sumRussian = 0, sumProgramming = 0, sumPhysical_culture = 0, sumHistory = 0;
            foreach (var student in Students)
            {
                sumMath += student.Math;
                sumRussian += student.Russian;
                sumProgramming += student.Programming;
                sumPhysical_culture += student.Physical_culture;
                sumHistory += student.History;
            }

            // Вычисляем общий средний балл по всем студентам
            int count = Students.Count;
            double averageMath = Math.Round(sumMath / count, 2);
            double averageRussian = Math.Round(sumRussian / count, 2);
            double averageProgramming = Math.Round(sumProgramming / count, 2);
            double averagePhysicalculture = Math.Round(sumPhysical_culture / count, 2);
            double averageHistory = Math.Round(sumHistory / count, 2);

            // Устанавливаем значения свойств для отображения в интерфейсе
            ScMath = averageMath.ToString(CultureInfo.CurrentCulture);
            ScRussian = averageRussian.ToString(CultureInfo.CurrentCulture);
            ScPhysicalCulture = averagePhysicalculture.ToString(CultureInfo.CurrentCulture);
            ScProgramming = averageProgramming.ToString(CultureInfo.InvariantCulture);
            ScHistory = averageHistory.ToString(CultureInfo.CurrentCulture);
            ScAveragePoint = Math.Round((averageMath + averageRussian + averageProgramming + averagePhysicalculture + averageHistory) / 5, 2).ToString(CultureInfo.CurrentCulture);
        }
        public ReactiveCommand<Unit, Unit> AddStudentCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteStudentCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> UploadCommand { get; }

        private void AddStudent()
        {
            Student newStudent = new Student()
            {
                Name = NewStudentName,
                Math = SelectedMath,
                Russian = SelectedRussian,
                Programming = SelectedProgramming,
                Physical_culture = SelectedPhysicalculture,
                History = SelectedHistory,
            };
            Students.Add(newStudent);
            UpdateScAverageMarks();
            OnPropertyChanged(nameof(Students));
        }
        private void DeleteStudent()
        {
            Student selectedStudent = Students[Index];
            Students.Remove(selectedStudent);
            UpdateScAverageMarks();
            OnPropertyChanged(nameof(Students));
        }
        private void UpdateScAverageMarks()
        {
            if (Students.Count == 0)
            {
                ScAveragePoint = ScMath = ScRussian = ScProgramming = ScPhysicalCulture = ScHistory = 0.ToString();
            }
            else
            {
                double total = 0, totalMath = 0, totalRussian = 0, totalProgramming = 0, totalPhysicalculture = 0, totalHistory = 0;
                foreach (Student student in Students)
                {
                    total += student.Average_point;
                    totalMath += student.Math;
                    totalRussian += student.Russian;
                    totalProgramming += student.Programming;
                    totalPhysicalculture += student.Physical_culture;
                    totalHistory += student.History;
                }
                ScAveragePoint = (total / Students.Count).ToString(CultureInfo.InvariantCulture);
                ScMath = (totalMath / Students.Count).ToString(CultureInfo.InvariantCulture);
                ScRussian = (totalRussian / Students.Count).ToString(CultureInfo.InvariantCulture);
                ScProgramming = (totalProgramming / Students.Count).ToString(CultureInfo.InvariantCulture);
                ScPhysicalCulture = (totalPhysicalculture / Students.Count).ToString(CultureInfo.InvariantCulture);
                ScHistory = (totalHistory / Students.Count).ToString(CultureInfo.InvariantCulture);
            }
            OnPropertyChanged(nameof(ScAveragePoint));
            OnPropertyChanged(nameof(ScMath));
            OnPropertyChanged(nameof(ScRussian));
            OnPropertyChanged(nameof(ScProgramming));
            OnPropertyChanged(nameof(ScPhysicalCulture));
            OnPropertyChanged(nameof(ScHistory));
        }
        public string ScMath { get; set; }
        public string ScRussian { get; set; }
        public string ScProgramming { get; set; }
        public string ScPhysicalCulture { get; set; }
        public string ScHistory { get; set; }
        public string ScAveragePoint { get; set; }

        public string NewStudentName
        {
            get { return _newStudentName; }
            set { this.RaiseAndSetIfChanged(ref _newStudentName, value); }
        }
        public int SelectedMath
        {
            get { return _selectedMath; }
            set { this.RaiseAndSetIfChanged(ref _selectedMath, value); }
        }
        public int SelectedRussian
        {
            get { return _selectedRussian; }
            set { this.RaiseAndSetIfChanged(ref _selectedRussian, value); }
        }
        public int SelectedProgramming
        {
            get { return _selectedProgramming; }
            set { this.RaiseAndSetIfChanged(ref _selectedProgramming, value); }
        }
        public int SelectedPhysicalculture
        {
            get { return _selectedPhysicalculture; }
            set { this.RaiseAndSetIfChanged(ref _selectedPhysicalculture, value); }
        }
        public int SelectedHistory
        {
            get { return _selectedHistory; }
            set { this.RaiseAndSetIfChanged(ref _selectedHistory, value); }
        }
        public List<int> GradeChoices
        {
            get => _gradeChoices;
            set => this.RaiseAndSetIfChanged(ref _gradeChoices, value);
        }
        private void ExecuteSaveCommand()
        {
            Serialization.SaveDataToXmlFile(Students, "../../../StudentsData.xml");
        }
        private void ExecuteUploadCommand()
        {
            Students = Serialization.LoadDataFromXmlFile("../../../StudentsData.xml");
            UpdateScAverageMarks();
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
