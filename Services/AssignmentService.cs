namespace goOfflineE.Services
{
    using goOfflineE.Common.Enums;
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using goOfflineE.Repository;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AssignmentService" />.
    /// </summary>
    public class AssignmentService : IAssignmentService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Defines the _pushNotificationService.
        /// </summary>
        private readonly IPushNotificationService _pushNotificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        /// <param name="pushNotificationService">The pushNotificationService<see cref="IPushNotificationService"/>.</param>
        public AssignmentService(ITableStorage tableStorage, IPushNotificationService pushNotificationService)
        {
            _tableStorage = tableStorage;
            _pushNotificationService = pushNotificationService;
        }

        /// <summary>
        /// The CreateStudentAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="StudentAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateStudentAssigments(StudentAssignment model)
        {


            var studentAssignment = await _tableStorage.GetAllAsync<Entites.StudentAssignment>("StudentAssignment");
            var content = studentAssignment.SingleOrDefault(assignment => assignment.RowKey == model.Id);

            if (content != null)
            {
                content.ReviewStatus = AssignmentReviewStatus.Completed;
                content.UpdatedOn = DateTime.UtcNow;

                try
                {
                    await _tableStorage.UpdateAsync("StudentAssignment", content);
                }
                catch (Exception ex)
                {
                    throw new AppException("update student assignment error: ", ex.InnerException);
                }
            }
            else
            {
                var assignmentId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

                var assignment = new Entites.StudentAssignment(model.SchoolId, assignmentId)
                {

                    StudentId = model.StudentId,
                    StudentName = model.StudentName,
                    AssignmentId = model.AssignmentId,
                    AssignmentURL = model.AssignmentURL,
                    ReviewStatus = AssignmentReviewStatus.UnderReview,

                    Active = true,
                    CreatedBy = model.StudentId,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = model.StudentId,
                };

                try
                {
                    await _tableStorage.AddAsync("StudentAssignment", assignment);
                    await SendPushNotificationToTeacher(model);
                }
                catch (Exception ex)
                {
                    throw new AppException("Create student assigment error: ", ex.InnerException);
                }
            }
        }

        /// <summary>
        /// The CreateTeacherAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="TeacherAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateTeacherAssigments(TeacherAssignment model)
        {
            var assignments = await _tableStorage.GetAllAsync<Entites.Assignment>("Assignment");
            var content = assignments.SingleOrDefault(assignment => assignment.RowKey == model.Id);

            if (content != null)
            {
                content.Active = model.Active;
                content.UpdatedOn = DateTime.UtcNow;
                content.UpdatedBy = model.CreatedBy;

                try
                {
                    await _tableStorage.UpdateAsync("Assignment", content);
                }
                catch (Exception ex)
                {
                    throw new AppException("Update assignment error: ", ex.InnerException);
                }

            }
            else
            {
                var assignmentId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

                var assignment = new Entites.Assignment(model.SchoolId, assignmentId)
                {

                    AssignmentName = model.AssignmentName,
                    AssignmentDescription = model.AssignmentDescription,
                    AssignmentURL = model.AssignmentURL,
                    ClassId = model.ClassId,
                    SubjectName = model.SubjectName,

                    Active = true,
                    CreatedBy = model.CreatedBy,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = model.CreatedBy,
                };
                try
                {
                    await _tableStorage.AddAsync("Assignment", assignment);
                    await SendPushNotificationToStudent(model);
                }
                catch (Exception ex)
                {
                    throw new AppException("Create assignment error: ", ex.InnerException);
                }
            }
        }

        /// <summary>
        /// The GetAssignments.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{TeacherAssignment}}"/>.</returns>
        public async Task<IEnumerable<TeacherAssignment>> GetAssignments(string schoolId, string classId)
        {
            TableQuery<Entites.Assignment> assignmentQuery = new TableQuery<Entites.Assignment>()
                 .Where(TableQuery.GenerateFilterCondition("ClassId", QueryComparisons.Equal, classId));
            var assignments = await _tableStorage.QueryAsync<Entites.Assignment>("Assignment", assignmentQuery);

            TableQuery<Entites.StudentAssignment> query = new TableQuery<Entites.StudentAssignment>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, schoolId));
            var studentAssignments = await _tableStorage.QueryAsync<Entites.StudentAssignment>("StudentAssignment", query);

            var assignmentList = from assignment in assignments
                                 join sassignment in studentAssignments
                                on assignment.RowKey equals sassignment.AssignmentId into studassignments
                                 where assignment.Active.GetValueOrDefault(false)
                                 orderby assignment.UpdatedOn descending
                                 select new TeacherAssignment
                                 {
                                     Id = assignment.RowKey,
                                     CreatedDate = assignment.Timestamp.DateTime,
                                     AssignmentName = assignment.AssignmentName,
                                     AssignmentDescription = assignment.AssignmentDescription,
                                     AssignmentURL = assignment.AssignmentURL,
                                     SubjectName = assignment.SubjectName,
                                     StudentAssignments = (from sa in studassignments
                                                           select new StudentAssignment
                                                           {
                                                               AssignmentId = sa.AssignmentId,
                                                               AssignmentURL = sa.AssignmentURL,
                                                               SchoolId = sa.PartitionKey,
                                                               Id = sa.RowKey,
                                                               StudentId = sa.StudentId,
                                                               StudentName = sa.StudentName,
                                                               CreatedDate = sa.UpdatedOn.GetValueOrDefault(DateTime.UtcNow),
                                                               ReviewStatus = sa.ReviewStatus
                                                           }).ToList()
                                 };

            return assignmentList;
        }

        /// <summary>
        /// The SendPushNotificationToStudent.
        /// </summary>
        /// <param name="assignment">The assignment<see cref="TeacherAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendPushNotificationToStudent(TeacherAssignment assignment)
        {
            try
            {
                var pushNotification = new PushNotification
                {
                    Title = assignment.SubjectName,
                    Body = assignment.AssignmentName
                };

                var studentData = await _tableStorage.GetAllAsync<Entites.Student>("Student");
                var students = studentData.Where(s => s.PartitionKey == assignment.SchoolId && s.ClassId == assignment.ClassId);
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
        /// <param name="studAssignment">The studAssignment<see cref="StudentAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendPushNotificationToTeacher(StudentAssignment studAssignment)
        {
            try
            {
                var assignments = await _tableStorage.GetAllAsync<Entites.Assignment>("Assignment");
                var assignment = assignments.SingleOrDefault(assignment => assignment.RowKey == studAssignment.AssignmentId);

                var pushNotification = new PushNotification
                {
                    Title = studAssignment.StudentName, 
                    Body = $"Upload assignment: {assignment.AssignmentName}"
                };

                var userData = await _tableStorage.GetAllAsync<Entites.User>("User");
                var user = userData.FirstOrDefault(s => s.RowKey == assignment.CreatedBy);
                if (!String.IsNullOrEmpty(user.NotificationToken))
                {
                    pushNotification.RecipientDeviceToken = user.NotificationToken;
                    await _pushNotificationService.SendAsync(pushNotification);
                }

            }
            catch (Exception ex)
            {
                throw new AppException("Exception thrown in Notify Service: ", ex.InnerException);
            }

    }
}
}
