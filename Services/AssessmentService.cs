namespace goOfflineE.Services
{
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using goOfflineE.Repository;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AssessmentService" />.
    /// </summary>
    public class AssessmentService : IAssessmentService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        public AssessmentService(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        /// <summary>
        /// The CreateAssessment.
        /// </summary>
        /// <param name="model">The model<see cref="Assessment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateAssessment(Assessment model)
        {
            var assessmentId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

            var assessment = new Entites.Assessment(model.SchoolId, assessmentId)
            {

                AssessmentTitle = model.AssessmentTitle,
                AssessmentDescription = model.AssessmentDescription,
                AssessmentQuiz = model.AssessmentQuiz,
                ClassId = model.ClassId,
                SubjectName = model.SubjectName,

                Active = true,
                CreatedBy = model.CreatedBy,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = model.CreatedBy,
            };
            try
            {
                await _tableStorage.AddAsync("Assessments", assessment);
            }
            catch (Exception ex)
            {
                throw new AppException("Create assessment error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The CreateStudentAssessment.
        /// </summary>
        /// <param name="model">The model<see cref="StudentAssessment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateStudentAssessment(StudentAssessment model)
        {
            var studentAssessmentId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

            var studentAssessment = new Entites.StudentAssessment(model.SchoolId, studentAssessmentId)
            {
                StudentId = model.StudentId,
                StudentName = model.StudentName,
                AssessmentId = model.AssessmentId,
                AssessmentAnswers = model.AssessmentAnswers,
                Attempts = model.Attempts,

                Active = true,
                CreatedBy = model.StudentId,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = model.StudentId,
            };

            try
            {
                await _tableStorage.AddAsync("StudentAssessments", studentAssessment);
            }
            catch (Exception ex)
            {
                throw new AppException("Create student assessment error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The GetAssessments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{Assessment}}"/>.</returns>
        public async Task<IEnumerable<Assessment>> GetAssessments(string schoolId, string classId)
        {
            TableQuery<Entites.Assessment> assessmentQuery = new TableQuery<Entites.Assessment>()
                 .Where(TableQuery.GenerateFilterCondition("ClassId", QueryComparisons.Equal, classId));
            var assessments = await _tableStorage.QueryAsync<Entites.Assessment>("Assessments", assessmentQuery);

            var assessmentList = from assessment in assessments
                                 where assessment.Active.GetValueOrDefault(false)
                                 orderby assessment.UpdatedOn descending
                                 select new Assessment
                                 {
                                     Id = assessment.RowKey,
                                     CreatedDate = assessment.Timestamp.DateTime,
                                     AssessmentTitle = assessment.AssessmentTitle,
                                     AssessmentDescription = assessment.AssessmentDescription,
                                     AssessmentQuiz = assessment.AssessmentQuiz,
                                     SubjectName = assessment.SubjectName
                                 };

            return assessmentList;
        }

        /// <summary>
        /// The GetStudentAssessments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{StudentAssessment}}"/>.</returns>
        public async Task<IEnumerable<StudentAssessment>> GetStudentAssessments(string schoolId, string classId)
        {
            TableQuery<Entites.StudentAssessment> assessmentQuery = new TableQuery<Entites.StudentAssessment>()
               .Where(TableQuery.GenerateFilterCondition("ClassId", QueryComparisons.Equal, classId));
            var assessments = await _tableStorage.QueryAsync<Entites.StudentAssessment>("StudentAssessments", assessmentQuery);

            var assessmentList = from assessment in assessments
                                 where assessment.Active.GetValueOrDefault(false)
                                 orderby assessment.UpdatedOn descending
                                 select new StudentAssessment
                                 {
                                     Id = assessment.RowKey,
                                     CreatedDate = assessment.Timestamp.DateTime,
                                     AssessmentId = assessment.AssessmentId,
                                     AssessmentAnswers = assessment.AssessmentAnswers,
                                     Attempts = assessment.Attempts,
                                     StudentId = assessment.StudentId,
                                     StudentName = assessment.StudentName
                                 };

            return assessmentList;
        }
    }
}
