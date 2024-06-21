namespace GetStudentGpa.Models
{
    public class StudentGPAModel
    {
        public double CumulativeGpa { get; set; }
        public double UnitsEarned { get; set; }
        public double UnitsAttempted { get; set; }
        public double QualityPoints { get; set; }
        public double QualityUnits { get; set; }
        public double GradePoints { get; set; }
        public int CoursesTaken { get; set; }
        public string CumulativeGpaText { get; set; }
    }
}
