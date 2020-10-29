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
    /// Defines the <see cref="AssignmentService" />.
    /// </summary>
    public class AssignmentService : IAssignmentService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        public AssignmentService(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        /// <summary>
        /// The CreateStudentAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="StudentAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateStudentAssigments(StudentAssignment model)
        {
            var assignmentId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

            var assignment = new Entites.StudentAssignment(model.SchoolId, assignmentId)
            {

                StudentId = model.StudentId,
                StudentName = model.StudentName,
                AssignmentId = model.AssignmentId,
                AssignmentURL = model.AssignmentURL,

                Active = true,
                CreatedBy = model.StudentId,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = model.StudentId,
            };

            try
            {
                await _tableStorage.AddAsync("StudentAssignment", assignment);
            }
            catch (Exception ex)
            {
                throw new AppException("Create student assigment error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The CreateTeacherAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="TeacherAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateTeacherAssigments(TeacherAssignment model)
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
            }
            catch (Exception ex)
            {
                throw new AppException("Create teacher assignments error: ", ex.InnerException);
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
                                                               CreatedDate = sa.Timestamp.DateTime
                                                           }).ToList()
                                 };

            return assignmentList;
        }
    }
}
