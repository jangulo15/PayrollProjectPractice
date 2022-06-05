namespace PayrollProject;

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