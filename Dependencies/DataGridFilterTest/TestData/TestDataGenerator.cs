using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace DataGridFilterTest.TestData
{
    public class TestDataGenerator : INotifyPropertyChanged
    {
        private TestDataGenerator()
        {
            employeeList = new ObservableCollection<Employee>();
            employeePositionList = new ObservableCollection<EmployeePosition>();
            NumberOfRecordsToGenerate = 1000;
        }

        static TestDataGenerator()
        {
            instance = null;
        }

        private ObservableCollection<Employee> employeeList;
        private ObservableCollection<Employee> employeeListCopy;

        private ObservableCollection<EmployeePosition> employeePositionList;
        private ObservableCollection<EmployeeStatus> employeeStatusList;
        private double testDataGenerationPercent;
        private bool isTestDataGenerationInProgress;
        private int numberOfRecordsToGenerate;

        private static TestDataGenerator instance;

        public static TestDataGenerator Instance
        {
            get
            {
                if (instance == null) instance = new TestDataGenerator();

                return instance;
            }
        }

        public int NumberOfRecordsToGenerate
        {
            get
            {
                return numberOfRecordsToGenerate;
            }
            set
            {
                numberOfRecordsToGenerate = value;
                NotifyPropertyChanged("NumberOfRecordsToGenerate");
            }
        }

        public ObservableCollection<Employee> EmployeeList
        {
            get
            {
                return employeeList;
            }
            set
            {
                employeeList = value;
                NotifyPropertyChanged("EmployeeList");
            }
        }

        public ObservableCollection<Employee> EmployeeListCopy
        {
            get
            {
                return employeeListCopy;
            }
            set
            {
                employeeListCopy = value;
                NotifyPropertyChanged("EmployeeListCopy");
            }
        }

        public ObservableCollection<EmployeePosition> EmployeePositionList
        {
            get
            {
                return employeePositionList;
            }
            set
            {
                employeePositionList = value;
                NotifyPropertyChanged("EmployeePositionList");
            }
        }

        public ObservableCollection<EmployeeStatus> EmployeeStatuses
        {
            get
            {
                return employeeStatusList;
            }
            set
            {
                employeeStatusList = value;
                NotifyPropertyChanged("EmployeeStatuses");
            }
        }

        public double TestDataGenerationPercent
        {
            get
            {
                return testDataGenerationPercent;
            }
            set
            {
                testDataGenerationPercent = value;
                NotifyPropertyChanged("TestDataGenerationPercent");
            }
        }

        public bool IsTestDataGenerationInProgress
        {
            get
            {
                return isTestDataGenerationInProgress;
            }
            set
            {
                isTestDataGenerationInProgress = value;
                NotifyPropertyChanged("IsTestDataGenerationInProgress");
            }
        }

        public void GenerateTestData(Action<EventArgs> callback)
        {
            if (NumberOfRecordsToGenerate > 0)
            {
                IsTestDataGenerationInProgress = true;

                BackgroundWorker worker = new BackgroundWorker();

                List<Employee> list = new List<Employee>();

                worker.DoWork += delegate(object sender, DoWorkEventArgs e)
                {
                    for (int i = 0; i < NumberOfRecordsToGenerate; i++)
                    {
                        initRandomGenerator();

                        Employee emp = new Employee();

                        fillWithTheRandomData(emp, i);

                        list.Add(emp);

                        TestDataGenerationPercent = ((double)i / NumberOfRecordsToGenerate) * 100;
                    }
                };

                worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
                {
                    EmployeeStatuses = new ObservableCollection<EmployeeStatus>( STATUS.ToList());
                    EmployeePositionList = new ObservableCollection<EmployeePosition>(POSITIONS.ToList());

                    EmployeeList = new ObservableCollection<Employee>(list);

                    Employee[] tmpArray = new Employee[EmployeeList.Count];
                    EmployeeList.ToList().CopyTo(tmpArray);
                    EmployeeListCopy = new ObservableCollection<Employee>(tmpArray.ToList());

                    IsTestDataGenerationInProgress = false;

                    if (callback != null) callback(EventArgs.Empty);
                };

                worker.RunWorkerAsync();
            }
        }

        public void InsertNewEmployeeWithNewPosition()
        {
            EmployeePosition newPosition = new EmployeePosition()
            {
                Id = (this.EmployeePositionList.Max(p => p.Id) + 1),
                Name = "Position " + (this.EmployeePositionList.Max(p => p.Id) + 1)
            };

            this.EmployeePositionList.Add(newPosition);
            NotifyPropertyChanged("EmployeePositionList");

            Employee emp = new Employee();
            int employeeId = this.EmployeeList.Max(e => e.Id) + 1;

            fillWithTheRandomData(emp, employeeId);

            emp.Position = newPosition;

            this.EmployeeList.Add(emp);
            NotifyPropertyChanged("EmployeeList");
        }

        #region Internal - random data generation
        
        Random random;
        private void initRandomGenerator()
        {
            random = new Random((int)DateTime.Now.Ticks);

            random = new Random((int)new DateTime(
                random.Next(DateTime.MinValue.Year, DateTime.MaxValue.Year), 
                random.Next(1,12), 
                random.Next(1,28)).Ticks);

            System.Threading.Thread.Sleep(5);
        }

        private void fillWithTheRandomData(Employee e, int i)
        {
            e.Id               = i;

            e.Name             = getRandomName();
            e.Email            = getRandomEmail(e.Name);
            e.Address          = getRandomAddress();
            e.EmployeeGuid     = getEmployeeGuid(i);
            e.WorkExperience = random.Next(0, 40);
            e.Position         = i % 13 == 0 ? null : getRadndomPosition();
            e.EmployeeStatusId = getRandomStatusId();
            e.IsInterviewed    = getRandomIsInterviewed();
            e.DateOfBirth      = getRandomDateOfBirth();
        }

        private string getRandomName()
        {
            int number = random.Next(NAMES.Length - 1);

            return NAMES[number];
        }

        private string getRandomEmail(string email)
        {
            return email + "-" + getRandomString(3) + "@" + getRandomDomain();
        }

        private string getRandomAddress()
        {
            return getRandomString(random.Next(5, 10)) + ", " + getRandomString(random.Next(5, 15)) + " " + random.Next(0, 200);
        }

        private Guid? getEmployeeGuid(int i)
        {
            Guid? value;

            if (i % 10 == 0)
            {
                value = null;
            }
            else
            {
                value = Guid.NewGuid();
            }

            return value;
        }

        private EmployeePosition getRadndomPosition()
        {
            int number = random.Next(POSITIONS.Length - 1);

            return POSITIONS[number];
        }

        private int getRandomStatusId()
        {
            return random.Next(1, STATUS.Length);
        }

        private bool getRandomIsInterviewed()
        {
            return random.NextDouble() < 0.5 ? false : true;
        }

        private DateTime getRandomDateOfBirth()
        {
            return new DateTime(
                random.Next(1950, 1990), random.Next(1, 12), random.Next(1, 28));
        }

        private readonly string[] NAMES = new string[]
        {
            "Mark", "Tom", "Harry", "Sally", "Sandra", "Paul", "Anastasia", "David", "Alex", "Michael", "Tina", "Zachary", "Bob", "Elise",
            "Jime", "Anderry","Rustin","Ivadon","Nichardo","Jasey","Rent","Millack","Alenn","Serrett","Tanifer","Syllica","Allickie","Jacey"
            ,"Janther","Racey","Alicherry","Clary","Kather","Bonna"
        };

        private readonly string[] DOMAINS = new string[]
        {
            "xxx.com", "aa.com", "min.com", "erp.com", "holidays.com", "mon.com", "san.com", "sun.com", "ibm.com", "hp.com", "google.com", "yahoo.com", "bing.com", "ask.com"
        };

        private readonly string ASCII = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        private readonly EmployeeStatus[] STATUS = new EmployeeStatus[]
        {
            new EmployeeStatus{Id=1, Name="Available"},
            new EmployeeStatus{Id=2, Name="Not Available"},
            new EmployeeStatus{Id=3, Name="Undefined"}
        };

        private readonly EmployeePosition[] POSITIONS = new EmployeePosition[] 
        { 
            new EmployeePosition {Id=1,  Name="EAP Specialist"},
            new EmployeePosition {Id=2,  Name="Instructor"},
            new EmployeePosition {Id=3,  Name="Full professor"},
            new EmployeePosition {Id=4,  Name="ERP Specialist"},
            new EmployeePosition {Id=5,  Name="SQL Programmer"},
            new EmployeePosition {Id=6,  Name="QA Tester"},
            new EmployeePosition {Id=7,  Name="Senior Software Engineer "},
            new EmployeePosition {Id=8,  Name="Technical Analyst"},
            new EmployeePosition {Id=9,  Name="Web Master"},
            new EmployeePosition {Id=10, Name="Programmer Analyst "}
        };

        private string getRandomDomain()
        {
            int number = random.Next(DOMAINS.Length - 1);

            return NAMES[number];
        }

        private char getRandomChar()
        {
            int number = random.Next(ASCII.Length - 1);

            return ASCII[number];
        }

        private string getRandomString(int length)
        {
            StringBuilder randomString = new StringBuilder();

            for(int i = 0; i < length; i++)
            {
                randomString.Append(getRandomChar());
            }

            return randomString.ToString();
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
