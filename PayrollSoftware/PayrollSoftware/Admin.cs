namespace PayrollProject;

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