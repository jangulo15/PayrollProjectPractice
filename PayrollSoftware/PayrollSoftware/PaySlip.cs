namespace PayrollProject;

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