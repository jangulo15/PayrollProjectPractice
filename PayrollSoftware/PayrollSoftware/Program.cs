using System;
using System.IO;

namespace PayrollProject
{
    class Staff
    {
        private float hourlyRate;
        private int hWorked;

        public float TotalPay { get; protected set; }
        public float BasicPay { get; private set; }
        public string NameOfStaff { get; private set; }
        public int HoursWorked
        {
            get
            {
                return hWorked;
            }
            set
            {
                if (value > 0)
                    hWorked = value;
                else
                    hWorked = 0;
            }


        }
        public Staff(string name, float rate)
        {
            NameOfStaff = name;
            hourlyRate = rate;
        }

        public virtual void CalculatePay()
        {
            Console.WriteLine("Calculating Pay...");
            BasicPay = hWorked * hourlyRate;
            TotalPay = BasicPay;
        }
        public override string ToString()
        {
            return "\nName of Staff: " + NameOfStaff
                + "\nRate: " + hourlyRate
                + "\nHours Worked: " + hWorked
                + "\nBasic Pay: " + BasicPay
                + "\nTotal Pay: " + TotalPay;
        }
    }

    class Manager : Staff
    {
        private const float managerHourlyRate = 50;
        public int Allowance { get; private set; }


        public Manager(string name)
            : base(name, managerHourlyRate)
        { }

        public override void CalculatePay()
        {
            base.CalculatePay();

            Allowance = 1000;

            if (HoursWorked > 160)
                TotalPay = BasicPay + Allowance;
        }

        public override string ToString()
        {
            return "\nName of Staff: " + NameOfStaff
                 + "\nRate: " + managerHourlyRate
                 + "\nHours Worked: " + HoursWorked
                 + "\nBasic Pay: " + BasicPay
                 + "\nAllowance: " + Allowance
                 + "\nTotal Pay: " + TotalPay;
        }
    }

    class Admin : Staff
    {
        private const float overtimeRate = 15.5f;
        private const float adminHourlyRate = 30;

        public float Overtime { get; private set; }

        public Admin(string name) : base(name, adminHourlyRate)
        { }

        public override void CalculatePay()
        {
            base.CalculatePay();

            if (HoursWorked > 160)
            {
                Overtime = overtimeRate * (HoursWorked - 160);
                TotalPay = BasicPay + Overtime;
            }
        }

        public override string ToString()
        {
            return "\nName of Staff: " + NameOfStaff
                + "\nRate: " + adminHourlyRate
                + "\nHours Worked: " + HoursWorked
                + "\nBasic Pay: " + BasicPay
                + "\nOvertime Pay: " + Overtime
                + "\nTotal Pay: " + TotalPay;
        }

    }

    class FileReader
    {
        public List<Staff> ReadFile()
        {
            List<Staff> myStaff = new List<Staff>();

            string[] result = new string[2];
            string path = "C:\\Users\\imjos\\Documents\\" +
                "C# Projects\\CSProject\\CSProject\\bin\\Debug\\staff.txt";
            string[] seperator = { ", " };

            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        result = sr.ReadLine().Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                        if (result[1] == "Manager")
                            myStaff.Add(new Manager(result[0]));
                        else
                            myStaff.Add(new Admin(result[0]));

                    }
                    sr.Close();
                }
            }
            else
                Console.WriteLine("Error. File Does Not Exist");

            return myStaff;
        }

    }

    class PaySlip
    {
        private int month;
        private int year;

        enum MonthsOfYear
        {
            JAN = 1,
            FEB = 2,
            MAR = 3,
            APR = 4,
            MAY = 5,
            JUN = 6,
            JUL = 7,
            AUG = 8,
            SEP = 9,
            OCT = 10,
            NOV = 11,
            DEC = 12
        }

        public PaySlip(int payMonth, int payYear)
        {
            month = payMonth;
            year = payYear;
        }

        public void GeneratePaySlip(List<Staff> myStaff)
        {
            string path;

            foreach (Staff staff in myStaff)
            {
                path = staff.NameOfStaff + ".txt";

                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine("PAYSLIP FOR {0} {1}", (MonthsOfYear)month, year);
                    sw.WriteLine("==========================");
                    sw.WriteLine("Name of Staff: {0}", staff.NameOfStaff);
                    sw.WriteLine("\nBasic Pay: {0:C}", staff.BasicPay);
                    if (staff.GetType() == typeof(Admin))
                        sw.WriteLine("OverTime: {0:C}", ((Admin)staff).Overtime);
                    else
                        sw.WriteLine("Allowance: {0:C}", ((Manager)staff).Allowance);

                    sw.WriteLine("\n==========================");
                    sw.WriteLine("Total Pay: {0:C}", staff.TotalPay);
                    sw.WriteLine("\n==========================");

                    sw.Close();

                }
            }


        }
        public void GenerateSummary(List<Staff> myStaff)
        {
            var result =
                from staff in myStaff
                where staff.HoursWorked < 10
                orderby staff.NameOfStaff ascending
                select new { staff.NameOfStaff, staff.HoursWorked };

            string path = "summary.txt";

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("Staff with less than 10 working hours in" +
                    " {0}, {1}\n", (MonthsOfYear)month, year);
                foreach (var staff in result)
                {
                    sw.WriteLine("Name of Staff: {0}, Hours Worked: {1}", staff.NameOfStaff, staff.HoursWorked);
                }
                sw.Close();
            }
        }

        public override string ToString()
        {
            return "month = " + month + "year = " + year;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Staff> myStaff = new List<Staff>();

            FileReader fr = new FileReader();

            int month = 0, year = 0;

            while (year == 0)
            {
                Console.Write("Please enter the year: ");

                try
                {
                    year = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "Try Again");
                }
            }

            while (month == 0)
            {
                Console.Write("\nPlease enter the month: ");
                try
                {
                    month = Convert.ToInt32(Console.ReadLine());
                    if (month < 1 || month > 12)
                    {
                        Console.WriteLine("Error. Invalid range." +
                            "Month must be betweeen 1 - 12.");
                        month = 0;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "Please Try again");
                }
            }

            myStaff = fr.ReadFile();

            for (int i = 0; i < myStaff.Count; i++)
            {
                try
                {
                    Console.Write("\nEnter hours worked for {0}:", myStaff[i].NameOfStaff);
                    myStaff[i].HoursWorked = Convert.ToInt32(Console.ReadLine());
                    myStaff[i].CalculatePay();
                    Console.WriteLine(myStaff[i].ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    i--;
                }
            }

            PaySlip ps = new PaySlip(month, year);

            ps.GeneratePaySlip(myStaff);
            ps.GenerateSummary(myStaff);

            Console.Read();
        }

    }



}