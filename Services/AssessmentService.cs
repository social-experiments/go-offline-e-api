namespace goOfflineE.Services
{
    using goOfflineE.Common.Enums;
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using goOfflineE.Repository;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;
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
        /// Defines the jsonSettings.
        /// </summary>
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Defines the _pushNotificationService.
        /// </summary>
        private readonly IPushNotificationService _pushNotificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        /// <param name="pushNotificationService">The pushNotificationService<see cref="IPushNotificationService"/>.</param>
        public AssessmentService(ITableStorage tableStorage, IPushNotificationService pushNotificationService)
        {
            _tableStorage = tableStorage;
            _pushNotificationService = pushNotificationService;
        }

        /// <summary>
        /// The CreateAssessment.
        /// </summary>
        /// <param name="model">The model<see cref="Assessment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateAssessment(Assessment model)
        {
            var assessments = await _tableStorage.GetAllAsync<Entites.Assessment>("Assessments");
            var assessment = assessments.SingleOrDefault(assessment => assessment.RowKey == model.Id);

            if (assessment != null)
            {
                assessment.AssessmentDescription = model.AssessmentDescription;
                assessment.AssessmentTitle = model.AssessmentTitle;
                assessment.SubjectName = model.SubjectName;
                assessment.Active = model.Active;

                try
                {
                    await _tableStorage.UpdateAsync("Assessments", assessment);
                }
                catch (Exception ex)
                {
                    throw new AppException("update assessment error: ", ex.InnerException);
                }
            }
            else
            {
                var assessmentId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

                assessment = new Entites.Assessment(model.SchoolId, assessmentId)
                {

                    AssessmentTitle = model.AssessmentTitle,
                    AssessmentDescription = model.AssessmentDescription,
                    AssessmentQuiz = JsonConvert.SerializeObject(model.AssessmentQuestions),
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
                ClassId = model.ClassId,
                AssessmentAnswers = JsonConvert.SerializeObject(model.AssessmentAnswers),

                Active = true,
                CreatedBy = model.StudentId,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = model.StudentId,
            };

            try
            {
                await _tableStorage.AddAsync("StudentAssessments", studentAssessment)
                    await SendPushNotificationToTeacher(model);
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
        /// <returns>The <see cref="Task{IEnumerable{Assessment}}"/>.</returns>
        public async Task<IEnumerable<Assessment>> GetAssessments(string schoolId)
        {

            var dbAssessments = await _tableStorage.GetAllAsync<Entites.Assessment>("Assessments");

            var assessmentList = from assessment in dbAssessments
                                 where assessment.Active.GetValueOrDefault(false)
                                 orderby assessment.UpdatedOn descending
                                 select new Assessment
                                 {
                                     Id = assessment.RowKey,
                                     CreatedDate = assessment.Timestamp.DateTime,
                                     AssessmentTitle = assessment.AssessmentTitle,
                                     AssessmentDescription = assessment.AssessmentDescription,
                                     Questions = assessment.AssessmentQuiz,
                                     ClassId = assessment.ClassId,
                                     SubjectName = assessment.SubjectName,
                                     Active = assessment.Active
                                 };
            List<Assessment> assessments = new List<Assessment>();
            foreach (var assessment in assessmentList)
            {
                var studAssessment = (await GetStudentAssessments(schoolId, assessment.Id)).ToList();
                var assessmentQuestions = string.IsNullOrEmpty(assessment.Questions) ?
                    new List<Question>() : JsonConvert.DeserializeObject<List<Question>>(assessment.Questions, jsonSettings);
                var questions = assessmentQuestions.Where(a => a.Active.GetValueOrDefault(false)).Select(a => a).ToList();
                assessment.Questions = "";
                assessment.StudentAssessments.AddRange(studAssessment);
                assessment.AssessmentQuestions.AddRange(questions);
                assessments.Add(assessment);
            }

            return assessments;
        }

        /// <summary>
        /// The GetAssessments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <param name="studentId">The studentId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{Assessment}}"/>.</returns>
        public async Task<IEnumerable<Assessment>> GetAssessments(string schoolId, string classId, string studentId)
        {
            var dbAssessmentShare = await _tableStorage.GetAllAsync<Entites.AssessmentShare>("AssessmentShare");
            var dbAssessments = await _tableStorage.GetAllAsync<Entites.Assessment>("Assessments");
            var studentAssessments = await GetStudentAssessments(schoolId, classId, studentId);

            var studAssessmentIds = studentAssessments.Select(s => s.AssessmentId);

            var assessmentList = from s in dbAssessmentShare.Where(d => d.PartitionKey == schoolId && d.ClassId == classId)
                                 join a in dbAssessments on s.AssessmentId equals a.RowKey

                                 orderby a.UpdatedOn descending
                                 where a.Active.GetValueOrDefault(false) && !studAssessmentIds.Contains(a.RowKey)
                                 select new Assessment
                                 {
                                     Id = a.RowKey,
                                     CreatedDate = a.Timestamp.DateTime,
                                     AssessmentTitle = a.AssessmentTitle,
                                     AssessmentDescription = a.AssessmentDescription,
                                     Questions = a.AssessmentQuiz,
                                     ClassId = s.ClassId,
                                     SubjectName = a.SubjectName,
                                     Active = a.Active
                                 };
            List<Assessment> assessments = new List<Assessment>();
            foreach (var assessment in assessmentList)
            {
                var assessmentQuestions = string.IsNullOrEmpty(assessment.Questions) ?
                   new List<Question>() : JsonConvert.DeserializeObject<List<Question>>(assessment.Questions, jsonSettings);
                var questions = assessmentQuestions.Where(a => a.Active.GetValueOrDefault(false)).Select(a => a).ToList();
                assessment.Questions = "";
                assessment.AssessmentQuestions.AddRange(questions);
                assessments.Add(assessment);
            }
            return assessments;
        }

        /// <summary>
        /// The GetStudentAssessments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <param name="studentId">The studentId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{StudentAssessment}}"/>.</returns>
        public async Task<IEnumerable<StudentAssessment>> GetStudentAssessments(string schoolId, string classId, string studentId)
        {
            TableQuery<Entites.StudentAssessment> assessmentQuery = new TableQuery<Entites.StudentAssessment>()
               .Where(TableQuery.GenerateFilterCondition("ClassId", QueryComparisons.Equal, classId));
            var assessmentDB = await _tableStorage.QueryAsync<Entites.StudentAssessment>("StudentAssessments", assessmentQuery);

            var assessmentList = from assessment in assessmentDB
                                 where assessment.Active.GetValueOrDefault(false) &&
                                        assessment.StudentId == studentId
                                 orderby assessment.UpdatedOn descending
                                 select new StudentAssessment
                                 {
                                     Id = assessment.RowKey,
                                     CreatedDate = assessment.Timestamp.DateTime,
                                     AssessmentId = assessment.AssessmentId,
                                     Answers = assessment.AssessmentAnswers,
                                     StudentId = assessment.StudentId,
                                     StudentName = assessment.StudentName,
                                     Active = assessment.Active
                                 };

            List<StudentAssessment> assessments = new List<StudentAssessment>();
            foreach (var assessment in assessmentList)
            {
                var assessmentQuestions = string.IsNullOrEmpty(assessment.Answers) ?
                   new List<Answer>() : JsonConvert.DeserializeObject<List<Answer>>(assessment.Answers, jsonSettings);
                var questions = assessmentQuestions.Select(a => a).ToList();
                assessment.Answers = "";
                assessment.AssessmentAnswers.AddRange(questions);
                assessments.Add(assessment);
            }
            return assessments;
        }

        /// <summary>
        /// The UpdateAssessmentQuestion.
        /// </summary>
        /// <param name="model">The model<see cref="Question"/>.</param>
        /// <param name="assessmentId">The assessmentId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task UpdateAssessmentQuestion(Question model, string assessmentId)
        {
            var assessments = await _tableStorage.GetAllAsync<Entites.Assessment>("Assessments");
            var assessment = assessments.SingleOrDefault(assessment => assessment.RowKey == assessmentId);

            if (assessment != null)
            {
                var assessmentQuestions = assessment.AssessmentQuiz != null ? JsonConvert.DeserializeObject<List<Question>>(assessment.AssessmentQuiz, jsonSettings) : null;

                var question = assessmentQuestions != null ? assessmentQuestions.FirstOrDefault((ques) => ques.Id == model.Id) : null;

                if (question != null)
                {
                    assessmentQuestions = assessmentQuestions
                                        .Where(a => a.Id != model.Id)
                                        .Select(r => r).ToList();

                }
                else
                {
                    var studentAssessmentId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;
                    model.Id = studentAssessmentId;
                }
                if (assessmentQuestions == null)
                {
                    assessmentQuestions = new List<Question>();
                }
                assessmentQuestions.Add(model);

                assessment.AssessmentQuiz = JsonConvert.SerializeObject(assessmentQuestions);
                assessment.UpdatedOn = DateTime.UtcNow;

                try
                {
                    await _tableStorage.UpdateAsync("Assessments", assessment);
                }
                catch (Exception ex)
                {
                    throw new AppException("update assessment error: ", ex.InnerException);
                }

            }
        }

        /// <summary>
        /// The AssessmentShare.
        /// </summary>
        /// <param name="model">The model<see cref="AssessmentShare"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<string> AssessmentShare(AssessmentShare model)
        {
            var sharedAssessments = await _tableStorage.GetAllAsync<Entites.AssessmentShare>("AssessmentShare");
            var isShared = sharedAssessments.Any(assessment => assessment.ClassId == model.ClassId && assessment.AssessmentId == model.AssessmentId);

            if (isShared)
            {
                return "Already shared assessment.";
            }

            // Create new content
            var assessmentSharedId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

            var assessmentShare = new Entites.AssessmentShare(model.SchoolId, assessmentSharedId)
            {
                ClassId = model.ClassId,
                AssessmentId = model.AssessmentId,
                Active = true,
                CreatedBy = model.CreatedBy,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = model.CreatedBy,
            };

            try
            {
                await _tableStorage.AddAsync("AssessmentShare", assessmentShare);
                await SendPushNotificationToStudent(model);
                return "Assessment shared successfully.";
            }
            catch (Exception ex)
            {

                throw new AppException("Assessment share error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The GetAssessmentSubjects.
        /// </summary>
        /// <returns>The <see cref="Task{List{string}}"/>.</returns>
        public async Task<List<string>> GetAssessmentSubjects()
        {
            var dbAssessments = await _tableStorage.GetAllAsync<Entites.Assessment>("Assessments");

            return (from a in dbAssessments
                    group a by a.SubjectName into s
                    select s.Key).ToList();
        }

        /// <summary>
        /// The GetStudentAssessments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="assessmentId">The assessmentId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{StudentAssessment}}"/>.</returns>
        private async Task<IEnumerable<StudentAssessment>> GetStudentAssessments(string schoolId, string assessmentId)
        {
            TableQuery<Entites.StudentAssessment> assessmentQuery = new TableQuery<Entites.StudentAssessment>()
               .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, schoolId));
            var assessmentDB = await _tableStorage.QueryAsync<Entites.StudentAssessment>("StudentAssessments", assessmentQuery);

            var assessmentList = from assessment in assessmentDB
                                 where assessment.Active.GetValueOrDefault(false) && assessment.AssessmentId == assessmentId
                                 orderby assessment.UpdatedOn descending
                                 select new StudentAssessment
                                 {
                                     Id = assessment.RowKey,
                                     CreatedDate = assessment.Timestamp.DateTime,
                                     AssessmentId = assessment.AssessmentId,
                                     Answers = assessment.AssessmentAnswers,
                                     StudentId = assessment.StudentId,
                                     StudentName = assessment.StudentName,
                                     ClassId = assessment.ClassId
                                 };

            List<StudentAssessment> assessments = new List<StudentAssessment>();
            foreach (var assessment in assessmentList)
            {
                var assessmentQuestions = string.IsNullOrEmpty(assessment.Answers) ?
                   new List<Answer>() : JsonConvert.DeserializeObject<List<Answer>>(assessment.Answers, jsonSettings);
                var questions = assessmentQuestions.Select(a => a).ToList();
                assessment.Answers = "";
                assessment.AssessmentAnswers.AddRange(questions);
                assessments.Add(assessment);
            }
            return assessments;
        }

        /// <summary>
        /// The SendPushNotificationToStudent.
        /// </summary>
        /// <param name="model">The model<see cref="AssessmentShare"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendPushNotificationToStudent(AssessmentShare model)
        {
            try
            {
                var assessments = await _tableStorage.GetAllAsync<Entites.Assessment>("Assessments");
                var assessment = assessments.SingleOrDefault(a => a.RowKey == model.AssessmentId);

                var pushNotification = new PushNotification
                {
                    Title = $"{assessment.SubjectName}",
                    Body = $"Assessment available, ${assessment.AssessmentTitle}"
                };

                var studentData = await _tableStorage.GetAllAsync<Entites.Student>("Student");
                var students = studentData.Where(s => s.PartitionKey == model.SchoolId && s.ClassId == model.ClassId);
                foreach (var student in students)
                {
                    if (!String.IsNullOrEmpty(student.NotificationToken))
                    {
                        pushNotification.RecipientDeviceToken = student.NotificationToken;
                        await _pushNotificationService.SendAsync(pushNotification);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new AppException("Exception thrown in Notify Service: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The SendPushNotificationToTeacher.
        /// </summary>
        /// <param name="model">The model<see cref="StudentAssessment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendPushNotificationToTeacher(StudentAssessment model)
        {
            try
            {
                var assessments = await _tableStorage.GetAllAsync<Entites.Assessment>("Assessments");
                var assessment = assessments.SingleOrDefault(a => a.RowKey == model.AssessmentId);

                var pushNotification = new PushNotification
                {
                    Title = model.StudentName,
                    Body = $"Assessment {assessment.SubjectName} - {assessment.AssessmentTitle} submitted"
                };

                var userData = await _tableStorage.GetAllAsync<Entites.User>("User");
                var users = userData.Where(u => u.Role == Role.Teacher.ToString() && u.RowKey == assessment.CreatedBy);
                foreach (var user in users)
                {
                    if (!String.IsNullOrEmpty(user.NotificationToken))
                    {
                        pushNotification.RecipientDeviceToken = user.NotificationToken;
                        await _pushNotificationService.SendAsync(pushNotification);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new AppException("Exception thrown in Notify Service: ", ex.InnerException);
            }
        }
    }
}
